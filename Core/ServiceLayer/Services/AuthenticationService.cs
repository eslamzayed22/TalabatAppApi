using AutoMapper;
using DomainLayer.Exceptions;
using DomainLayer.Models.IdentityModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ServiceAbstractionLayer;
using Shared.DataTransferObjects.IdentityDtos;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Services
{
    public class AuthenticationService(UserManager<ApplicationUser> _userManager,
        IConfiguration _configuration, IMapper _mapper) : IAuthenticationService
    {
        public async Task<bool> CheckEmailAsync(string email)
        {
            var user = await  _userManager.FindByEmailAsync(email);
            return user is not null;
        }

        public async Task<AddressDto> GetCurrentUserAddressAsync(string email)
        {
            var user = await _userManager.Users.Include(u => u.Address).FirstOrDefaultAsync(u => u.Email == email) ?? throw new UserNotFoundException(email);
            if (user.Address is not null)
            {
                return _mapper.Map<Address, AddressDto>(user.Address);
            }
            else throw new AddressNotFoundException(user.UserName!);
        }

        public async Task<AddressDto> UpdateCurrentUserAddressAsync(string email, AddressDto addressDto)
        {
            var user = await _userManager.Users.Include(u => u.Address).FirstOrDefaultAsync(u => u.Email == email) 
                                    ?? throw new UserNotFoundException(email);
            if (user.Address is not null) //Update existing address
            {
                _mapper.Map(addressDto, user.Address);
            }
            else //Add new address
            {
                var newAddress = _mapper.Map<AddressDto, Address>(addressDto);
                user.Address = newAddress;
            }
            await _userManager.UpdateAsync(user);
            return _mapper.Map<AddressDto>(user.Address);
        }

        public async Task<UserDto> GetCurrentUserAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email) ?? throw new UserNotFoundException(email);
            return new UserDto()
            {
                Email = user.Email,
                DisplayName = user.DisplayName,
                Token = await CreateTokenAsync(user)
            };
        }

        public async Task<UserDto> LoginAsync(LoginDto loginDto)
        {

            //Check if email exists
            var user = await _userManager.FindByEmailAsync(loginDto.Email) ?? throw new UserNotFoundException(loginDto.Email); ;

            //Check if password is correct
            var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (isPasswordValid)
            {
                return new UserDto
                {
                    Email = user.Email,
                    DisplayName = user.DisplayName,
                    Token = await CreateTokenAsync(user)
                };
            }
            else throw new UnauthorizedException();
        }

        public async Task<UserDto> RegisterAsync(RegisterDto registerDto)
        {
            var appUser = new ApplicationUser
            {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                PhoneNumber = registerDto.PhoneNumber,
                UserName = registerDto.UserName
            };

            //Create user
            var res = await _userManager.CreateAsync(appUser, registerDto.Password);
            if (res.Succeeded)
            {
                return new UserDto
                {
                    DisplayName = appUser.DisplayName,
                    Email = appUser.Email,
                    Token = await CreateTokenAsync(appUser)
                };
            }
            else
            {
                var errors = res.Errors.Select(e => e.Description).ToList();
                throw new BadRequestException(errors);
            }
        }


        private async Task<string> CreateTokenAsync(ApplicationUser user)
        {
            #region JWT Token Payload
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.NameIdentifier, user.Id!)
            };
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            #endregion

            var secretKey = _configuration.GetSection("JWTOptions")["SecretKey"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration.GetSection("JWTOptions")["Issuer"],
                audience: _configuration.GetSection("JWTOptions")["Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
                );

            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}
