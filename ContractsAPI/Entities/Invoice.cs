using ContractsAPI.Dto;
using MySqlConnector;

namespace ContractsAPI.Entities;

public class Invoice
{
    public decimal Price { get; set; }
    public DateTime ApproveDate { get; set; }

    public List<ContractDto> RelatedContracts = new List<ContractDto>();

    internal Invoice()
    {
    }
}

public static class InvoiceFactory
{
    private static MySqlConnection CreateDbConnection()
    {
        ConfigurationManager manager = new ConfigurationManager();

        manager.SetBasePath(@"C:\Users\windo\RiderProjects\TimeSheets\ContractsAPI\appsettings.json");

        return new MySqlConnection(manager.GetConnectionString("default"));
    }

    private static MySqlConnection Connection { get; }

    static InvoiceFactory()
    {
        Connection = CreateDbConnection();
    }

    public static bool Create(int inn, out Invoice invoice)
    {
        try
        {
            Connection.Open();

            var cmd = Connection.CreateCommand();

            cmd.CommandText = "SELECT * FROM contracts WHERE @inn = company_inn AND is_done = true";

            cmd.Parameters.AddWithValue("@inn", inn);

            invoice = new Invoice();

            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                invoice.RelatedContracts.Add(new ContractDto
                {
                    CompanyInn = inn,
                    IsDone = true,
                    CreationDate = reader.GetDateTime(2),
                    EmployeesGroup = reader.GetInt32(4),
                    Price = reader.GetDecimal(5)
                });
            }

            invoice.ApproveDate = DateTime.Now;

            invoice.Price = CalculateInvoicePrice(invoice.RelatedContracts);

            return true;
        }
        catch
        {
            //todo logger
            invoice = new Invoice();
            return true;
        }
        finally
        {
            try
            {
                Connection.Close();
            }
            catch
            {
                Connection.Close();
            }
        }
    }

    private static decimal CalculateInvoicePrice(List<ContractDto> contractDtos)
    {
        decimal totalPrice = 0;

        foreach (var contract in contractDtos)
            totalPrice += contract.Price;

        return totalPrice;
    }
}