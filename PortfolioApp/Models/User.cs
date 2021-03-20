using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortfolioApp.Models
{
    public class User
    {
        public int UserId { get; set; }
        
        public string FullName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }  

        public string BirthDate { get; set; }

        public string PhotoFileName { get; set; }

        public string UserRole { get; set; }

    }
}
