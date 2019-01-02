using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using  AspNetCoreVueJs.Web.Data;
using Microsoft.AspNetCore.Authorization;

namespace  AspNetCoreVueJs.Web.Features.Users
{
    [Authorize(Policy = "RequireAdministratorRole")]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly EcommerceContext _db;

        public UsersController(EcommerceContext db)
        {
            _db = db;
        }
        // GET: api/<controller>
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            return Ok(await _db.Users.ToListAsync());
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
