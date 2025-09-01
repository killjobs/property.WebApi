namespace property.Domain.Dtos
{
    public class ListPropertiesDto
    {
        public string IdProperty { get; set; }
        public string? IdPropertyImage { get; set; }
        public string NameProperty { get; set; }
        public string AddressProperty { get; set; }
        public double Price { get; set; }
        public long CodeInternal { get; set; }
        public byte[] File { get; set; }
    }
}
