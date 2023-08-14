using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Minimal_Chat_Application.ParameterModels
{
    public class ReceiveMessage
    { 
        public string ReceiverID { get; set; }
        public string Content { get; set; }

        // Navigation property
        //public ICollection<Message> Messages { get; set; }
    }
}
