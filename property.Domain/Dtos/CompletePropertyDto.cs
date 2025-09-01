namespace property.Domain.Dtos
{
    public class CompletePropertyDto
    {
        public string IdOwner { get; set; }
        public string NameOwner { get; set; }
        public List<ListPropertiesDto> ListProperties { get; set; }
    }
}
