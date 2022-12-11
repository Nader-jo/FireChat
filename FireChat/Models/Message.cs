using System;

namespace FireChat.Models
{
    public interface IHasId
    {
        string Id { get; set; }
    }

    public class Message : IHasId
    {
        public Message(string fromUser, string toUser, string content)
        {
            FromUserEmail = fromUser;
            ToUserEmail = toUser;
            SentDate = DateTime.Now;
            SeenDate = DateTime.MaxValue;
            Content = content;
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }
        public string FromUserEmail { get; set; }
        public string ToUserEmail { get; set; }
        public DateTime SentDate { get; set; }
        public DateTime SeenDate { get; set; }
        public string Content { get; set; }
    }
}