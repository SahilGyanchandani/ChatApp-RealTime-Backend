namespace Minimal_Chat_Application.Response_Models
{
    public class MessageResponse
    {
        public int id { get; set; }
        public string content { get; set; }
        public string userId { get; set; }
        public string receiverID { get; set; }
        public DateTime timestamp { get; set; }
    }
}
