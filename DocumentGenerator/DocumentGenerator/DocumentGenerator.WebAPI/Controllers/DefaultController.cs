using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DocumentGenerator.WebAPI.Controllers
{
  public class DefaultController : ApiController
  {
    [HttpGet]
    [Route("")]
    public IEnumerable<string> Get()
    {
      return new string[] { "No methods have been implemented for this route." };
    }
  }
}