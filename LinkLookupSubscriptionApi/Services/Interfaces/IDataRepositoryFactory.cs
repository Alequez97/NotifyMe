using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinkLookupSubscriptionApi.Services.Interfaces
{
    /// <summary>
    /// Factory for creating data access service
    /// </summary>
    public interface IDataRepositoryFactory 
    {
        /// <summary>
        /// Method that creates data access service
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableName">Optional parameter if tableName is required, for example for mongoDb</param>
        /// <returns>Ready to use data access serivce object</returns>
        IDataRepository<T> Get<T>(string tableName = null) where T : new();
    }
}
