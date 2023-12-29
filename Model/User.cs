
using MongoDB.Bson;

public class User
{
    public ObjectId Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public string Profession { get; set; }
}
