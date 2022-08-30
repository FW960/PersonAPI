using EmployeesAPI.Entities;
using EmployeesAPI.Repositories.Interfaces;
using MySqlConnector;

namespace EmployeesAPI.Repositories;

public class CompanyRepository : ICompanyRepository
{
    private readonly MySqlConnection _connection;

    public CompanyRepository(MySqlConnection connection)
    {
        _connection = connection;
    }

    public bool Add(Company company)
    {
        try
        {
            _connection.Open();

            var cmd = _connection.CreateCommand();

            cmd.CommandText = "SELECT * FROM customers WHERE company_inn = @inn AND POST = 'CEO'";

            cmd.Parameters.AddWithValue("@inn", company.Inn);

            var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                int ceoId = reader.GetInt32(0);
                
                reader.Close();
                
                cmd.CommandText = "INSERT INTO companies (name, inn, ceo_id) VALUES (@name, @inn, @ceo_id)";

                cmd.Parameters.AddWithValue("ceo_id", ceoId);
            }
            else
            {
                reader.Close();
                
                cmd.CommandText = "INSERT INTO companies (name, inn) VALUES (@name, @inn)";    
            }

            cmd.Parameters.AddWithValue("@name", company.Name);

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

    public bool Delete(string inn)
    {
        try
        {
            _connection.Open();

            var cmd = _connection.CreateCommand();

            cmd.CommandText = "DELETE FROM companies WHERE inn = @inn";

            cmd.Parameters.AddWithValue("@inn", inn);

            if (cmd.ExecuteNonQuery() == 0)
                return false;

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

    public bool TryFind(string inn, out Company company)
    {
        try
        {
            _connection.Open();

            var cmd = _connection.CreateCommand();

            cmd.CommandText = @"SELECT *
            FROM companies
            LEFT JOIN customers c on companies.inn = c.company_inn AND c.post = 'CEO'
            WHERE inn = @inn";

            cmd.Parameters.AddWithValue("@inn", inn);

            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                company = new Company
                {
                    Inn = inn,
                    Name = reader.GetString(0),
                };

                try
                {
                    company.Ceo = new Ceo
                    {
                        Email = reader.GetString(8),
                        FirstName = reader.GetString(4),
                        LastName = reader.GetString(5)
                    };
                }
                catch
                {
                    return true;
                }

                return true;
            }

            company = new Company();

            return false;
        }
        catch (Exception e)
        {
            company = new Company();

            throw new Exception(e.ToString());
        }
        finally
        {
            _connection.Close();
        }
    }

    public bool Update(Company company)
    {
        try
        {
            _connection.Open();

            var cmd = _connection.CreateCommand();

            cmd.CommandText = "UPDATE companies SET name = @name, ceo_id = @ceo_id, inn = @inn WHERE inn = @inn";

            cmd.Parameters.AddWithValue("@name", company.Name);

            cmd.Parameters.AddWithValue("@ceo_id", company.Ceo.Id);

            cmd.Parameters.AddWithValue("@inn", company.Inn);

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