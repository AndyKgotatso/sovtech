using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FSTest.Search.Domain.Entities;
using JW;

namespace FSTest.Search.Application.Queries
{
    public class JokesAndPeopleViewModel
    {
        public JokesResponse Chuck { get; set; }
        public Pager ChuckPager { get; set; }
        public PeopleResponse Swapi { get; set; }
    }
}
