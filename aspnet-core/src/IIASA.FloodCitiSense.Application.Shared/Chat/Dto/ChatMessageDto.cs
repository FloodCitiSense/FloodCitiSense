using Abp.Application.Services.Dto;
using System;

namespace IIASA.FloodCitiSense.Chat.Dto
{
    public class ChatMessageDto : EntityDto
    {
        public long UserId { get; set; }

        public int? TenantId { get; set; }

        public long TargetUserId { get; set; }

        public int? TargetTenantId { get; set; }

        public ChatSide Side { get; set; }

        public ChatMessageReadState ReadState { get; set; }

        public ChatMessageReadState ReceiverReadState { get; set; }

        public string Message { get; set; }

        public DateTime CreationTime { get; set; }

        public string SharedMessageId { get; set; }
    }
}