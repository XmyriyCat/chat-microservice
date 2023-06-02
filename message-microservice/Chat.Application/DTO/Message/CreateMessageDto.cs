namespace Chat.Application.DTO.Message
{
    public class CreateMessageDto
    {
        public string Value { get; set; }

        public int SenderUserId { get; set; }

        public int ReceiverUserId { get; set; }
    }
}
