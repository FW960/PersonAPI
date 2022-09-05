using EmployeesAPI.Entities;
using EmployeesAPI.Repositories.Persons.Interfaces;
using MySqlConnector;

namespace EmployeesAPI.Repositories.Persons;

public class CustomerRepository : IPersonsRepository<Customer>
{
    private readonly MySqlConnection _connection;

    public CustomerRepository(MySqlConnection connection)
    {
        _connection = connection;
    }

    public bool Add(Customer customer)
    {
        try
        {
            _connection.Open();

            var cmd = _connection.CreateCommand();

            cmd.CommandText =
                "INSERT INTO customers (firstname, lastname, email, post, company_inn) values(@fName, @lName, @email, @post, @comp_inn)";

            cmd.Parameters.AddWithValue("@fName", customer.FirstName);

            cmd.Parameters.AddWithValue("@lName", customer.LastName);

            cmd.Parameters.AddWithValue("@email", customer.Email);

            cmd.Parameters.AddWithValue("@post", customer.Post);

            cmd.Parameters.AddWithValue("@comp_inn", customer.Company.Inn);

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

    public bool Update(Customer employee)
    {
        try
        {
            _connection.Open();

            var cmd = _connection.CreateCommand();

            cmd.CommandText =
                "UPDATE customers SET FirstName = @fName, LastName = @lName, Email = @email, company_inn = @com_inn, post = @post WHERE Id = @id";

            cmd.Parameters.AddWithValue("@id", employee.Id);

            cmd.Parameters.AddWithValue("@fName", employee.FirstName);

            cmd.Parameters.AddWithValue("@lName", employee.LastName);

            cmd.Parameters.AddWithValue("@email", employee.Email);

            cmd.Parameters.AddWithValue("@com_inn", employee.Company.Inn);

            cmd.Parameters.AddWithValue("@post", employee.Post);

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

    public bool TryFind(int id, out Customer employee)
    {
        try
        {
            _connection.Open();

            var cmd = _connection.CreateCommand();

            cmd.CommandText = @"SELECT *
         FROM customers
         LEFT JOIN companies comp on customers.company_inn = comp.inn
         LEFT JOIN customers cust on cust.id = comp.ceo_id WHERE customers.id = @id";

            cmd.Parameters.AddWithValue("@id", id);

            cmd.Prepare();

            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                employee = new Customer
                {
                    Id = reader.GetInt32(0),
                    FirstName = reader.GetString(1),
                    LastName = reader.GetString(2),
                    Post = reader.GetString(3),
                    Email = reader.GetString(5),
                };
                try
                {
                    employee.Company = new Company
                    {
                        Name = reader.GetString(6),
                        Inn = reader.GetString(8),
                        Ceo = new Ceo
                        {
                            Id = reader.GetInt32(7),
                            FirstName = reader.GetString(10),
                            LastName = reader.GetString(11),
                            Email = reader.GetString(14)
                        }
                    };
                }
                catch
                {
                    return true;
                }

                return true;
            }

            employee = new Customer();

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

    public bool TryFind(string firstName, string lastName, out Customer employee)
    {
        try
        {
            _connection.Open();

            var cmd = _connection.CreateCommand();

            cmd.CommandText = @"SELECT *
         FROM customers
         LEFT JOIN companies comp on customers.company_inn = comp.inn
         LEFT JOIN customers cust on cust.id = comp.ceo_id WHERE customers.lastname = @lName AND customers.firstname = @fName";

            cmd.Parameters.AddWithValue("@lName", lastName);

            cmd.Parameters.AddWithValue("@fName", firstName);

            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                employee = new Customer
                {
                    Id = reader.GetInt32(0),
                    FirstName = reader.GetString(1),
                    LastName = reader.GetString(2),
                    Post = reader.GetString(3),
                    Email = reader.GetString(5),
                };

                try
                {
                    employee.Company = new Company
                    {
                        Name = reader.GetString(6),
                        Inn = reader.GetString(8),
                        Ceo = new Ceo
                        {
                            Id = reader.GetInt32(7),
                            FirstName = reader.GetString(10),
                            LastName = reader.GetString(11),
                            Email = reader.GetString(14)
                        }
                    };
                }
                catch
                {
                    return false;
                }

                return true;
            }

            employee = new Customer();

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

            cmd.CommandText = "DELETE FROM customers WHERE Id = @id";

            cmd.Parameters.AddWithValue("@id", id);

            if (cmd.ExecuteNonQuery() == 0)
                return false;

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

    public bool FindRange(int startIndex, int endIndex, out IReadOnlyCollection<Customer> employee)
    {
        try
        {
            _connection.Open();

            var cmd = _connection.CreateCommand();

            cmd.CommandText = @"SELECT *
         FROM customers
         LEFT JOIN companies comp on customers.company_inn = comp.inn
         LEFT JOIN customers cust on cust.id = comp.ceo_id WHERE customers.id >= @startIndex AND customers.Id <= @endIndex";

            cmd.Parameters.AddWithValue("@startIndex", startIndex);

            cmd.Parameters.AddWithValue("@endIndex", endIndex);
            
            var reader = cmd.ExecuteReader();
            
            List<Customer> list = new List<Customer>();

            while (reader.Read())
            {
                var personEntity = new Customer
                {
                    Id = reader.GetInt32(0),
                    FirstName = reader.GetString(1),
                    LastName = reader.GetString(2),
                    Post = reader.GetString(3),
                    Email = reader.GetString(5),
                };

                try
                {
                    personEntity.Company = new Company
                    {
                        Name = reader.GetString(6),
                        Inn = reader.GetString(8),
                        Ceo = new Ceo
                        {
                            Id = reader.GetInt32(7),
                            FirstName = reader.GetString(10),
                            LastName = reader.GetString(11),
                            Email = reader.GetString(14)
                        }
                    };
                }
                catch
                {
                }

                list.Add(personEntity);
            }

            employee = list;

            if (employee.Count == 0)
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