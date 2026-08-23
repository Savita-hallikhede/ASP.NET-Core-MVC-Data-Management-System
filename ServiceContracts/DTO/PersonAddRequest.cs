using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO
{
    public class PersonAddRequest
    {
        public string? PersonName { get; set; }
        public string? PersonEmail { get; set; }

        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }

        public Guid? CountryId { get; set; }
        public string? Country { get; set; }
        public string? Address { get; set; }
        public bool ReceiveNewsLetters { get; set; }
    }
}
