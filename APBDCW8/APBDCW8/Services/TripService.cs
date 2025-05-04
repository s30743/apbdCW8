using APBDCW8.Models;
using Microsoft.Data.SqlClient;

namespace APBDCW8.Services;

public class TripService : ITripService
{
    private readonly string _connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;";
    

    public async Task<List<Trip>> GetTrips()
    {
        var trips = new List<Trip>();
        await using var con = new SqlConnection(_connectionString);
        await using var com = new SqlCommand();
        
        com.Connection = con;
        com.CommandText = @"SELECT t.IdTrip, t.Name AS TripName, t.Description, t.DateFrom, t.DateTo, t.MaxPeople, c.Name
            From Trip t Join Country_Trip CT on t.IdTrip = CT.IdTrip Join Country C on CT.IdCountry = C.IdCountry";
        await con.OpenAsync();
        
        SqlDataReader reader = await com.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                int idTrip = (int)reader["idTrip"];
                string Name = (string)reader["TripName"];
                string Description = (string)reader["Description"];
                DateTime DateFrom = (DateTime)reader["DateFrom"];
                DateTime DateTo = (DateTime)reader["DateTo"];
                int maxPeople = (int)reader["MaxPeople"];
                List<string> country = (List<string>)reader["Name"];
                
                
                
                trips.Add(new Trip()
                {
                    IdTrip = idTrip,
                    Name = Name,
                    Description = Description,
                    DateFrom = DateFrom,
                    DateTo = DateTo,
                    MaxPeople = maxPeople,
                    Countries = reader["Countries"] != DBNull.Value ? reader["Countries"].ToString().Split(',').ToList() : new List<string>()
                    
                });
            }
        
        
        return trips;
    }
    

    
}