using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LocalGram.Model;

namespace LocalGram.DataLayer.SQL
{
    public class DataLayer : IDataLayer
    {
        private readonly string _connectionString;

        public DataLayer(string connectionString)
        {
            if (connectionString == null)
                throw new ArgumentNullException(nameof(connectionString));

            _connectionString = connectionString;
        }

        public Users AddUser(Users user)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    user.Id = Guid.NewGuid();
                    command.CommandText = "insert into Users (ID, Name) values (@ID, @Name)";
                    command.Parameters.AddWithValue("@ID", user.Id);
                    command.Parameters.AddWithValue("@Name", user.Name);
                    command.ExecuteNonQuery();
                    return user;
                }
                
            }

        }


        public Posts AddPost(Posts post)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    post.Id = Guid.NewGuid();
                    command.CommandText = "insert into Posts (ID, UserID) values (@ID, @UserID)";
                    command.Parameters.AddWithValue("@ID", post.Id);
                    command.Parameters.AddWithValue("@UserID", post.UserId);
                   // DateTime s = DateTime.Now;
                   // command.Parameters.AddWithValue("@DatePublication", s);
                    command.ExecuteNonQuery();

                }
                return post;

            }
       
        }

        public Users GetUser(Guid id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "select id, Name from Users where id = @id";
                    command.Parameters.AddWithValue("@id", id);
                    using (var reader = command.ExecuteReader())
                    {
                        reader.Read();
                        return new Users
                        {
                            Id = reader.GetGuid(0),
                            Name = reader.GetString(1)
                        };
                    }
                }
            }
        }

        public Posts GetPost(Guid id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "select id, UserID from Posts where id = @id";
                    command.Parameters.AddWithValue("@id", id);
                    using (var reader = command.ExecuteReader())
                    {
                        reader.Read();
                        return new Posts

                        {
                            Id = reader.GetGuid(0),
                            UserId = reader.GetGuid(1)
                        };
                    }
                }
            }
        }



        public Comments AddComment(Comments comment)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    comment.Id = Guid.NewGuid();
                    command.CommandText = "insert into comments (ID, Text,PostID) values (@ID, @Text, @PostID)";
                    command.Parameters.AddWithValue("@ID", comment.Id);
                    command.Parameters.AddWithValue("@Text", comment.Text);
                    command.Parameters.AddWithValue("@PostID", comment.PostId);
                    command.ExecuteNonQuery();
                    return comment;
                }

            }
        }


       public Comments GetComment(Guid id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "select id, Text, PostID from Comments where id = @id";
                    command.Parameters.AddWithValue("@id", id);
                    using (var reader = command.ExecuteReader())
                    {
                        reader.Read();
                        return new Comments
                    
                        {
                            Id = reader.GetGuid(0),
                            Text = reader.GetString(1),
                            PostId = reader.GetGuid(2)
                        };
                    }
                }
            }
        }
            

        public Hashtags AddHashtag(Hashtags hashtag)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    hashtag.Id = Guid.NewGuid();
                    command.CommandText = "insert into Hashtags (ID, Name) values (@ID, @Name)";
                    command.Parameters.AddWithValue("@ID", hashtag.Id);
                    command.Parameters.AddWithValue("@Name", hashtag.Name);
                    command.ExecuteNonQuery();
                    return hashtag;
                }
            }
        }

        public Hashtags GetHashtag(Guid id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "select id, Name from Hashtags where id = @id";
                    command.Parameters.AddWithValue("@id", id);
                    using (var reader = command.ExecuteReader())
                    {
                        reader.Read();
                        return new Hashtags
                        {
                            Id = reader.GetGuid(0),
                            Name = reader.GetString(1)
                        };
                    }
                }
            }
        }
    }
}

    

    


