using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordManagerAPI.Contexts
{
    /// <summary>
    /// Interface contract used by context handlers.
    /// </summary>
    public interface IContext
    {
        IDbConnection CreateConnection(string connectionName);
    }
}
