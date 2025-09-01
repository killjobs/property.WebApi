using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace property.Domain.Dtos
{
    public class CompletePropertyRequestDto
    {
        public string? NameProperty { get; set; }
        public string? AddressProperty { get; set; }
        public double MinPrice { get; set; }
        public double MaxPrice { get; set; }
    }
}
