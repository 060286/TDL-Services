using FluentValidation;
using TDL.Services.Dto;

namespace TDL.Services.Validators
{
    public class PaginationRequestDtoValidator : AbstractValidator<PaginationRequestDto>
    {
        public PaginationRequestDtoValidator()
        {
            RuleFor(x => x.PageIndex).GreaterThanOrEqualTo(1);
            RuleFor(x => x.PageSize).GreaterThan(0);
        }
    }
}
