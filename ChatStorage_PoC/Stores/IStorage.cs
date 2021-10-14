using ChatStorage_PoC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatStorage_PoC.Stores
{
    public interface IStorage : IDisposable
    {
        public Task AddChat(Chat chat);
        public Task AddChats(IEnumerable<Chat> chats);
        public Task<List<Chat>> GetItems();
        public Task<Chat> GetChat(Guid id);
        public Task<Message> GetMessage(Guid id);
    }
}
