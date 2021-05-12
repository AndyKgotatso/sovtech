using System;
using System.Collections.Generic;

namespace FSTest.Search.Domain.Entities
{
    public class JokesResponse
    {
        public int Total { get; set; }
        public List<JokeResult> Result { get; set; }
    }

    public class JokeResult
    {
        public List<string> Categories { get; set; }
        public string Created_at { get; set; }
        public string Icon_url { get; set; }
        public string Id { get; set; }
        public string Updated_at { get; set; }
        public string Url { get; set; }
        public string Value { get; set; }
    }

}
