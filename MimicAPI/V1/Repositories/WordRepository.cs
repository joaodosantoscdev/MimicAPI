using Microsoft.EntityFrameworkCore;
using MimicAPI.Database;
using MimicAPI.Helpers;
using MimicAPI.V1.Models;
using MimicAPI.V1.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MimicAPI.V1.Repositories
{
    public class WordRepository : IWordRepository
    {
        private readonly MimicContext _context;
        public WordRepository(MimicContext context)
        {
            _context = context;
        }

        public PaginationList<Word> GetAllWords(WordUrlQuery query)
        {
            var list = new PaginationList<Word>();
            var item = _context.Words.AsNoTracking().AsQueryable();

            if (query.Date.HasValue)
            {
                item = item.Where(i => i.Created > query.Date.Value);
            }

            if (query.PgNumber.HasValue)
            {
                var qntTotalRegis = item.Count();
                item = item.Skip((query.PgNumber.Value - 1) * query.PgRegister.Value).Take(query.PgRegister.Value);

                var pagination = new Pagination
                {
                    NumPage = query.PgNumber.Value,
                    RegisPage = query.PgRegister.Value,
                    TotalRegis = qntTotalRegis,
                    TotalPages = (int)Math.Ceiling((double)qntTotalRegis / query.PgRegister.Value)
                };

                list.Pagination = pagination;
               
            }
            list.Results.AddRange(item.ToList());
            
            return list;
        }

        public Word GetWord(int id)
        {
            return _context.Words.AsNoTracking().FirstOrDefault(a => a.Id == id);
        }

        public void Add(Word word)
        {
            _context.Words.Add(word);
            _context.SaveChanges();
        }

        public void Update(Word word)
        {
            _context.Words.Update(word);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var obj = GetWord(id);
            obj.Active = false;
            _context.Words.Update(obj);
            _context.SaveChanges();
        }

    }
}
