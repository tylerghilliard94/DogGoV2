using DogGo.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace DogGo.Repositories
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly IConfiguration _config;

        // The constructor accepts an IConfiguration object as a parameter. This class comes from the ASP.NET framework and is useful for retrieving things out of the appsettings.json file like connection strings.
        public OwnerRepository(IConfiguration config)
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

        public List<Owner> GetAllOwners()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT Id, [Email], Name, Address, Phone, NeighborhoodId
                        FROM Owner
                    ";

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Owner> owners = new List<Owner>();
                    while (reader.Read())
                    {
                        Owner owner = new Owner()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            Address = reader.GetString(reader.GetOrdinal("Address")),
                            Phone = reader.GetString(reader.GetOrdinal("Phone")),
                            NeighborhoodId = reader.GetInt32(reader.GetOrdinal("NeighborhoodId"))
                        };

                        owners.Add(owner);
                    }

                    reader.Close();

                    return owners;
                }
            }
        }

        public Owner GetOwnerById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT o.Id, o.Email, o.Name, o.Address, o.Phone, o.NeighborhoodId
                        FROM Owner o
                       
                        WHERE o.Id = @id
                    ";

                    cmd.Parameters.AddWithValue("@id", id);

                    SqlDataReader reader = cmd.ExecuteReader();
                    Owner owner = new Owner()
                    {
                        Dogs = new List<Dog>()
                    };
                  

                   
                    if (reader.Read())
                    {
                       
                        owner.Id = id;
                        owner.Name = reader.GetString(reader.GetOrdinal("Name"));
                        owner.Email = reader.GetString(reader.GetOrdinal("Email"));
                        owner.Address = reader.GetString(reader.GetOrdinal("Address"));
                        owner.Phone = reader.GetString(reader.GetOrdinal("Phone"));
                        owner.NeighborhoodId = reader.GetInt32(reader.GetOrdinal("NeighborhoodId"));

                        reader.Close();
                       /* cmd.CommandText = @"
                        SELECT Id, Name, OwnerId, Breed, Notes, ImageUrl
                        FROM Dog
                       
                        WHERE OwnerId = @id
                    ";
                        

                        SqlDataReader reader2 = cmd.ExecuteReader();
                        while (reader2.Read())
                        {
                            Dog dog = new Dog
                            {
                                Id = reader2.GetInt32(reader2.GetOrdinal("Id")),
                                Name = reader2.GetString(reader2.GetOrdinal("Name")),
                                OwnerId = reader2.GetInt32(reader2.GetOrdinal("OwnerId")),
                                Breed = reader2.GetString(reader2.GetOrdinal("Breed")),
                                Notes = reader2.GetString(reader2.GetOrdinal("Notes")),
                                ImageUrl = reader2.GetString(reader2.GetOrdinal("ImageUrl"))
                            };

                            owner.Dogs.Add(dog);
                        }


                        
                        reader2.Close();*/
                        return owner;
                    }
                    else
                    {
                        reader.Close();
                        return null;
                    }
                    
                  
                }
            }
        }
        public Owner GetOwnerByEmail(string email)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT o.Id, o.Email, o.Name, o.Address, o.Phone, o.NeighborhoodId
                        FROM Owner o
                       
                        WHERE o.Email = @email
                    ";

                    cmd.Parameters.AddWithValue("@email", email);

                    SqlDataReader reader = cmd.ExecuteReader();
                    Owner owner = new Owner()
                    {
                        Dogs = new List<Dog>()
                    };



                    if (reader.Read())
                    {

                        owner.Id = reader.GetInt32(reader.GetOrdinal("Id"));
                        owner.Name = reader.GetString(reader.GetOrdinal("Name"));
                        owner.Email = reader.GetString(reader.GetOrdinal("Email"));
                        owner.Address = reader.GetString(reader.GetOrdinal("Address"));
                        owner.Phone = reader.GetString(reader.GetOrdinal("Phone"));
                        owner.NeighborhoodId = reader.GetInt32(reader.GetOrdinal("NeighborhoodId"));

                        reader.Close();
                        /* cmd.CommandText = @"
                         SELECT Id, Name, OwnerId, Breed, Notes, ImageUrl
                         FROM Dog

                         WHERE OwnerId = @id
                     ";


                         SqlDataReader reader2 = cmd.ExecuteReader();
                         while (reader2.Read())
                         {
                             Dog dog = new Dog
                             {
                                 Id = reader2.GetInt32(reader2.GetOrdinal("Id")),
                                 Name = reader2.GetString(reader2.GetOrdinal("Name")),
                                 OwnerId = reader2.GetInt32(reader2.GetOrdinal("OwnerId")),
                                 Breed = reader2.GetString(reader2.GetOrdinal("Breed")),
                                 Notes = reader2.GetString(reader2.GetOrdinal("Notes")),
                                 ImageUrl = reader2.GetString(reader2.GetOrdinal("ImageUrl"))
                             };

                             owner.Dogs.Add(dog);
                         }



                         reader2.Close();*/
                        return owner;
                    }
                    else
                    {
                        reader.Close();
                        return null;
                    }


                }
            }
        }
        public void AddOwner(Owner owner)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                    INSERT INTO Owner ([Name], Email, Phone, Address, NeighborhoodId)
                    OUTPUT INSERTED.ID
                    VALUES (@name, @email, @phoneNumber, @address, @neighborhoodId);
                ";

                    cmd.Parameters.AddWithValue("@name", owner.Name);
                    cmd.Parameters.AddWithValue("@email", owner.Email);
                    cmd.Parameters.AddWithValue("@phoneNumber", owner.Phone);
                    cmd.Parameters.AddWithValue("@address", owner.Address);
                    cmd.Parameters.AddWithValue("@neighborhoodId", owner.NeighborhoodId);

                    int id = (int)cmd.ExecuteScalar();

                    owner.Id = id;
                }
            }
        }
        public void UpdateOwner(Owner owner)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            UPDATE Owner
                            SET 
                                [Name] = @name, 
                                Email = @email, 
                                Address = @address, 
                                Phone = @phone, 
                                NeighborhoodId = @neighborhoodId
                            WHERE Id = @id";

                    cmd.Parameters.AddWithValue("@name", owner.Name);
                    cmd.Parameters.AddWithValue("@email", owner.Email);
                    cmd.Parameters.AddWithValue("@address", owner.Address);
                    cmd.Parameters.AddWithValue("@phone", owner.Phone);
                    cmd.Parameters.AddWithValue("@neighborhoodId", owner.NeighborhoodId);
                    cmd.Parameters.AddWithValue("@id", owner.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteOwner(int ownerId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            DELETE FROM Owner
                            WHERE Id = @id
                        ";

                    cmd.Parameters.AddWithValue("@id", ownerId);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}