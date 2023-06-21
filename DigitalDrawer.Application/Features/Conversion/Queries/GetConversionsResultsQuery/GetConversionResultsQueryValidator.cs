using FluentValidation;

namespace DigitalDrawer.Application.Features.Conversion.Queries.GetConversionsResultsQuery
{
    public class GetConversionResultsQueryValidator : AbstractValidator<GetConversionResultsQuery>
    {
        public GetConversionResultsQueryValidator()
        {
            RuleFor(x => x.ConvertionTaskId).NotEmpty().NotNull();
        }
    }
}