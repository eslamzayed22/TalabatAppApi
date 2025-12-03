namespace DomainLayer.Models.ProductModels
{
    public class ProductBrand : BaseEntity<int>
    {
       public string Name { get; set; } = null!;
    }
}