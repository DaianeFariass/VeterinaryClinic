using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace VeterinaryClinic.Data.Entities
{
    public class User : IdentityUser
    {     
        public string FirstName { get; set; }
     
        public string LastName { get; set; }
    }
}
