using DataLayer.Access.Services.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApp.Shop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IDefaultCreatorRepository _defaultCreatorRepository;
        public ProductController(IDefaultCreatorRepository defaultCreatorRepository)
        {
            _defaultCreatorRepository = defaultCreatorRepository;
        }
        // GET: api/<ProductController>
        [HttpGet]
        [Authorize]
        public IEnumerable<string> Get()
        {
            //await _defaultCreatorRepository.FastCreate();
            return new string[] { "value1", "value2" };
        }

        // GET api/<ProductController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ProductController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ProductController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ProductController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
