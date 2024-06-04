using AutoMapper;
using ISTA.SecureApp.Models;
using ISTA.SecureApp.Models.DTO;

namespace ISTA.SecureApp.Api
{
    public class MappingProfile: Profile
    {

        public MappingProfile() 
        { 
            CreateMap<User, UserDTO>();
            CreateMap<UserDTO, User>();
        }
    }
}
