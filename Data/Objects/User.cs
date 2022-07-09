using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CashLad.Data
{
    public class User : BaseEntity
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string PassHash { get; set; }
        public string Salt { get; set; }

        public bool IsAdmin { get; set; }

        public string SessionId { get; set; }
    }
}
