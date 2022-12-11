using DataAnnotationsExtensions;
using System.ComponentModel;

namespace TodoWebApp.Model
{
    public class User
    {
        public int UserId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string fullName => firstName + ", " + lastName;
        [Email]
        public string email { get; set; }
        [PasswordPropertyText]
        public string password { get; set; }
        public string PhoneNumber { get; set; }
    }
}
