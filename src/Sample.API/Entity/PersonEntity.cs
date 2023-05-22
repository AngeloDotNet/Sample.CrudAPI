namespace Sample.API.Entity;

public class PersonEntity : IEntity<Guid>
{
    public Guid Id { get; set; }
    public string Cognome { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
}