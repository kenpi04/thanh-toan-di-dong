using System.Collections.Generic;
using PlanX.Web.Framework.Mvc;

namespace PlanX.Web.Models.Common
{
    public partial class LanguageSelectorModel : BaseNopModel
    {
        public LanguageSelectorModel()
        {
            AvailableLanguages = new List<LanguageModel>();
        }

        public IList<LanguageModel> AvailableLanguages { get; set; }

        public int CurrentLanguageId { get; set; }

        public bool UseImages { get; set; }
    }
}