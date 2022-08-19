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

            cmd.CommandText = "SELECT id FROM companies WHERE inn = @inn";

            cmd.Parameters.AddWithValue("@inn", customer.Company.Inn);
            
            var reader = cmd.ExecuteReader();

            int comp_id = -1;

            while (reader.Read())
                comp_id = reader.GetInt32(0);
            
            reader.Close();

            if (comp_id == -1)
            {
                _connection.Close();

                throw new Exception($"Haven't found company with {customer.Company.Inn}");
            }
                

            cmd.CommandText =
                "INSERT INTO customers (firstname, lastname, email, post, company_id) values(@fName, @lName, @email, @post, @comp_id)";

            cmd.Parameters.AddWithValue("@fName", customer.FirstName);

            cmd.Parameters.AddWithValue("@lName", customer.LastName);

            cmd.Parameters.AddWithValue("@email", customer.Email);

            cmd.Parameters.AddWithValue("@post", customer.Post);

            cmd.Parameters.AddWithValue("@comp_id", comp_id);

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

    public bool Update(int id, Customer personEntity)
    {
        try
        {
            _connection.Open();

            var cmd = _connection.CreateCommand();

            cmd.CommandText =
                "UPDATE customers SET FirstName = @fName, LastName = @lName, Email = @email, company_id = @com_id, post = @post WHERE Id = @id";

            cmd.Parameters.AddWithValue("@id", id);

            cmd.Parameters.AddWithValue("@fName", personEntity.FirstName);

            cmd.Parameters.AddWithValue("@lName", personEntity.LastName);

            cmd.Parameters.AddWithValue("@email", personEntity.Email);

            cmd.Parameters.AddWithValue("@com_id", personEntity.Company.Id);

            cmd.Parameters.AddWithValue("@post", personEntity.Post);

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

    public bool TryFind(int id, out Customer personEntity)
    {
        try
        {
            _connection.Open();

            var cmd = _connection.CreateCommand();

            cmd.CommandText = @"SELECT *
            FROM customers
            LEFT JOIN companies comp on customers.company_id = comp.id
            LEFT JOIN customers c on comp.id = c.id WHERE customers.id = @id";

            cmd.Parameters.AddWithValue("@id", id);

            cmd.Prepare();

            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                personEntity = new Customer
                {
                    Id = reader.GetInt32(0),
                    FirstName = reader.GetString(1),
                    LastName = reader.GetString(2),
                    Post = reader.GetString(3),
                    Email = reader.GetString(5),
                    Company = new Company
                    {
                        Id = reader.GetInt32(6),
                        Name = reader.GetString(7),
                        Inn = reader.GetInt32(9),
                        Ceo = new Ceo
                        {
                            Id = reader.GetInt32(8),
                            FirstName = reader.GetString(11),
                            LastName = reader.GetString(12),
                            Email = reader.GetString(15)
                        }
                    },
                };
                return true;
            }

            personEntity = new Customer();

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

    public bool TryFind(string firstName, string lastName, out Customer personEntity)
    {
        try
        {
            _connection.Open();

            var cmd = _connection.CreateCommand();

            cmd.CommandText = @"SELECT *
            FROM customers
            LEFT JOIN companies comp on customers.company_id = comp.id
            LEFT JOIN customers c on comp.id = c.id WHERE c.lastname = @lName AND c.firstname = @fName";

            cmd.CommandText = "SELECT * FROM employees ";

            cmd.Parameters.AddWithValue("@lName", lastName);

            cmd.Parameters.AddWithValue("@fName", firstName);

            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                personEntity = new Customer
                {
                    Id = reader.GetInt32(0),
                    FirstName = reader.GetString(1),
                    LastName = reader.GetString(2),
                    Post = reader.GetString(3),
                    Email = reader.GetString(5),
                    Company = new Company
                    {
                        Id = reader.GetInt32(6),
                        Name = reader.GetString(7),
                        Inn = reader.GetInt32(9),
                        Ceo = new Ceo
                        {
                            Id = reader.GetInt32(8),
                            FirstName = reader.GetString(11),
                            LastName = reader.GetString(12),
                            Email = reader.GetString(15)
                        }
                    },
                };
                return true;
            }

            personEntity = new Customer();

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

    public bool FindRange(int startIndex, int endIndex, out IReadOnlyCollection<Customer> persons)
    {
        try
        {
            _connection.Open();

            var cmd = _connection.CreateCommand();

            cmd.CommandText = @"SELECT *
            FROM customers
            LEFT JOIN companies comp on customers.company_id = comp.id
            LEFT JOIN customers c on comp.id = c.id WHERE customers.id >= @startIndex AND customers.Id <= @endIndex";

            cmd.Parameters.AddWithValue("@startIndex", startIndex);

            cmd.Parameters.AddWithValue("@endIndex", endIndex);

            var reader = cmd.ExecuteReader();

            List<Customer> list = new List<Customer>();

            while (reader.Read())
            {
                list.Add(new Customer
                {
                    Id = reader.GetInt32(0),
                    FirstName = reader.GetString(1),
                    LastName = reader.GetString(2),
                    Post = reader.GetString(3),
                    Email = reader.GetString(5),
                    Company = new Company
                    {
                        Id = reader.GetInt32(6),
                        Name = reader.GetString(7),
                        Inn = reader.GetInt32(9),
                        Ceo = new Ceo
                        {
                            Id = reader.GetInt32(8),
                            FirstName = reader.GetString(11),
                            LastName = reader.GetString(12),
                            Email = reader.GetString(15)
                        }
                    },
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