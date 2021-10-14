using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatStorage_PoC.Models
{
    public class Chat
    {
        public Guid Id { get; set; }
        public string ChatName { get; set; }
        public List<Message> Messages { get; set; }
    }
}
