using Castle.Components.DictionaryAdapter;
using IIASA.FloodCitiSense.Friendships.Dto;
using System;
using System.Collections.Generic;

namespace IIASA.FloodCitiSense.Chat.Dto
{
    public class GetUserChatFriendsWithSettingsOutput
    {
        public DateTime ServerTime { get; set; }

        public List<FriendDto> Friends { get; set; }

        public GetUserChatFriendsWithSettingsOutput()
        {
            Friends = new EditableList<FriendDto>();
        }
    }
}