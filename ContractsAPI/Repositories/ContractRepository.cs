using ContractsAPI.Entities;
using MySqlConnector;

namespace ContractsAPI.Repositories;

public class ContractRepository : IContractsRepository
{
    private readonly MySqlConnection _connection;

    public ContractRepository(MySqlConnection connection)
    {
        _connection = connection;
    }

    public bool Add(Contract contract, out int id)
    {
        try
        {
            _connection.Open();

            var cmd = _connection.CreateCommand();

            cmd.CommandText =
                @"INSERT INTO contracts (company_inn, creation_date, last_update_date ,is_done, employees_group) 
                VALUES (@inn, @creat_date, @update_date, @isDone, @group)";

            cmd.Parameters.AddWithValue("@inn", contract.CompanyInn);

            cmd.Parameters.AddWithValue("@creat_date", contract.CreationDate);

            cmd.Parameters.AddWithValue("@update_date", contract.CreationDate);

            cmd.Parameters.AddWithValue("@isDone", contract.isDone);

            cmd.Parameters.AddWithValue("@group", contract.EmployeesGroup);

            if (cmd.ExecuteNonQuery() == 1)
            {
                cmd.CommandText = "SELECT Id FROM contracts WHERE company_inn = @creat AND company_inn = @inn";

                var reader = cmd.ExecuteReader();

                id = reader.GetInt32(0);

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

    public bool Update(Contract contract)
    {
        try
        {
            _connection.Open();

            var cmd = _connection.CreateCommand();

            cmd.CommandText =
                @"UPDATE contracts SET is_done = @isDone, employees_group = @group
                WHERE company_inn = @inn AND creation_date = @creat";

            cmd.Parameters.AddWithValue("@isDone", contract.isDone);

            cmd.Parameters.AddWithValue("@group", contract.EmployeesGroup);

            cmd.Parameters.AddWithValue("@inn", contract.CompanyInn);

            cmd.Parameters.AddWithValue("@creat", contract.CreationDate);

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

    public bool Get(DateTime creationDate, int companyInn, out Contract contract)
    {
        try
        {
            _connection.Open();

            var cmd = _connection.CreateCommand();

            cmd.CommandText =
                "SELECT * FROM contracts company_inn WHERE company_inn = @inn AND creation_date = @creat";

            cmd.Parameters.AddWithValue("@inn", companyInn);

            cmd.Parameters.AddWithValue("@creat", creationDate);

            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                contract = new Contract
                {
                    Id = reader.GetInt32(0),
                    CompanyInn = reader.GetString(1),
                    CreationDate = reader.GetDateTime(2),
                    LastUpdateDate = reader.GetDateTime(3),
                    isDone = reader.GetBoolean(4),
                    EmployeesGroup = reader.GetInt32(5)
                };

                return true;
            }

            contract = new Contract();
            
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

    public bool Delete(DateTime creationDate, int companyInn)
    {
        try
        {
            _connection.Open();

            var cmd = _connection.CreateCommand();

            cmd.CommandText = "DELETE FROM contracts WHERE company_inn = @inn AND creation_date = @creat";

            cmd.Parameters.AddWithValue("@inn", companyInn);

            cmd.Parameters.AddWithValue("@creat", creationDate);

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