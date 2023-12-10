using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thesis.Inventory.Domain.Entities
{
    public class ChatRoomEntity : BaseEntity
    {
        public string Name { get; set; }
        public virtual IEnumerable<ChatMessageEntity> Messages { get; set; }
    }
}
