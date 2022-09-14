using ContractsAPI.Dto;
using MySqlConnector;

namespace ContractsAPI.Repositories;

public class ContractRepository : IContractsRepository
{
    private readonly MySqlConnection _connection;

    public ContractRepository(MySqlConnection connection)
    {
        _connection = connection;
    }

    public bool Add(ContractDto contract, out int id)
    {
        try
        {
            _connection.Open();

            var cmd = _connection.CreateCommand();

            cmd.CommandText =
                @"INSERT INTO contracts (company_inn, creation_date, is_done, employees_group, price) 
                VALUES (@inn, @creat_date, @isDone, @group, @price)";

            cmd.Parameters.AddWithValue("@inn", contract.CompanyInn);

            cmd.Parameters.AddWithValue("@creat_date", contract.CreationDate);

            cmd.Parameters.AddWithValue("@isDone", false);

            cmd.Parameters.AddWithValue("@group", contract.EmployeesGroup);

            cmd.Parameters.AddWithValue("@price", contract.Price);

            if (cmd.ExecuteNonQuery() == 1)
            {
                cmd.Parameters.Clear();

                cmd.CommandText = "SELECT id FROM contracts WHERE company_inn = @inn ORDER BY id DESC LIMIT 1";

                cmd.Parameters.AddWithValue("@inn", contract.CompanyInn);

                var reader = cmd.ExecuteReader();

                reader.Read();

                id = reader.GetInt32(0);

                reader.Close();

                return true;
            }

            id = -1;
            return false;
        }
        catch (Exception e)
        {
            throw new Exception(e.ToString());
        }
        finally
        {
            _connection.Close();
        }
    }

    public bool Update(ContractDto contract)
    {
        try
        {
            _connection.Open();

            var cmd = _connection.CreateCommand();

            cmd.CommandText =
                @"UPDATE contracts SET is_done = @isDone, employees_group = @group, company_inn = @inn, creation_date = @creat WHERE id = @id";

            cmd.Parameters.AddWithValue("@isDone", contract.IsDone);

            cmd.Parameters.AddWithValue("@group", contract.EmployeesGroup);

            cmd.Parameters.AddWithValue("@inn", contract.CompanyInn);

            cmd.Parameters.AddWithValue("@creat", contract.CreationDate);

            cmd.Parameters.AddWithValue("@id", contract.Id);

            cmd.ExecuteNonQuery();

            return true;
        }
        catch (Exception e)
        {
            throw new Exception(e.ToString());
        }
        finally
        {
            _connection.Close();
        }
    }

    public bool Get(int companyInn, int id, out ContractDto contract)
    {
        try
        {
            _connection.Open();

            var cmd = _connection.CreateCommand();

            cmd.CommandText =
                "SELECT * FROM contracts company_inn WHERE company_inn = @inn AND id = @id";

            cmd.Parameters.AddWithValue("@inn", companyInn);

            cmd.Parameters.AddWithValue("@id", id);

            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                contract = new ContractDto
                {
                    Id = reader.GetInt32(0),
                    CompanyInn = reader.GetInt32(1),
                    CreationDate = reader.GetDateTime(2),
                    IsDone = reader.GetBoolean(3),
                    EmployeesGroup = reader.GetInt32(4)
                };

                return true;
            }

            contract = new ContractDto();

            return false;
        }
        catch (Exception e)
        {
            throw new Exception(e.ToString());
        }
        finally
        {
            _connection.Close();
        }
    }

    public bool Get(string email, out ContractDto contract)
    {
        try
        {
            _connection.Open();

            var cmd = _connection.CreateCommand();

            cmd.CommandText = @"SELECT contracts.id, company_inn, creation_date, is_done, price
                            FROM contracts
                            LEFT JOIN employees c on c.group = employees_group
                            WHERE c.Email = @email";

            cmd.Parameters.AddWithValue("@email", email);

            var reader = cmd.ExecuteReader();

            reader.Read();

            contract = new ContractDto
            {
                Id = reader.GetInt32(0),
                CompanyInn = reader.GetInt32(1),
                CreationDate = reader.GetDateTime(2),
                IsDone = reader.GetBoolean(3),
                Price = reader.GetDecimal(4)
            };

            return true;
        }
        catch
        {
            contract = new ContractDto();
            
            return false;
        }
        finally
        {
            _connection.Close();
        }
    }

    public bool Delete(DateTime creationDate, int companyInn, int id)
    {
        try
        {
            _connection.Open();

            var cmd = _connection.CreateCommand();

            cmd.CommandText = "DELETE FROM contracts WHERE company_inn = @inn AND creation_date = @creat AND id = @id";

            cmd.Parameters.AddWithValue("@inn", companyInn);

            cmd.Parameters.AddWithValue("@creat", creationDate);

            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();

            return true;
        }
        catch (Exception e)
        {
            throw new Exception(e.ToString());
        }
        finally
        {
            _connection.Close();
        }
    }
}