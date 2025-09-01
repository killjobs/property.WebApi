using property.Domain.Dtos;
using System.Net;
using System.Runtime.CompilerServices;

namespace property.Tests.Builders
{
    public class OwnerDtoBuilder
    {
        private string _name = "Julian";
        private string _address = "1st Street";
        private string _photo = Convert.ToBase64String(new byte[0]);
        private DateTime _birthday = DateTime.Now;

        public OwnerDtoBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public OwnerDtoBuilder WithAddress(string address)
        {
            _address = address;
            return this;
        }

        public OwnerDtoBuilder WithPhoto(string photo)
        {
            _photo = photo;
            return this;
        }

        public OwnerDtoBuilder WithBirthday(DateTime birthday)
        {
            _birthday = birthday;
            return this;
        }

        public OwnerDto Build()
        {
            return new OwnerDto
            {
                Name = _name,
                Address = _address,
                Photo = _photo,
                Birthday = _birthday
            };
        }
    }
}
