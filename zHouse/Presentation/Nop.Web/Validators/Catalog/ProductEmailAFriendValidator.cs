using FluentValidation;
using Nop.Services.Localization;
using Nop.Web.Models.Catalog;

namespace Nop.Web.Validators.Catalog
{
    public class ProductEmailAFriendValidator : AbstractValidator<ProductEmailAFriendModel>
    {
        public ProductEmailAFriendValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.FriendEmail).NotEmpty().WithMessage(localizationService.GetResource("Products.EmailAFriend.FriendEmail.Required"));
            RuleFor(x => x.FriendEmail).EmailAddress().WithMessage(localizationService.GetResource("Common.WrongEmail"));
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Products.EmailAFriend.Name.Required"));
            RuleFor(x => x.YourEmailAddress).NotEmpty().WithMessage(localizationService.GetResource("Products.EmailAFriend.YourEmailAddress.Required")).When(x=>x.Hour==0);
            RuleFor(x => x.YourEmailAddress).EmailAddress().WithMessage(localizationService.GetResource("Common.WrongEmail"));
            RuleFor(x => x.Minute).NotEqual(0).WithMessage(localizationService.GetResource("Products.EmailAFriend.Minute.Required")).When(x => x.Hour > 0);
            RuleFor(x => x.Date).NotEqual(0).WithMessage(localizationService.GetResource("Products.EmailAFriend.Date.Required")).When(x => x.Hour > 0);
            RuleFor(x => x.Year).NotEqual(0).WithMessage(localizationService.GetResource("Products.EmailAFriend.Year.Required")).When(x => x.Hour > 0);
            RuleFor(x => x.Hour).NotEqual(0).WithMessage(localizationService.GetResource("Products.EmailAFriend.Hour.Required")).When(x => x.Hour > 0);
        }}
}