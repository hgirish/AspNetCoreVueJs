using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using  AspNetCoreVueJs.Web.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace  AspNetCoreVueJs.Web.Features.Filters
{
    [Route("api/[controller]")]
    public class FiltersController : Controller
    {
        private readonly EcommerceContext _db;

        public FiltersController(EcommerceContext db)
        {
            _db = db;
        }
        // GET: api/<controller>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var brands = await _db.Brands
                .Select(x => x.Name)
                .ToListAsync();
            var storage = await _db.Storage
                .Select(x => x.Capacity)
                .ToListAsync();

            var colours = await _db.Colours
              .Select(x => x.Name)
              .ToListAsync();

            var os = await _db.OS
              .Select(x => x.Name)
              .ToListAsync();

            var features = await _db.Features
              .Select(x => x.Name)
              .ToListAsync();

            return Ok(new FilterListViewModel
            {
                Brands = brands,
                Storage = storage,
                Colours = colours,
                OS = os,
                Features = features

            });
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
