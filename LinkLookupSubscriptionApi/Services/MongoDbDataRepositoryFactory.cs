using DomainEntites;
using LinkLookupSubscriptionApi.Services.Interfaces;
using System;

namespace LinkLookupSubscriptionApi.Services
{
    public class MongoDbDataRepositoryFactory : IDataRepositoryFactory
	{
        public IDataRepository<T> Get<T>(string tableName = null) where T : ModelBase, new()
        {
            if (tableName == null)
            {
                throw new NullReferenceException(nameof(tableName));
            }

            var databaseName = "NotifyMe"; // Will be read from configuration
            var dataRepository = new MongoDbDataRepository<T>(databaseName, tableName);
            return dataRepository;
        }
    }
}
