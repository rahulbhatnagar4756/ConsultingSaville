using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TestConnectDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DemoController : ControllerBase
    {

        //get the string value "hello world" from the api
        [HttpGet("HelloWorld")]
        public string Get()
        {
            return "Hello World";
        }
    }
}
