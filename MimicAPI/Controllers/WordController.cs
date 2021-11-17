using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimicAPI.Database;
using MimicAPI.Helpers;
using MimicAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MimicAPI.Controllers
{
    [Route("api/words")]
    public class PalavrasController : ControllerBase
    {

        private readonly MimicContext _context;
        public PalavrasController(MimicContext context)
        {
            _context = context;
        }

        //APP -- api/words?date=yyyy-MM-dd
        [Route("")]
        [HttpGet]
        public ActionResult GetAll([FromQuery]WordUrlQuery query)
        {
            var item = _context.Words.AsQueryable();
            if (query.Date.HasValue)
            {
                item = item.Where(i => i.Created > query.Date.Value);
            }

            if (query.PgNumber.HasValue)
            {
                var qntTotalRegis = item.Count(); 
                item = item.Skip((query.PgNumber.Value - 1) * query.PgRegister.Value).Take(query.PgRegister.Value);

                var pagination = new Pagination();
                pagination.NumPage = query.PgNumber.Value;
                pagination.RegisPage = query.PgRegister.Value;
                pagination.TotalRegis = qntTotalRegis;
                pagination.TotalPages = (int)Math.Ceiling( (double)qntTotalRegis / query.PgRegister.Value );

                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(pagination));

                if (query.PgNumber > pagination.TotalPages)
                {
                    return NotFound();
                }
            }
            return Ok(item);
        }


        //WEB -- api/words/{id}
        [Route("{id}")]
        [HttpGet]
        public ActionResult GetById(int id)
        {
            var obj = _context.Words.Find(id);

            if (obj == null)
            {
                return NotFound();
            }
            return Ok();
        }

        // -- /api/words (POST: id, name, active, score, date)
        [Route("")]
        [HttpPost]
        public ActionResult Add([FromBody]Word word)
        {
            _context.Words.Add(word);
            _context.SaveChanges();

            return Created($"/api/words/{word.Id}", word);
        }

        // -- /api/words/{id} (PUT: id, name, active, score, date)
        [Route("{id}")]
        [HttpPut]
        public ActionResult Update(int id, [FromBody]Word word)
        {
            var obj = _context.Words.AsNoTracking().FirstOrDefault(a => a.Id == id);

            if (obj == null)
            {
                return NotFound();
            }

            word.Id = id;
            _context.Words.Update(word);
            _context.SaveChanges();

            return Ok();
        }

        // -- api/words/{id} (DELETE)
        [Route("{id}")]
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            var obj = _context.Words.Find(id);

            if (obj == null)
            {
                return NotFound();
            }

            obj.Active = false;
            _context.Words.Update(obj);
            _context.SaveChanges();

            return NoContent();
        }
    }
}


