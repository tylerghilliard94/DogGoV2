using DogGo.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
namespace DogGo.Repositories
{
    public class WalkRepository : IWalkRepository
    {
        private readonly IConfiguration _config;

        // The constructor accepts an IConfiguration object as a parameter. This class comes from the ASP.NET framework and is useful for retrieving things out of the appsettings.json file like connection strings.
        public WalkRepository(IConfiguration config)
        {
            _config = config;
        }

        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }

        public List<Walks> GetAllWalks()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT w.Id, w.[Date], w.Duration, w.WalkerId, w.DogId, d.Name as DogName, d.OwnerId, o.Name as OwnerName, wa.Name as WalkerName
                        FROM Walks w
                        JOIN Dog d ON w.DogId = d.Id
                        JOIN Owner o On o.Id = d.OwnerId
                        Join Walker wa ON w.WalkerId = wa.Id
                
                        Order By WalkerName
                        
                    ";

             

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Walks> walks = new List<Walks>();
                    while (reader.Read())
                    {
                        Walks walk = new Walks
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                            Duration = reader.GetInt32(reader.GetOrdinal("Duration")),
                            WalkerId = reader.GetInt32(reader.GetOrdinal("WalkerId")),
                            DogId = reader.GetInt32(reader.GetOrdinal("DogId")),
                            Walker = new Walker
                            {
                                Name = reader.GetString(reader.GetOrdinal("WalkerName")),
                            },
                            Dog = new Dog
                            {
                                Name = reader.GetString(reader.GetOrdinal("DogName")),
                            },
                            Owner = new Owner
                            {
                                Name = reader.GetString(reader.GetOrdinal("OwnerName")),
                            }
                        };

                        walks.Add(walk);
                    }

                    reader.Close();

                    return walks;
                }
            }
        }
        public List<Walks> GetAllWalksById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT w.Id, w.[Date], w.Duration, w.WalkerId, w.DogId, d.Name as DogName, d.OwnerId, o.Name as OwnerName
                        FROM Walks w
                        JOIN Dog d ON w.DogId = d.Id
                        JOIN Owner o On o.Id = d.OwnerId
                        
                        WHERE WalkerId = @id
                        Order By OwnerName
                        
                    ";

                    cmd.Parameters.AddWithValue("@id", id);

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Walks> walks = new List<Walks>();
                    while (reader.Read())
                    {
                        Walks walk = new Walks
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                            Duration = reader.GetInt32(reader.GetOrdinal("Duration")),
                            WalkerId = reader.GetInt32(reader.GetOrdinal("WalkerId")),
                            DogId = reader.GetInt32(reader.GetOrdinal("DogId")),
                            Dog = new Dog
                            {
                                Name = reader.GetString(reader.GetOrdinal("DogName")),
                            },
                            Owner = new Owner
                            {
                                Name = reader.GetString(reader.GetOrdinal("OwnerName")),
                            }
                        };

                        walks.Add(walk);
                    }

                    reader.Close();

                    return walks;
                }
            }
        }
        public void AddWalks(Walks walk, int counter)
        {
           
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"
                    INSERT INTO Walks (Date, Duration, WalkerId, DogId)
                    OUTPUT INSERTED.ID
                    VALUES (@date, @duration, @walkerId, @dogId);
                ";

                        cmd.Parameters.AddWithValue("@date", walk.Date);
                        cmd.Parameters.AddWithValue("@duration", walk.Duration);
                        cmd.Parameters.AddWithValue("@walkerId", walk.WalkerId);
                        cmd.Parameters.AddWithValue("@dogId", walk.Dogs[counter]);


                        int id = (int)cmd.ExecuteScalar();

                        walk.Id = id;
                    }

                }
            }
        

    }
}
