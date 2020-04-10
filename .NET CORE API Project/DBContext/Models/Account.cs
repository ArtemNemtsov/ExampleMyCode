using System;
using System.Collections.Generic;

namespace DBContext.Models
{
    public partial class Account
    {
        public Account()
        {
            User = new HashSet<User>();
        }

        public int IdAccount { get; set; }
        public int IdTypeAccount { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        public virtual TypeAccount IdTypeAccountNavigation { get; set; }
        public virtual ICollection<User> User { get; set; }
    }
}
