using LinkLookupSubscriptionApi.Services.Interfaces;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinkLookupSubscriptionApi.Services
{
    public class MongoDbDataRepository<T> : IDataRepository<T> where T : new()
    {
        IMongoDatabase _mongoDb;

        private readonly string _tableName;

        /// <summary>
        /// Constructor that connects to mongoDb by connection string
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="databaseName"></param>
        public MongoDbDataRepository(string connectionString, string databaseName, string tableName)
        {
            var client = new MongoClient(connectionString);
            _mongoDb = client.GetDatabase(databaseName);
            _tableName = tableName;
        }

        /// <summary>
        /// Constructor that connects to localhost mongoDb
        /// </summary>
        /// <param name="databaseName"></param>
        public MongoDbDataRepository(string databaseName, string tableName)
        {
            var client = new MongoClient(); // Connects to localhost if no connection string is provided
            _mongoDb = client.GetDatabase(databaseName);
            _tableName = tableName;
        }

        public T Read(string id)
        {
            return new T();
        }

        public List<T> ReadAll()
        {
            return new List<T>();
        }

        public bool Write(T obj)
        {
            try
            {
                var collection = _mongoDb.GetCollection<T>(_tableName);
                collection.InsertOne(obj);
                return true;
            }
            catch (Exception e)
            {
                //TODO: Add logging of exceptions
                return false;
            }
            
        }

        public bool Update(T obj)
        {
            return true;
        }

        public bool Delete(string id)
        {
            return true;
        }
    }
}
