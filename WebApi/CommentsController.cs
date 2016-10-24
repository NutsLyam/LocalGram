using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LocalGram.Model;

namespace LocalGram.WebApi.Controllers
{
    public class CommentsController : ApiController
    {
        private const string ConnectionString = "Data Source=NUTS;Initial Catalog=LocalGram;Integrated Security=True";
        private readonly IDataLayer _dataLayer;

        public CommentsController()
        {
            _dataLayer = new DataLayer.SQL.DataLayer(ConnectionString);
        }

        [HttpPost]
        public Comments CreateComment(Comments comment)
        {
            return _dataLayer.AddComment(comment);
        }

        [HttpGet]
        [Route("api/comments/{id}")]
        public Comments GetComment(Guid id)
        {
            return _dataLayer.GetComment(id);
        }
    }
}
