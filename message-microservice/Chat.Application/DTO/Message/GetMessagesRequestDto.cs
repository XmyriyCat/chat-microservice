namespace Chat.Application.DTO.Message
{
    public class GetMessagesRequestDto
    {
        public int SenderUserId { get; set; }

        public int ReceiverUserId { get; set; }
    }
}
