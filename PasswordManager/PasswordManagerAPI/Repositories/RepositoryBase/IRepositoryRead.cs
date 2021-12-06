using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManagerAPI.Repositories.RepositoryBase
{
    /// <summary>
    /// Interface to hold contract for repositories with read access.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity to used by the repository.</typeparam>
    /// <typeparam name="KPrimaryKeyType">The datatype of the entity's unique identifier.</typeparam>
    //public interface IRepositoryRead<TEntity, KPrimaryKeyType>
    //{
    //    Task<TEntity> GetAsync(KPrimaryKeyType id);
    //    Task<IEnumerable<TEntity>> GetAllAsync();
    //}
}
