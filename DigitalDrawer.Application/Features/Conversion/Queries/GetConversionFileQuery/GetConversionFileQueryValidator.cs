using FluentValidation;

namespace DigitalDrawer.Application.Features.Conversion.Queries.GetConversionFileQuery
{
    public class GetConversionFileQueryValidator : AbstractValidator<GetConversionFileQuery>
    {
        public GetConversionFileQueryValidator()
        {
            RuleFor(x => x.Id).NotNull().NotEmpty();
        }
    }
}