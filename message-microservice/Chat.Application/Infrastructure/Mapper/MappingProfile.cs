using AutoMapper;
using Chat.Application.DTO.Message;
using Chat.Application.DTO.User;
using Chat.Data.Models;

namespace Chat.Application.Infrastructure.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserOutputDto>();

            CreateMap<CreateUserDto, User>();

            CreateMap<CreateMessageDto, Message>();

            CreateMap<Message, MessageOutputDto>();
        }
    }
}
