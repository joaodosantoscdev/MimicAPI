using MimicAPI.Helpers;
using MimicAPI.V1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MimicAPI.V1.Repositories.Interfaces
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
