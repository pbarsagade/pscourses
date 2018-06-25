using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class NewAuthorDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTimeOffset DateOfBirth { get; set; }
        public string Genre { get; set; }

        public ICollection<NewBookDto> Books { get; set; } = new List<NewBookDto>();
    }
}
