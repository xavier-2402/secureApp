
namespace ISTA.SecureApp.Repositories.DbContext.Mongo
{
    public interface IMongoSetting
    {
        string MongoDbConnection { get; set; }
        string Database {  get; set; }
        string User {  get; set; }
        string Password { get; set; }
    }
}
