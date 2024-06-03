using ISTA.SecureApp.Models;
using ISTA.SecureApp.Repositories;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Text;

namespace ISTA.SecureApp.Services
{
    public class UserService
    {
        private readonly UserRepository _repository; 

        public UserService(UserRepository repository)
        {
            _repository = repository;
        }

        public User Add(User user)
        {
            user.Password = Encript.EncriptPassword(user.Password);
            return _repository.Add(user);
        }

        public List<User> GetAll()
        {
            return _repository.GetAll();
        }

    }
}
