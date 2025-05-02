using Furia_FanHub.MVVM.Models.Interfaces;
using System.Text.RegularExpressions;

namespace Furia_FanHub.MVVM.Helpers
{
    public class RegexValidationRule : IValidationRule<string>
    {
        public string Pattern { get; set; }
        public string ValidationMessage { get; set; } = "Formato de e-mail inválido.";

        public bool Check(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return false;
            return Regex.IsMatch(value, Pattern);
        }
    }
}
