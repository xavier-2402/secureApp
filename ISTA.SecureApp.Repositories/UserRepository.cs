using ISTA.SecureApp.Models;
using ISTA.SecureApp.Repositories.DbContext.Mongo;
using MongoDB.Driver;

namespace ISTA.SecureApp.Repositories
{
    public class UserRepository
    {
        private readonly MongoContext _context; 

        public UserRepository(MongoContext context)
        {
            _context = context;
        }

        public User Add(User user) 
        {
            _context.users.InsertOne(user);
            return user;
        }

        public List<User> GetAll()
        {
            return _context.users
                .Find(Builders<User>.Filter.Empty)
                .ToList();
        }
    }
}
