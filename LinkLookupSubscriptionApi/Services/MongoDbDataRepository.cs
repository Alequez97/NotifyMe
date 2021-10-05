using LinkLookupSubscriptionApi.Models;
using LinkLookupSubscriptionApi.Services.Interfaces;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace LinkLookupSubscriptionApi.Services
{
    public class MongoDbDataRepository<T> : IDataRepository<T> where T : ModelBase, new()
    {
        protected readonly IMongoDatabase _mongoDb;
        protected readonly IMongoCollection<T> _collection;
        protected readonly string _tableName;

        /// <summary>
        /// Constructor that connects to mongoDb by connection string
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="databaseName"></param>
        public MongoDbDataRepository(string connectionString, string databaseName, string tableName)
        {
            var client = new MongoClient(connectionString);
            _mongoDb = client.GetDatabase(databaseName);
            _collection = _mongoDb.GetCollection<T>(tableName);
            _tableName = tableName;
        }

        /// <summary>
        /// Constructor that connects to localhost mongoDb
        /// </summary>
        /// <param name="databaseName"></param>
        public MongoDbDataRepository(string databaseName, string tableName)
        {
            var client = new MongoClient();
            _mongoDb = client.GetDatabase(databaseName);
            _tableName = tableName;
        }

        public T Read(string id)
        {
            try
            {
                var user = _collection.Find(x => x.Id == Guid.Parse(id)).FirstOrDefault();
                return user;
            }
            catch (Exception e)
            {
                //TODO: Add logging of exceptions
                return null;
            }
        }

        public List<T> ReadAll()
        {
            try
            {
                var users = _collection.AsQueryable().ToList();
                return users;
            }
            catch (Exception e)
            {
                //TODO: Add logging of exceptions
                return null;
            }
        }

        public bool Write(T obj)
        {
            try
            {
                _collection.InsertOne(obj);
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
            try
            {
                var record = _collection.FindOneAndReplace(x => x.Id == obj.Id, obj);
                return true;
            }
            catch (Exception e)
            {
                //TODO: Add logging of exceptions
                return false;
            }
        }

        public bool Delete(string id)
        {
            try
            {
                var record = _collection.DeleteOne(x => x.Id == Guid.Parse(id));
                return true;
            }
            catch (Exception e)
            {
                //TODO: Add logging of exceptions
                return false;
            }
        }

        public T FindByExpression(Expression<Func<T, bool>> expression)
        {
            try
            {
                var obj = _collection.Find(expression).FirstOrDefault();
                return obj;
            }
            catch (Exception e)
            {
                //TODO: Add logging of exceptions
                return null;
            }
        }
    }
}
