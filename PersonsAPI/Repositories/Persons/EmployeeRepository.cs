using MySqlConnector;
using EmployeesAPI.Entities;
using EmployeesAPI.Repositories.Persons.Interfaces;

namespace EmployeesAPI.Repositories.Persons;

public class EmployeeRepository : IPersonsRepository<Employee>
{
    private readonly MySqlConnection _connection;

    public EmployeeRepository(MySqlConnection connection)
    {
        _connection = connection;
    }

    public bool Add(Employee employee)
    {

        try
        {
            _connection.Open();

            var cmd = _connection.CreateCommand();

            cmd.CommandText =
                "INSERT INTO employees (FirstName, LastName, Email, Password, Age) values(@fName, @lName, @email, @password, @age)";

            cmd.Parameters.AddWithValue("@fName", employee.FirstName);

            cmd.Parameters.AddWithValue("@lName", employee.LastName);

            cmd.Parameters.AddWithValue("@email", employee.Email);

            cmd.Parameters.AddWithValue("@password", employee.Password);

            cmd.Parameters.AddWithValue("@age", employee.Age);

            cmd.Prepare();

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

    public bool Update(int id, Employee customer)
    {
        try
        {
            _connection.Open();

            var cmd = _connection.CreateCommand();

            cmd.CommandText =
                "UPDATE employees SET FirstName = @fName, LastName = @lName, Email = @email, Password = @password, Age = @age WHERE Id = @id";

            cmd.Parameters.AddWithValue("@id", id);

            cmd.Parameters.AddWithValue("@fName", customer.FirstName);

            cmd.Parameters.AddWithValue("@lName", customer.LastName);

            cmd.Parameters.AddWithValue("@email", customer.Email);

            cmd.Parameters.AddWithValue("@password", customer.Password);

            cmd.Parameters.AddWithValue("@age", customer.Age);

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

    public bool TryFind(int id, out Employee personEntity)
    {
        try
        {
            _connection.Open();

            var cmd = _connection.CreateCommand();

            cmd.CommandText = "SELECT * FROM employees WHERE Id = @id";

            cmd.Parameters.AddWithValue("@id", id);

            cmd.Prepare();

            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                personEntity = new Employee
                {
                    Id = reader.GetInt32(0),
                    FirstName = reader.GetString(1),
                    LastName = reader.GetString(2),
                    Email = reader.GetString(3),
                    Age = reader.GetInt32(4),
                    Password = reader.GetString(5)
                };
                return true;
            }

            personEntity = new Employee();

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

    public bool TryFind(string firstName, string lastName, out Employee personEntity)
    {
        try
        {
            _connection.Open();

            var cmd = _connection.CreateCommand();

            cmd.CommandText = "SELECT * FROM employees WHERE LastName = @lName AND FirstName = @fName";

            cmd.Parameters.AddWithValue("@lName", lastName);

            cmd.Parameters.AddWithValue("@fName", firstName);

            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                personEntity = new Employee
                {
                    Id = reader.GetInt32(0),
                    FirstName = reader.GetString(1),
                    LastName = reader.GetString(2),
                    Email = reader.GetString(3),
                    Age = reader.GetInt32(4),
                    Password = reader.GetString(5)
                };
                return true;
            }

            personEntity = new Employee();

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

    public bool Delete(int id)
    {
        try
        {
            _connection.Open();

            var cmd = _connection.CreateCommand();

            cmd.CommandText = "DELETE FROM employees WHERE Id = @id";

            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        finally
        {
            _connection.Close();
        }
    }

    public bool FindRange(int startIndex, int endIndex, out IReadOnlyCollection<Employee> persons)
    {
        try
        {
            _connection.Open();

            var cmd = _connection.CreateCommand();

            cmd.CommandText = "SELECT * FROM employees WHERE Id >= @startIndex AND Id <= @endIndex";

            cmd.Parameters.AddWithValue("@startIndex", startIndex);

            cmd.Parameters.AddWithValue("@endIndex", endIndex);

            var reader = cmd.ExecuteReader();

            List<Employee> list = new List<Employee>(); 

            while (reader.Read())
            {
                list.Add(new Employee
                {
                    Id = reader.GetInt32(0),
                    FirstName = reader.GetString(1),
                    LastName = reader.GetString(2),
                    Email = reader.GetString(3),
                    Age = reader.GetInt32(4),
                    Password = reader.GetString(5)
                });
            }

            persons = list;

            if (persons.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        finally
        {
            _connection.Close();
        }
    }
}