using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thesis.Inventory.MobileApp.Model
{
    public class ChatModel
    {
        public string User { get; set; }
        public string Message { get; set; }
        public bool isYou { get; set; }
    }
}
