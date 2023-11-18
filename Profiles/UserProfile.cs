using AutoMapper;
using TaskLogix.Dtos;
using TaskLogix.Models;

namespace TaskLogix.Profiles
{
    public class UserProfile:Profile
    {
        public UserProfile()
        {
            CreateMap<UserCreateDto, User>();
            CreateMap<User, UserReadDto>();
        }
    }
}