using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimicAPI.Helpers;
using MimicAPI.Models;
using MimicAPI.Models.DTO;
using MimicAPI.Repositories.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MimicAPI.Controllers
{
    [Route("api/words")]
    public class PalavrasController : ControllerBase
    {

        private readonly IWordRepository _repository;
        private readonly IMapper _mapper;
        public PalavrasController(IWordRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        //APP -- api/words?date=yyyy-MM-dd
        [HttpGet("", Name = "GetAll")]
        public ActionResult GetAll([FromQuery]WordUrlQuery query)
        {
            var item = _repository.GetAllWords(query);

            if (item.Results.Count == 0)
            {
                return NotFound();
            }

            PaginationList<WordDTO> list = CreateLinksListWordDTO(query, item);

            return Ok(list);
        }

        //WEB -- api/words/{id}
        [HttpGet("{id}", Name = "GetWord")]
        public ActionResult GetById(int id)
        {
            var obj = _repository.GetWord(id);

            if (obj == null)
            {
                return NotFound();
            }

            WordDTO wordDTO = _mapper.Map<Word, WordDTO>(obj);
            wordDTO.Links.Add(
                new LinkDTO("self", Url.Link("GetWord", new { id = wordDTO.Id }), "GET")
                );
            wordDTO.Links.Add(
                new LinkDTO("update", Url.Link("UpdateWord", new { id = wordDTO.Id }), "PUT")
                );
            wordDTO.Links.Add(
                new LinkDTO("delete", Url.Link("DeleteWord", new { id = wordDTO.Id }), "DELETE")
                );

            return Ok(wordDTO);
        }

        // -- /api/words (POST: id, name, active, score, date)
        [Route("")]
        [HttpPost]
        public ActionResult Add([FromBody]Word word)
        {
            if (word == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

            word.Active = true;
            word.Created = DateTime.Now;
            _repository.Add(word);

            WordDTO wordDTO = _mapper.Map<Word, WordDTO>(word);
            wordDTO.Links.Add(
                new LinkDTO("self", Url.Link("GetWord", new { id = wordDTO.Id }), "GET")
                );


            return Created($"/api/words/{word.Id}", wordDTO);
        }

        // -- /api/words/{id} (PUT: id, name, active, score, date)
        [HttpPut("{id}", Name = "UpdateWord")]
        public ActionResult Update(int id, [FromBody]Word word)
        {
            var obj = _repository.GetWord(id);

            if (obj == null)
            {
                return NotFound();
            }

            word.Id = id;
            word.Active = obj.Active;
            word.Created = obj.Created;
            word.Att = DateTime.Now;
            _repository.Update(word);

            WordDTO wordDTO = _mapper.Map<Word, WordDTO>(word);
            wordDTO.Links.Add(
                new LinkDTO("self", Url.Link("GetWord", new { id = wordDTO.Id }), "GET")
                );

            return Ok(word);
        }

        // -- api/words/{id} (DELETE)
        [HttpDelete("{id}", Name = "DeleteWord")]
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

        // GETALL METHOD - PAGINATION NEXT AND PREV
        private PaginationList<WordDTO> CreateLinksListWordDTO(WordUrlQuery query, PaginationList<Word> item)
        {
            var list = _mapper.Map<PaginationList<Word>, PaginationList<WordDTO>>(item);

            foreach (var word in list.Results)
            {
                word.Links = new List<LinkDTO>();
                word.Links.Add(new LinkDTO("self", Url.Link("GetWord", new { id = word.Id }), "GET"));
            }

            list.Links.Add(new LinkDTO("self", Url.Link("GetAll", query), "GET"));

            if (item.Pagination != null)
            {
                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(item.Pagination));
                if (query.PgNumber + 1 <= item.Pagination.TotalPages)
                {
                    var queryString = new WordUrlQuery() { PgNumber = query.PgNumber + 1, PgRegister = query.PgRegister, Date = query.Date };
                    list.Links.Add(new LinkDTO("next", Url.Link("GetAll", queryString), "GET"));
                }

                if (query.PgNumber - 1 > 0)
                {
                    var queryString = new WordUrlQuery() { PgNumber = query.PgNumber - 1, PgRegister = query.PgRegister, Date = query.Date };
                    list.Links.Add(new LinkDTO("prev", Url.Link("GetAll", queryString), "GET"));
                }
            }

            return list;
        }
    }
}


