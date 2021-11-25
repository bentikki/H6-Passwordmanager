using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordManagerAPI.Contexts
{
    /// <summary>
    /// Connection Context helper for Dapper implementation.
    /// Grants connection access to database, based on connectionstrings in appserrings.json.
    /// </summary>
    public class DapperContext
    {
        private readonly IConfiguration _configuration;
        public DapperContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Returns IDbConnection matching the named connectionstring, provided in appsettings.json.
        /// </summary>
        /// <param name="connectionName">The name of the connectionstring to use.</param>
        /// <returns>SqlConnection from the provided connectionstring</returns>
        public IDbConnection CreateConnection(string connectionName)
            => new SqlConnection(_configuration.GetConnectionString(connectionName));
    }
}
