namespace ContractsAPI.Dtos;

public abstract class ContractDto
{
    public string CompanyInn { get; set; }
    
    public int Id { get; set; }
    
    public DateTime CreationDate { get; set; }
    
    public int EmployeesGroup { get; set; }
    
    public bool isDone { get; set; }
}