using ISTA.SecureApp.Models;
using MongoDB.Driver;

namespace ISTA.SecureApp.Repositories.DbContext.Mongo
{
    public class MongoContext
    {
        public readonly IMongoCollection<User> users;

        public MongoContext(IMongoSetting setting)
        {
            var client = new MongoClient(setting.MongoDbConnection);
            var database = client.GetDatabase(setting.Database);

            this.users = database.GetCollection<User>("users");
        }
    }
}
