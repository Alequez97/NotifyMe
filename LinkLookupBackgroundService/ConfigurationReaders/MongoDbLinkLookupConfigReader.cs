using DomainEntites;
using LinkLookupBackgroundService.Interfaces;
using MessageSender.Models;
using MongoDB.Driver;
using System;

namespace LinkLookupBackgroundService.ConfigurationReaders
{
    public class MongoDbLinkLookupConfigReader : ILinkLookupConfigReader
    {
        private readonly IMongoCollection<Group> _collection;
        private readonly string _groupName;

        /// <summary>
        /// Constructor that connects to mongoDb by connection string
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="databaseName"></param>
        /// <param name="tableName"></param>
        public MongoDbLinkLookupConfigReader(string groupName, string connectionString, string databaseName = "NotifyMe", string tableName = "Groups")
        {
            var client = new MongoClient(connectionString);
            var mongoDb = client.GetDatabase(databaseName);
            _groupName = groupName;
            _collection = mongoDb.GetCollection<Group>(tableName);
        }

        /// <summary>
        /// Constructor that connects to localhost mongoDb
        /// </summary>
        /// <param name="databaseName"></param>
        /// <param name="tableName"></param>
        public MongoDbLinkLookupConfigReader(string groupName, string databaseName = "NotifyMe", string tableName = "Groups")
        {
            var client = new MongoClient();
            var mongoDb = client.GetDatabase(databaseName);
            _groupName = groupName;
            _collection = mongoDb.GetCollection<Group>(tableName);
        }

        public NotifyConfig GetGroupsConfiguration()
        {
            var group = _collection.Find(g => g.Name == _groupName).FirstOrDefault();
            return group?.NotifyConfig ?? throw new InvalidOperationException($"Group with name {_groupName} not found");
        }
    }
}
