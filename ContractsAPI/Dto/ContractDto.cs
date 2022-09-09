namespace ContractsAPI.Dto;

public class ContractDto
{
    public int CompanyInn { get; set; }
    public int Id { get; set; }
    public DateTime CreationDate { get; set; }
    public int EmployeesGroup { get; set; }
    public bool IsDone { get; set; }
    public decimal Price { get; set; }
}