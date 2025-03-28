namespace EShopService.Models
{
    public class Product : BaseModel
    {
        /// <summary>
        /// Unikalny identyfikator produktu
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Nazwa produktu
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Kod kreskowy
        /// </summary>
        public string? Ean { get; set; }

        /// <summary>
        /// Cena produktu
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Ilość dostępnych sztuk. Domyślna wartość 0
        /// </summary>
        public int Stock { get; set; } = 0;

        /// <summary>
        /// SKU (ang. Stock Keeping Unit) to alfanumeryczny kod, który można przypisać do produktu w katalogu
        /// </summary>
        public string? Sku { get; set; }

        /// <summary>
        /// Kategoria produktu
        /// </summary>
        public Category? Category { get; set; }        

    }
}
