namespace EShopService.Models
{
    public abstract class BaseModel
    {   
        /// <summary>
        /// Określenie aktywności produktu
        /// </summary>
        public bool Deleted { get; set; }
        /// <summary>
        /// Data utworzenia rekordu
        /// </summary>
        public DateTime CreatedAt { get; set; }
        /// <summary>
        /// Identyfikator Autora
        /// </summary>
        public Guid CreatedBy { get; set; }
        /// <summary>
        /// Data ostatniej aktualizacji rekordu
        /// </summary>
        public DateTime? UpdatedAt { get; set; }
        /// <summary>
        /// Identyfikator Edytora
        /// </summary>
        public Guid? UpdatedBy { get; set; }

    }
}
