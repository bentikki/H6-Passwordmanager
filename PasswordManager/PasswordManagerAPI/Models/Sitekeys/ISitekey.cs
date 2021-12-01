using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordManagerAPI.Models.Sitekeys
{
    public interface ISitekey
    {
        int Id { get; }
        int FK_Users_Id { get; }
        string Sitename { get; }
        string LoginName { get; }
        string LoginPassword { get; }
    }
}
