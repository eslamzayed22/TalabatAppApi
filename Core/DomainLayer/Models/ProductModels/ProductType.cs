namespace DomainLayer.Models.ProductModels
{
    public class ProductType : BaseEntity<int>
    {
        public string Name { get; set; } = null!;
    }
}