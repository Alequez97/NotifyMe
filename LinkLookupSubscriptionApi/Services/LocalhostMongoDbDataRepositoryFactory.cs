using LinkLookupSubscriptionApi.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinkLookupSubscriptionApi.Services
{
    public class LocalhostMongoDbDataRepositoryFactory : IDataRepositoryFactory
    {
        public IDataRepository<T> Create<T>(string tableName = null) where T : new()
        {
            if (tableName == null)
            {
                throw new NullReferenceException(nameof(tableName));
            }

            var databaseName = "NotifyMeDatabase"; // Will be read from configuration
            var dataRepository = new MongoDbDataRepository<T>(databaseName, tableName);
            return dataRepository;
        }
    }
}
