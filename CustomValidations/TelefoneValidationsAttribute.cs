using System.ComponentModel.DataAnnotations;

namespace project_dc_system.CustomValidations
{
    public class TelefoneValidationsAttribute : ValidationAttribute
    {
        public TelefoneValidationsAttribute() { }

        // Verifica se o tamanho do telefone digitado é válido
        public override bool IsValid(object? value)
        {
            if (value == null)
                return false;

            // Separa apenas os números para realizar a validação
            value = String.Join("", System.Text.RegularExpressions.Regex.Split(value.ToString(), @"[^\d]"));
            if (value.ToString().Length == 11 || value.ToString().Length == 10)
                return true;

            return false;
        }

    }
}
