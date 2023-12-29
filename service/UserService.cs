using MongoDB.Driver;

public class UserService
{
    private  IMongoCollection<User> users;

    public UserService()
    {
        var client = new MongoClient("mongodb://localhost:27017");
        var database = client.GetDatabase("testdb");

        users = database.GetCollection<User>("users");
    }

    public List<User> Get()
    {
        return users.Find(user => true).ToList();
    }

   public User Get(string id)
{
    var USERS = users.AsQueryable();
    foreach (var user in USERS)
    {
        if (user.Id.ToString() == id)
        {
            return user;
        }
    }

    return null;
}

    public User Create(User user)
    {
        users.InsertOne(user);
        return user;
    }

    public void Update(string id, User userIn)
    {
        users.ReplaceOne(user => user.Id.ToString() == id, userIn);
    }

    public void Remove(User userIn)
    {
        users.DeleteOne(user => user.Id == userIn.Id);
    }

    public void Remove(string id)
    {
        users.DeleteOne(user => user.Id.ToString() == id);
    }
}
