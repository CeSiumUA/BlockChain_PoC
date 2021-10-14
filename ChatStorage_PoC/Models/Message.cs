using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatStorage_PoC.Models
{
    public class Message
    {
        public Guid Id { get; set; }
        [BsonIgnore]
        public Chat Chat {  get; set; }
        public Guid ChatId { get; set; }
        public DateTime DateSent { get; set; }
        public string Text { get; set; }
        public string Sender { get; set; }
    }
}
