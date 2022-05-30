using System.ComponentModel.DataAnnotations;
using project_dc_system.CustomValidations;
using System.Linq;

namespace project_dc_system.Models
{
    public class Cliente
    {
        public int ClienteId { get; set; }

        [Required(ErrorMessage = "Informe um nome")]
        [MinLength(3, ErrorMessage = "O nome deve conter pelo menos 3 letras")]
        [Display(Name = "Nome do cliente")]
        public string ClienteName { get; set; }

        [Required(ErrorMessage = "Digite o cpf ou o cnpj")]
        [Display(Name = "CPF ou CNPJ")]
        [CpfCnpjValidations(ErrorMessage = "Digite um cpf ou cnpj válido")]
        public string CpfOrCnpj { get; set; }

        public virtual List<Telefone> Telefones { get; set; }
        public virtual List<Email> Emails { get; set; }
        public virtual List<Venda> Vendas { get; set; }
       
        public Cliente()
        {
            Telefones = (Enumerable.Empty<Telefone>()).ToList();
            Emails = (Enumerable.Empty<Email>()).ToList();
            Vendas = (Enumerable.Empty<Venda>()).ToList();
        }
    }
}
