namespace HRM.Domain.Entities;

public class Employee
{
    public Guid Id { get; private set; }

    public string EmployeeNumber { get; private set; } = string.Empty;

    public string FirstName { get; private set; } = string.Empty;

    public string LastName { get; private set; } = string.Empty;

    public string Email { get; private set; } = string.Empty;

    public DateTime HireDate { get; private set; }

    public bool IsActive { get; private set; }

    public Employee(
        string employeeNumber,
        string firstName,
        string lastName,
        string email,
        DateTime hireDate)
    {
        Id = Guid.NewGuid();

        EmployeeNumber = employeeNumber;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        HireDate = hireDate;

        IsActive = true;
    }

    public void Update(
        string firstName,
        string lastName,
        string email)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
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