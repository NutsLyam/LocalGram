using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LocalGram.Model;

namespace LocalGram.WebApi.Controllers
{
    public class PostsController : ApiController
    {
        private const string ConnectionString = "Data Source=NUTS;Initial Catalog=LocalGram;Integrated Security=True";
        private readonly IDataLayer _dataLayer;

        public PostsController()
        {
            _dataLayer = new DataLayer.SQL.DataLayer(ConnectionString);
        }

        [HttpPost]
        public Posts CreatePost(Posts post)
        {
            return _dataLayer.AddPost(post);
        }

        [HttpGet]
        [Route("api/posts/{id}")]
        public Posts GetPosts(Guid id)
        {
            return _dataLayer.GetPost(id);
        }

        [HttpDelete]
        [Route("api/posts/{id}")]
        public void DeletePost(Guid id)
        {
        }
    }
}
