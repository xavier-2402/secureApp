namespace ISTA.SecureApp.Repositories.DbContext.Mongo
{
    public class MongoSetting : IMongoSetting
    {
        public string MongoDbConnection { get; set; }
        public string Database { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
    }
}
