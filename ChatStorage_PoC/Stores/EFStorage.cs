using ChatStorage_PoC.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatStorage_PoC.Stores
{
    public class EFStorage : DbContext, IStorage
    {
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Message> Messages { get; set; }

        public EFStorage() { }
        public EFStorage(DbContextOptions dbContextOptions) : base(dbContextOptions) 
        {
            this.Database.Migrate();
        }

        public async Task AddChat(Chat chat)
        {
            await this.Chats.AddAsync(chat);
            await this.SaveChangesAsync();
        }

        public async Task<List<Chat>> GetItems()
        {
            return await this.Chats.ToListAsync();
        }

        public async Task<Chat> GetChat(Guid id)
        {
            return await this.Chats.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Message> GetMessage(Guid id)
        {
            return await this.Messages.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task AddChats(IEnumerable<Chat> chats)
        {
            await this.Chats.AddRangeAsync(chats);
            await this.SaveChangesAsync();
        }
    }
}
