using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimicAPI.Helpers;
using MimicAPI.Models;
using MimicAPI.Repositories.Interfaces;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace MimicAPI.Controllers
{
    [Route("api/words")]
    public class PalavrasController : ControllerBase
    {

        private readonly IWordRepository _repository;
        public PalavrasController(IWordRepository repository)
        {
            _repository = repository;
        }

        //APP -- api/words?date=yyyy-MM-dd
        [Route("")]
        [HttpGet]
        public ActionResult GetAll([FromQuery]WordUrlQuery query)
        {
            var item = _repository.GetAllWords(query);

            if (query.PgNumber > item.Pagination.TotalPages)
            {
                return NotFound();
            }

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(item.Pagination));

            return Ok(item.ToList());
        }


        //WEB -- api/words/{id}
        [Route("{id}")]
        [HttpGet]
        public ActionResult GetById(int id)
        {
            var obj = _repository.GetWord(id);

            if (obj == null)
            {
                return NotFound();
            }
            return Ok(obj);
        }

        // -- /api/words (POST: id, name, active, score, date)
        [Route("")]
        [HttpPost]
        public ActionResult Add([FromBody]Word word)
        {
            _repository.Add(word);

            return Created($"/api/words/{word.Id}", word);
        }

        // -- /api/words/{id} (PUT: id, name, active, score, date)
        [Route("{id}")]
        [HttpPut]
        public ActionResult Update(int id, [FromBody]Word word)
        {
            var obj = _repository.GetWord(id);

            if (obj == null)
            {
                return NotFound();
            }

            word.Id = id;
            _repository.Update(word);
            
            return Ok();
        }

        // -- api/words/{id} (DELETE)
        [Route("{id}")]
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            var obj = _repository.GetWord(id);

            if (obj == null)
            {
                return NotFound();
            }

            _repository.Delete(id);

            return NoContent();
        }
    }
}


