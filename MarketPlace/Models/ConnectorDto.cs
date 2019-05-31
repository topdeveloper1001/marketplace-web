using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPlace.Models
{
    public class ConnectorDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public Guid CategoryId { get; set; }
        public CategoryDto Category { get; set; }
        public IList<ConnectorConfigDto> ConnectorConfigs { get; set; }

    }
}
