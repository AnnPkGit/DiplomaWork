using FluentValidation;

namespace Application.Follows.Queries.GetFollowRecommendations;

public class GetFollowRecommendationsQueryValidator : AbstractValidator<GetFollowRecommendationsQuery>
{
    public GetFollowRecommendationsQueryValidator()
    {
        const int maxAccountsPerQuery = 10;
        const int minAccountsPerQuery = 4;
        RuleFor(query => query.PageSize)
            .InclusiveBetween(minAccountsPerQuery, maxAccountsPerQuery);
    }
}