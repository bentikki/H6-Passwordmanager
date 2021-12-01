using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordManagerAPI.Models.Sitekeys
{
    public class CreateSitekeyResponse : ISitekey
    {
        public int Id { get; set; }
        public int FK_Users_Id { get; set; }
        public string Sitename { get; set; }
        public string LoginName { get; set; }
        public string LoginPassword { get; set; }
    }
}
