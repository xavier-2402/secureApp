using AutoMapper;
using ISTA.SecureApp.Models;
using ISTA.SecureApp.Models.DTO;
using ISTA.SecureApp.Repositories;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Text;

namespace ISTA.SecureApp.Services
{
    public class UserService
    {
        private readonly UserRepository _repository;
        private readonly IMapper _mapper;

        public UserService(UserRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public User Add(User user)
        {
            user.Password = Encript.EncriptPassword(user.Password);
            return _repository.Add(user);
        }

        public List<UserDTO> GetAll()
        {
            return _mapper.Map<List<UserDTO>>(_repository.GetAll());
        }

    }
}
