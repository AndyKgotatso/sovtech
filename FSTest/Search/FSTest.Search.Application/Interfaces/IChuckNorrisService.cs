using FSTest.Search.Domain.Entities;
using JW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSTest.Search.Application.Interfaces
{
    public interface IChuckNorrisService
    {
        Task<JokesResponse> GetJokes(string query);
        List<JokeResult> PageJokesResponseResult(List<JokeResult> jokeResultsList, Pager chuckPager);
    }
}
