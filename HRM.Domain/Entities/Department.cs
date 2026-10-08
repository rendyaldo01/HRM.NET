namespace HRM.Domain.Entities;

public class Department
{
    public Guid Id { get; private set; }

    public string Code { get; private set; } = string.Empty;

    public string Name { get; private set; } = string.Empty;

    public bool IsActive { get; private set; }

    public Department(string code, string name)
    {
        Id = Guid.NewGuid();

        Code = code;
        Name = name;

        IsActive = true;
    }

    public void Update(string code, string name)
    {
        Code = code;
        Name = name;
    }

    public void Activate()
    {
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }
}