namespace ContractsAPI.Entities;

public class Contract
{
    public string CompanyInn { get; set; }
    public int Id { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime LastUpdateDate { get; set; }
    public int EmployeesGroup { get; set; }
    public bool isDone { get; set; }
}