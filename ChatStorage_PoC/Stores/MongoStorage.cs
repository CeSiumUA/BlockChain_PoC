using ChatStorage_PoC.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatStorage_PoC.Stores
{
    public class MongoStorage : IStorage
    {
        private readonly MongoClient mongoClient;
        private readonly IMongoDatabase mongoDatabase;
        private readonly IMongoCollection<Chat> mongoCollection;

        public MongoStorage(string connectionString)
        {
            this.mongoClient = new MongoClient(connectionString);
            this.mongoDatabase = this.mongoClient.GetDatabase("local");
            this.mongoCollection = this.mongoDatabase.GetCollection<Chat>("test");
        }
        public async Task AddChat(Chat chat)
        {
            throw new NotImplementedException();
        }

        public async Task AddChats(IEnumerable<Chat> chats)
        {
            await this.mongoCollection.InsertManyAsync(chats);
        }

        public void Dispose()
        {

        }

        public async Task<Chat> GetChat(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Chat>> GetItems()
        {
            throw new NotImplementedException();
        }

        public async Task<Message> GetMessage(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
