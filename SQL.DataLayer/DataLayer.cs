using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LocalGram.Model;
using NLog;
using System.Diagnostics;
using System.IO;
namespace LocalGram.DataLayer.SQL
{
    public class DataLayer : IDataLayer
    {
        private readonly string _connectionString;

        private static Logger logger = LogManager.GetCurrentClassLogger();


        public DataLayer(string connectionString)
        {
            if (connectionString == null)
                throw new ArgumentNullException(nameof(connectionString));

            _connectionString = connectionString;
        }

        public Users AddUser(Users user)
        {
            Stopwatch sWatch = new Stopwatch();
            sWatch.Start();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    user.Id = Guid.NewGuid();
                    command.CommandText = "insert into Users (ID, Name,Information) values (@ID, @Name, @Information)";
                    command.Parameters.AddWithValue("@ID", user.Id);
                    command.Parameters.AddWithValue("@Name", user.Name);
                    command.Parameters.AddWithValue("@Information", user.Information);
                    command.ExecuteNonQuery();
                }              
            }
            sWatch.Stop();
            logger.Info("Пользователь с именем {0} добавлен || {1} ms", user.Name, sWatch.ElapsedMilliseconds.ToString());
            return user;
        }
        // загужает фотографию из директории в пост
        public byte[] PutPhoto()
        {
            FileStream fStream = new FileStream("d:\\kotenok.jpg", FileMode.Open, FileAccess.Read);
            Byte[] imageBytes = new byte[fStream.Length];
            fStream.Read(imageBytes, 0, imageBytes.Length);

            return imageBytes;
        }


        public Posts AddPost(Posts post)
        {
            Stopwatch sWatch = new Stopwatch();
            sWatch.Start();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    post.Id = Guid.NewGuid();
                    post.Date = DateTime.Now;
                    command.CommandText = "insert into Posts (ID, UserID, DatePublication,Photo) values (@ID, @UserID, @DatePublication, @Photo)";
                    command.Parameters.AddWithValue("@ID", post.Id);
                    command.Parameters.AddWithValue("@UserID", post.UserId);
                    command.Parameters.AddWithValue("@DatePublication", post.Date);
                    command.Parameters.AddWithValue("@Photo", PutPhoto());
                    command.ExecuteNonQuery(); 
                }          
            }
            sWatch.Stop();
            logger.Info("Пост с  id {0}добавлен || { 1}  ms",post.Id, sWatch.ElapsedMilliseconds.ToString());
            return post;
        }

        public Users GetUser(Guid id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "select id, Name, Information from Users where id = @id";
                    command.Parameters.AddWithValue("@id", id);
                    using (var reader = command.ExecuteReader())
                    {
                        reader.Read();
                        return new Users
                        {
                            Id = reader.GetGuid(0),
                            Name = reader.GetString(1),
                            Information = reader.GetString(2)
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
                    command.CommandText = "select id, UserID, DatePublication from Posts where id = @id";
                    command.Parameters.AddWithValue("@id", id);
                    using (var reader = command.ExecuteReader())
                    {
                        reader.Read();
                        return new Posts

                        {
                            Id = reader.GetGuid(0),
                            UserId = reader.GetGuid(1),
                            Date = reader.GetDateTime(2)
                        };
                    }
                }
            }
        }

        public Comments AddComment(Comments comment)
        {
            Stopwatch sWatch = new Stopwatch();
            sWatch.Start();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    //comment.Id = Guid.NewGuid();
                    comment.Id = Guid.Parse("A4783CB4-B9F4-4D95-B6ED-D0857D5F8A86");
                    command.CommandText = "insert into comments (ID, Text,PostID, DatePublication) values (@ID, @Text, @PostID, @DatePublication)";
                    command.Parameters.AddWithValue("@ID", comment.Id);
                    command.Parameters.AddWithValue("@Text", comment.Text);
                    command.Parameters.AddWithValue("@PostID", comment.PostId);
                    command.Parameters.AddWithValue("@DatePublication", comment.Date);
                    command.ExecuteNonQuery();
                    logger.Info("Комментарий к посту {0} добавлен", comment.PostId);
                  
                }
            }
            sWatch.Stop();
            logger.Info("Комментарий  с  id {0}добавлен || { 1}  ms", comment.Id, sWatch.ElapsedMilliseconds.ToString());
            return comment;
        }

        public Comments GetComment(Guid id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "select id, Text, PostID ,DatePublication from Comments where id = @id";
                    command.Parameters.AddWithValue("@id", id);
                    using (var reader = command.ExecuteReader())
                    {
                        reader.Read();
                        return new Comments

                        {
                            Id = reader.GetGuid(0),
                            Text = reader.GetString(1),
                            PostId = reader.GetGuid(2),
                            Date = reader.GetDateTime(3)
                        };
                    }
                }
            }
        }

        //получение id хэштега
        public Guid GetHashtagId(string name)
        {
            int count;
            Guid id;
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "select count (id) from Hashtags where name = @name";
                    command.Parameters.AddWithValue("@name", name);
                    command.ExecuteNonQuery();

                    using (var reader = command.ExecuteReader())
                    {
                        reader.Read();
                        count = reader.GetInt32(0);
                    }
                }
                if (count == 0) //create new #
                {
                    Hashtags hashtag = new Hashtags
                    {
                        Id = Guid.NewGuid(),
                        Name = name
                    };
                    AddHashtag(hashtag);
                    id = hashtag.Id;
                }
                else
                {
                    using (var command2 = connection.CreateCommand())
                    {

                        command2.CommandText = "select id from Hashtags where name = @name";
                        command2.Parameters.AddWithValue("@name", name);
                        command2.ExecuteNonQuery();
                        using (var reader = command2.ExecuteReader())
                        {
                            reader.Read();
                            id = reader.GetGuid(0);
                        }

                    }
                }
               

            }
            return id;
        }
        

        //заполнение таблицы Hashtag
        public Hashtags AddHashtag(Hashtags hashtag)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    //hashtag.Id = Guid.NewGuid();
                    command.CommandText = "insert into Hashtags (ID, Name) values (@ID, @Name)";
                    command.Parameters.AddWithValue("@ID", hashtag.Id);
                    command.Parameters.AddWithValue("@Name", hashtag.Name);
                    command.ExecuteNonQuery();
                    logger.Info("Хештег {0} добавлен",hashtag.Name);

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

                        logger = LogManager.GetCurrentClassLogger();
                        logger.Info("hashtag добавлен");
                        reader.Read();
                        return new Hashtags
                        {
                            Id = reader.GetGuid(0),
                            Name = reader.GetString(1),

                        };
                    }
                }
            }
        }
        //заполнение таблицы Post-Hashtag
        public void AddHashtagToPost(Guid PostId, Guid HashtagId)
        {
            Stopwatch sWatch = new Stopwatch();
            sWatch.Start();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {

                    command.CommandText = "insert into HashtagToPost (PostID, HashtagID) values (@PostID, @HashtagID)";
                    command.Parameters.AddWithValue("@HashtagID", HashtagId);
                    command.Parameters.AddWithValue("@PostID", PostId);
                    command.ExecuteNonQuery();
                }
            }
        }
        public void SetLike()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "insert into Likes (PostID,UserID)  values (@PostID,@UserID)  ";
                    command.Parameters.AddWithValue("@UserID", Guid.Parse("A061C712-1D9F-4AF8-AE22-1BB78F67FE3E"));
                    command.Parameters.AddWithValue("@PostID", Guid.Parse("65F7964E-E8D7-40C2-AA64-DEE464D021C7"));
                    command.ExecuteNonQuery();
                }
            }
        }

        public void Mention()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "insert into UserToComment(UserID,CommentID)  values (@UserID,@CommentID)  ";
                    command.Parameters.AddWithValue("@CommentID", Guid.Parse("A4783CB4-B9F4-4D95-B6ED-D0857D5F8A86"));
                    command.Parameters.AddWithValue("@UserID", Guid.Parse("A061C712-1D9F-4AF8-AE22-1BB78F67FE3E"));
                    command.ExecuteNonQuery();
                }
            }

        }

        public int getLikes(Guid id) //post
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "select  count(*) from Likes where postid = @id"; ///кол-во лайков

                    command.Parameters.AddWithValue("@id", id);
                   Int32 count = (Int32)command.ExecuteScalar();
                    return count;
                }           
            }          
        }

        public void DeleteComments(Guid id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "delete from UserToComment where CommentID = @id";
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();

                }
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "delete from Comments where ID = @id ";
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                    logger.Info("Комментарий с поста {0} удален");
                }
            }
        }


        public void DeletePost(Guid id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "delete from Likes where PostID = @id";
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                }
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "select *from Comments where PostID = @id";
                    command.Parameters.AddWithValue("@id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        var list = new List<Guid>();
                        while (reader.Read())
                        {
                            var entity = new Guid();
                            entity = reader.GetGuid(0);
                            list.Add(entity);
                        }
                        list.ForEach(delegate (Guid CommentId)
                        {
                            DeleteComments(CommentId);

                        });
                    }
                }
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "delete from HashtagToPost where PostID = @id";
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                }
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "delete from Posts where ID = @id";
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                    logger.Info("Пост удален");
                }
            }
        }


        public void DeleteUser(Guid id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "delete from Likes where UserID = @id";
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                }
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "delete from UserToComment where UserID = @id";
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                }
                //delete comment???

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "select *from Posts where UserID = @id";
                    command.Parameters.AddWithValue("@id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        var list = new List<Guid>();
                        while (reader.Read())
                        {
                            var entity = new Guid();
                            entity = reader.GetGuid(0);
                            list.Add(entity);
                        }
                        list.ForEach(delegate (Guid PostId)
                        {
                            DeletePost(PostId);
                        });
                    }
                }
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "delete from Users where ID = @id";
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                    logger.Info("Пользователь удален");
                }
            }
        }
    }
}

    

    


