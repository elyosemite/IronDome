namespace Identity.Infrastructure;

public class UserDataModel(string name)
{
    public Guid Id { get; set; }
    public string Name { get; set; } = name;
}