using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filminurk.Core.Dto
{
    public class ActorsDTO
    {
        [Key]
        public Guid ActorID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Nickname { get; set; }
        public List<string>? MoviesActedFor { get; set; }
        public Guid? PortraitID { get; set; }

        // Enda 3 andmed

        public IEnumerable<string>? Health { get; set; }
        public int? Salary { get; set; }
        public string? Description { get; set; }
    }
}
