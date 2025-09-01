using property.Domain.Entities;

namespace property.Tests.Builders
{
    public class OwnerBuilder
    {
        private string _idOwner = Guid.NewGuid().ToString();
        private string _name = "Julian";
        private string _address = "1st Street";
        private byte[] _photo = new byte[0];
        private DateTime _birthday = DateTime.Now;

        public OwnerBuilder WithIdOwner(string id)
        {
            _idOwner = id;
            return this;
        }

        public OwnerBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public OwnerBuilder WithAddress(string address)
        {
            _address = address;
            return this;
        }

        public OwnerBuilder WithPhoto(byte[] photo)
        {
            _photo = photo;
            return this;
        }

        public OwnerBuilder WithBirthday(DateTime birthday)
        {
            _birthday = birthday;
            return this;
        }

        public Owner Build()
        {
            return new Owner
            {
                IdOwner = _idOwner,
                Name = _name,
                Address = _address,
                Photo = _photo,
                Birthday = _birthday
            };
        }
    }
}
