using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPlace.Models
{
    public class CreateConnectorRequestDto
    {
        public string Name { get; set; }
        public string Icon { get; set; }
        public Guid CategoryId { get; set; }
    }
}
