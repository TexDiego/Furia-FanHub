using Furia_FanHub.MVVM.Models.Interfaces;

namespace Furia_FanHub.MVVM.Helpers
{
    public class CPFValidationRule : IValidationRule<string>
    {
        public string ValidationMessage { get; set; } = "CPF inválido";

        public bool Check(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return false;

            var cpf = new string(value.Where(char.IsDigit).ToArray());

            if (cpf.Length != 11 || cpf.Distinct().Count() == 1)
                return false;

            int soma = 0;
            for (int i = 0; i < 9; i++)
                soma += (cpf[i] - '0') * (10 - i);

            int dig1 = soma % 11;
            dig1 = dig1 < 2 ? 0 : 11 - dig1;
            if ((cpf[9] - '0') != dig1)
                return false;

            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += (cpf[i] - '0') * (11 - i);

            int dig2 = soma % 11;
            dig2 = dig2 < 2 ? 0 : 11 - dig2;
            return (cpf[10] - '0') == dig2;
        }
    }
}
