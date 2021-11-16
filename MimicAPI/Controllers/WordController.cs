using Microsoft.AspNetCore.Mvc;
using MimicAPI.Database;
using MimicAPI.Models;
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

        //APP -- api/words/
        [Route("")]
        [HttpGet]
        public ActionResult GetAll()
        {
            return Ok(_context.Words);
        }


        //WEB -- api/words/{id}
        [Route("{id}")]
        [HttpGet]
        public ActionResult GetById(int id)
        {
            return Ok(_context.Words.Find(id));
        }

        // -- /api/words (POST: id, name, active, score, date)
        [Route("")]
        [HttpPost]
        public ActionResult Add([FromBody]Word word)
        {
            _context.Words.Add(word);
            _context.SaveChanges();

            return Ok();
        }

        // -- /api/words/{id} (PUT: id, name, active, score, date)
        [Route("{id}")]
        [HttpPut]
        public ActionResult Update(int id, [FromBody]Word word)
        {
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
            _context.Words.Remove(_context.Words.Find(id));
            _context.SaveChanges();

            return Ok();
        }
    }
}


