using MySqlConnector;
using PersonsAPI.Entities;

namespace PersonsAPI.Repositories.Persons;

public class PersonsRepository : IPersonsRepository
{
    private readonly MySqlConnection _connection;

    public PersonsRepository(MySqlConnection connection)
    {
        _connection = connection;
    }

    public bool Add(Person person)
    {
        try
        {
            _connection.Open();

            var cmd = _connection.CreateCommand();

            cmd.CommandText =
                "INSERT INTO persons (FirstName, LastName, Email, Company, Age) values(@fName, @lName, @email, @company, @age)";

            cmd.Parameters.AddWithValue("@fName", person.FirstName);

            cmd.Parameters.AddWithValue("@lName", person.LastName);

            cmd.Parameters.AddWithValue("@email", person.Email);

            cmd.Parameters.AddWithValue("@company", person.Company);

            cmd.Parameters.AddWithValue("@age", person.Age);

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

    public bool Update(int id, Person person)
    {
        try
        {
            _connection.Open();

            var cmd = _connection.CreateCommand();

            cmd.CommandText =
                "UPDATE persons SET FirstName = @fName, LastName = @lName, Email = @email, Company = @company, Age = @age WHERE Id = @id";

            cmd.Parameters.AddWithValue("@id", id);

            cmd.Parameters.AddWithValue("@fName", person.FirstName);

            cmd.Parameters.AddWithValue("@lName", person.LastName);

            cmd.Parameters.AddWithValue("@email", person.Email);

            cmd.Parameters.AddWithValue("@company", person.Company);

            cmd.Parameters.AddWithValue("@age", person.Age);

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

    public bool TryFind(int id, out Person person)
    {
        try
        {
            _connection.Open();

            var cmd = _connection.CreateCommand();

            cmd.CommandText = "SELECT * FROM persons WHERE Id = @id";

            cmd.Parameters.AddWithValue("@id", id);

            cmd.Prepare();

            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                person = new Person
                {
                    Id = reader.GetInt32(0),
                    FirstName = reader.GetString(1),
                    LastName = reader.GetString(2),
                    Email = reader.GetString(3),
                    Company = reader.GetString(4),
                    Age = reader.GetInt32(5)
                };
                return true;
            }

            person = new Person();

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

    public bool TryFind(string lastName, out Person person)
    {
        try
        {
            _connection.Open();

            var cmd = _connection.CreateCommand();

            cmd.CommandText = "SELECT * FROM persons WHERE LastName = @lName";

            cmd.Parameters.AddWithValue("@lName", lastName);

            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                person = new Person
                {
                    Id = reader.GetInt32(0),
                    FirstName = reader.GetString(1),
                    LastName = reader.GetString(2),
                    Email = reader.GetString(3),
                    Company = reader.GetString(4),
                    Age = reader.GetInt32(5)
                };
                return true;
            }

            person = new Person();

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

            cmd.CommandText = "DELETE FROM persons WHERE Id = @id";

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

    public bool FindRange(int startIndex, int endIndex, out IReadOnlyCollection<Person> persons)
    {
        try
        {
            _connection.Open();

            var cmd = _connection.CreateCommand();

            cmd.CommandText = "SELECT * FROM persons WHERE Id >= @startIndex AND Id <= @endIndex";

            cmd.Parameters.AddWithValue("@startIndex", startIndex);

            cmd.Parameters.AddWithValue("@endIndex", endIndex);

            var reader = cmd.ExecuteReader();

            List<Person> list = new List<Person>(); 

            while (reader.Read())
            {
                list.Add(new Person
                {
                    Id = reader.GetInt32(0),
                    FirstName = reader.GetString(1),
                    LastName = reader.GetString(2),
                    Email = reader.GetString(3),
                    Company = reader.GetString(4),
                    Age = reader.GetInt32(5)
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