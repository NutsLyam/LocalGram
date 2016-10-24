using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LocalGram.Model;

namespace LocalGram.Tests
{
    [TestClass]
    public class DataLayerTests
    {
        private const string ConnectionString = "Data Source=NUTS;Initial Catalog=LocalGram;Integrated Security=True";

        [TestMethod]
        public void ShouldAddUser()
        {
            //arrange
            var user = new Users
            {
                Name = Guid.NewGuid().ToString().Substring(10)
            };
            var dataLayer = new DataLayer.SQL.DataLayer(ConnectionString);
            //act
            user = dataLayer.AddUser(user);
            //asserts
            var resultUser = dataLayer.GetUser(user.Id);
            Assert.AreEqual(user.Name, resultUser.Name);
        }




        [TestMethod]
        public void ShouldAddPost()
        {
            //arrange
            var post = new Posts
            {

               // UserId = Guid.NewGuid(),
                UserId = Guid.Parse( "4448B3E0-21E0-4E48-90D4-B494226949FA" ),
            };

            //act
            var dataLayer = new DataLayer.SQL.DataLayer(ConnectionString);
            post = dataLayer.AddPost(post);

            //asserts
            var resultPost = dataLayer.GetPost(post.Id);
            Assert.AreEqual(post.UserId, resultPost.UserId);
        }
        [TestMethod]
        public void ShouldAddHashtag()
        {
            //arrange
            var hashtag = new Hashtags
            {
                Name = Guid.NewGuid().ToString().Substring(10)
            };
            var dataLayer = new DataLayer.SQL.DataLayer(ConnectionString);
            //act
            hashtag = dataLayer.AddHashtag(hashtag);
            //asserts
            var resultHashtag = dataLayer.GetHashtag(hashtag.Id);
            Assert.AreEqual(hashtag.Name, resultHashtag.Name);
        }

        [TestMethod]
        public void ShouldAddComment()
        {
            //arrange
            var comment = new Comments
            {
                // PostId = Guid.NewGuid(),
                PostId = Guid.Parse("4668F815-3D93-4D09-B6EB-1EA46F294F6F"),
                Text = Guid.NewGuid().ToString().Substring(10)

            };
            //act
            var dataLayer = new DataLayer.SQL.DataLayer(ConnectionString);
            comment = dataLayer.AddComment(comment);
            //asserts
            var resultComment = dataLayer.GetComment(comment.Id);
           Assert.AreEqual(comment.Text, resultComment.Text);
        }
    }
}


