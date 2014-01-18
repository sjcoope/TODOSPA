using Breeze.WebApi.EF;
using SJCNet.Todo.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SJCNet.Todo.Web.Api
{
    public class DefaultController : ApiController
    {
        [HttpGet]
        public string Metadata()
        {
            return new EFContextProvider<TodoDbContext>().Metadata();
        }
    }
}