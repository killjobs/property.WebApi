using property.Domain.Entities;

namespace property.Tests.Builders
{
    public class PropertyBuilder
    {
        private string _idProperty = Guid.NewGuid().ToString();
        private string _name = "First Building";
        private string _address = "10th Main Street";
        private double _price = 250000;
        private long _codeInternal = 1;
        private int _year = 2015;
        private string _idOwner = Guid.NewGuid().ToString();

        public PropertyBuilder WithIdProperty(string idProperty)
        {
            _idProperty = idProperty;
            return this;
        }

        public PropertyBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public PropertyBuilder WithAddress(string address)
        {
            _address = address;
            return this;
        }

        public PropertyBuilder WithPrice(double price)
        {
            _price = price;
            return this;
        }

        public PropertyBuilder WithCodeInternal(long codeInternal)
        {
            _codeInternal = codeInternal;
            return this;
        }

        public PropertyBuilder WithYear(int year)
        {
            _year = year;
            return this;
        }

        public PropertyBuilder WithIdOwner(string idOwner)
        {
            _idOwner = idOwner;
            return this;
        }

        public Property Build()
        {
            return new Property
            {
                IdProperty = _idProperty,
                Name = _name,
                Address = _address,
                Price = _price,
                CodeInternal = _codeInternal,
                Year = _year,
                IdOwner = _idOwner
            };
        }
    }
}
