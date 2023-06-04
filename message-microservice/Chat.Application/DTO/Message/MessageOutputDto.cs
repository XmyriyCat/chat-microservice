namespace Chat.Application.DTO.Message
{
    public class MessageOutputDto
    {
        public int Id { get; set; }

        public string SenderUsername { get; set; }

        public string Value { get; set; }

        public string ReceiverUsername { get; set; }
    }
}
