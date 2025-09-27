using AutoMapper;
using Domain.Models.Messaging;

namespace Infrastructure.MappingProfiles
{
    public class MessageProfile: Profile
    {
        public MessageProfile()
        {
            CreateMap<CreateMessageDto, Message>();
            CreateMap<Message, MessageDto>();
        }
    }
}
