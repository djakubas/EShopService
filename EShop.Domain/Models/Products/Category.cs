namespace EShop.Domain.Models.Products
{
    public class Category
    {
        /// <summary>
        /// Category Name, default "Unknown"
        /// </summary>
        public string Name { get; set; } = "Unknown";
        public Guid CategoryId { get; set; }
    }
}