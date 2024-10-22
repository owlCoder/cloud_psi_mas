using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class User
    {
        public required  string UserId { get; set; }
        public required string Fullname { get; set; }

        public User()
        {
            UserId = Guid.NewGuid().ToString().Replace("-", "");
        }
    }
}
