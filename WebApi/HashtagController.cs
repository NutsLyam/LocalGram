using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LocalGram.Model;

namespace LocalGram.WebApi.Controllers
{
    public class HashtagController : ApiController
    {
        private const string ConnectionString = "Data Source=NUTS;Initial Catalog=LocalGram;Integrated Security=True";
        private readonly IDataLayer _dataLayer;

        public HashtagController()
        {
            _dataLayer = new DataLayer.SQL.DataLayer(ConnectionString);
        }

        [HttpPost]
        [Route("api/hashtag/{name}")]
        public void  CreateHashtag(string name)
        {
           // string name = "snow";
            _dataLayer.AddHashtagToPost(Guid.Parse("54C40E6E-B749-4769-88F4-43C3F472C3F7"), _dataLayer.GetHashtagId(name)); //(Posts, hashtag)
             
        }
        [HttpGet]
        [Route("api/hashtag/{id}")]
        public Hashtags GetHashtag(Guid id)
        {
            return _dataLayer.GetHashtag(id);
        }
    }
}
