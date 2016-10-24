using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LocalGram.Model;

namespace LocalGram.WebApi.Controllers
{
    public class UsersController : ApiController
    {
        private const string ConnectionString = "Data Source=NUTS;Initial Catalog=LocalGram;Integrated Security=True";
        private readonly IDataLayer _dataLayer;

        public UsersController()
        {
            _dataLayer = new DataLayer.SQL.DataLayer(ConnectionString);
        }

        [HttpPost]
        public Users CreateUser(Users user)
        {
            return _dataLayer.AddUser(user);
        }

        [HttpGet]
        [Route("api/users/{id}")]
        public Users GetUser(Guid id)
        {
            return _dataLayer.GetUser(id);
        }
    }
}
