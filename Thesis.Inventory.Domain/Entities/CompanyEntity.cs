namespace Thesis.Inventory.Domain.Entities
{
    public class CompanyEntity : BaseEntity
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public string ContractNumber { get; set; }
        public int ProvinceId { get; set; }
    }
}
