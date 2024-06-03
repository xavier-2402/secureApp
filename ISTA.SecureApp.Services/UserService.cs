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
            user.Password = EncriptPassword(user.Password);
            return _repository.Add(user);
        }

        public List<User> GetAll()
        {
            return _repository.GetAll();
        }

        private string EncriptPassword(string input)
        {
            SHA256 md5Hash = SHA256.Create();
            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }
    }
}
