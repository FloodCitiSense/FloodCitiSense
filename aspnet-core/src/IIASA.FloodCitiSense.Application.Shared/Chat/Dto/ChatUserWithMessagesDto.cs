using System.Collections.Generic;

namespace IIASA.FloodCitiSense.Chat.Dto
{
    public class ChatUserWithMessagesDto : ChatUserDto
    {
        public List<ChatMessageDto> Messages { get; set; }
    }
}