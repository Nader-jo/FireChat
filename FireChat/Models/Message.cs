using System;

namespace FireChat.Models
{
    public interface HasId
    {
        string Id { get; set; }
    }

    public class Message : HasId
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; }
        public DateTime SentDate { get; set; }
        public DateTime SeenDate { get; set; }
        public string Content { get; set; }
    }
}