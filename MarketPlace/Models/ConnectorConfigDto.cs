using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPlace.Models
{
    public class ConnectorConfigDto
    {
        public Guid Id { get; set; }
        public string KeyName { get; set; }
        public string DataType { get; set; }
        public Guid ConnectorId { get; set; }
        public ConnectorDto Connector { get; set; }

    }
}
