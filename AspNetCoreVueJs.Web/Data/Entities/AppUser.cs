using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AspNetCoreVueJs.Web.Data.Entities
{
    public class AppUser : IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string RefreshToken { get; set; }
        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";

        public List<Order> Orders { get; set; } = new List<Order>();
    }
}
