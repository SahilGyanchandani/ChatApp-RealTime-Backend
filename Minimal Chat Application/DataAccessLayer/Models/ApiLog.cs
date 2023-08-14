using Humanizer;
using Microsoft.VisualBasic;
using System;

namespace Minimal_Chat_Application.DataAccessLayer.Models
{
    public class ApiLog
    {
        public int Id { get; set; }
        public string IpAddress { get; set; }
        public string RequestBody { get; set; }
        public DateTime TimeStamp { get; set; }
        public string? Email { get; set; }
    }

}
