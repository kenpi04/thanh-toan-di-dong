using FluentValidation;
using PlanX.Admin.Models.News;

namespace PlanX.Admin.Validators.News
{
    public class TagValidator : AbstractValidator<TagModel>
    {
        public TagValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name là bắt buộc");
        }
    }
}