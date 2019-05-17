using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPlace.Models
{
    public class CreateDeviceRequestDto
    {

        public string deviceIdentifier { get; set; }
        public string ipAddress { get; set; }
        public string type { get; set; }
        public Guid? clientId { get; set; }
        public Guid? buildingId { get; set; }
        public Guid? networkId { get; set; }
    }
}
