using MediatR;
using System.Threading;
using FluentValidation;

namespace FSTest.Search.Application.Queries
{
	public class SearchJokesAndPeopleQueryValidator : AbstractValidator<SearchJokesAndPeopleQuery>
	{
		public SearchJokesAndPeopleQueryValidator()
		{
			CascadeMode = CascadeMode.Stop;

			RuleFor(x => x.Query).NotNull()
				.NotEmpty()
				.MinimumLength(3)
				.MaximumLength(120);
		}
	}
}
