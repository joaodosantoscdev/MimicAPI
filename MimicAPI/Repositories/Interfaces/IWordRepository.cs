using MimicAPI.Helpers;
using MimicAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MimicAPI.Repositories.Interfaces
{
    public interface IWordRepository
    {
        PaginationList<Word> GetAllWords(WordUrlQuery query);
        Word GetWord(int id);

        void Add(Word word);
        void Update(Word word);
        void Delete(int id);

    }
}
