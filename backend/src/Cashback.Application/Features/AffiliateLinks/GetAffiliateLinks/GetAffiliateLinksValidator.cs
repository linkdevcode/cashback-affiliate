using FluentValidation;

namespace Cashback.Application.Features.AffiliateLinks.GetAffiliateLinks;

/// <summary>
/// Validator for paginated affiliate link list queries.
/// </summary>
public sealed class GetAffiliateLinksValidator : AbstractValidator<GetAffiliateLinksQuery>
{
    /// <summary>
    /// Initializes validation rules for affiliate link list queries.
    /// </summary>
    public GetAffiliateLinksValidator()
    {
        RuleFor(query => query.Page)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page must be at least 1.");

        RuleFor(query => query.PageSize)
            .InclusiveBetween(1, 100)
            .WithMessage("Page size must be between 1 and 100.");
    }
}
