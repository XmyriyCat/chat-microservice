namespace Chat.Data.Models
{
    public class Message
    {
        public int Id { get; set; }

        public string Value { get; set; }

        public User Sender { get; set; }

        public User Receiver { get; set; }
    }
}
