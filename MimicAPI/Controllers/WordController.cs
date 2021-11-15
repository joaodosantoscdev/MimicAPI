using Microsoft.AspNetCore.Mvc;
using MimicAPI.Database;
using MimicAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MimicAPI.Controllers
{
    public class PalavrasController : ControllerBase
    {
    
        private readonly MimicContext _context;
        public PalavrasController(MimicContext context)
        {
            _context = context;
        }

        //APP
        public ActionResult GetAll()
        {
            return Ok(_context.Words);
        }


        //WEB
        public ActionResult GetById(int id)
        {
            return Ok(_context.Words.Find(id));
        }

        public ActionResult Add(Word word)
        {
            _context.Words.Add(word);

            return Ok();
        }

        public ActionResult Update(int id, Word word)
        {
            _context.Words.Update(word);

            return Ok();
        }

        public ActionResult Delete(int id)
        {
            _context.Words.Remove(_context.Words.Find(id));

            return Ok();
        }
    }
}


