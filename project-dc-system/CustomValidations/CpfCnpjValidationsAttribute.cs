using System.ComponentModel.DataAnnotations;


namespace project_dc_system.CustomValidations
{
    public class CpfCnpjValidationsAttribute : ValidationAttribute
    {
        public CpfCnpjValidationsAttribute() { }

        // Verifica se o cpf ou cnpj é válido
        public override bool IsValid(object? value)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
                return false;

            return ValidateCpfCnpj(value.ToString());
        }

        private bool ValidateCpfCnpj(string? value)
        {
            // Separa apenas os números para realizar a validação
            value = String.Join("", System.Text.RegularExpressions.Regex.Split(value, @"[^\d]"));

            if (value.Length == 11)
            {
                string novoCpf = value;
                novoCpf = geraDigitosCPF(novoCpf.Substring(0, 9));
                // Verifica se os dígitos do cpf são válidos e se ele não é uma sequência do mesmo número
                if (string.Compare(value, novoCpf) == 0 && !(verificaSequencia(value)))
                    return true;
                return false;
            }
            if(value.Length == 14)
            {
                string novoCnpj = value;
                novoCnpj = geraDigitosCNPJ(novoCnpj.Substring(0, 12));
                // Verifica se os dígitos do cnpj são válidos
                if (string.Compare(value, novoCnpj) == 0)
                    return true;
                return false;
            }
            return false;
        }

        // Gera os dois últimos dígitos do cpf baseado nos 9 primeiros
        public static string geraDigitosCPF(string cpf)
        {
            if (cpf.Length != 9)
                return "";

            int soma = 0;
            int regressivo = 10;
            for (int i = 0; i < 9; i++)
            {
                soma += int.Parse(cpf.Substring(i, 1)) * regressivo;
                regressivo--;
            }
            soma = 11 - (soma % 11);
            if (soma > 9)
                soma = 0;

            cpf = string.Concat(cpf, Convert.ToString(soma));

            soma = 0;
            regressivo = 11;
            for (int i = 0; i < 10; i++)
            {
                soma += int.Parse(cpf.Substring(i, 1)) * regressivo;
                regressivo--;
            }
            soma = 11 - (soma % 11);
            if (soma > 9)
                soma = 0;

            cpf = string.Concat(cpf, Convert.ToString(soma));

            return cpf;
        }

        // Verifica se o cpf digitado é uma sequência do mesmo número
        public static bool verificaSequencia(string cpf)
        {
            var seqCPF = new string(cpf[0], 11);
            if (string.Compare(cpf, seqCPF) == 0)
                return true;
            return false;
        }

        // Gera os dois últimos dígitos do cnpj de acordo com os 12 primeiros
        public static string geraDigitosCNPJ(string cnpj)
        {
            if (cnpj.Length != 12)
                return "";

            int soma = 0;
            int regressivo = 5;
            for (int i = 0; i < 12; i++)
            {
                if (regressivo < 2)
                    regressivo = 9;
                soma += int.Parse(cnpj.Substring(i, 1)) * regressivo;
                regressivo--;
            }
            soma = 11 - (soma % 11);
            if (soma > 9)
                soma = 0;

            cnpj = string.Concat(cnpj, Convert.ToString(soma));

            soma = 0;
            regressivo = 6;
            for (int i = 0; i < 13; i++)
            {
                if (regressivo < 2)
                    regressivo = 9;
                soma += int.Parse(cnpj.Substring(i, 1)) * regressivo;
                regressivo--;
            }
            soma = 11 - (soma % 11);
            if (soma > 9)
                soma = 0;

            cnpj = string.Concat(cnpj, Convert.ToString(soma));

            return cnpj;
        }
    }
}
