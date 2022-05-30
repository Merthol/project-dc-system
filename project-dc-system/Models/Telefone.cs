using System.ComponentModel.DataAnnotations;
using project_dc_system.CustomValidations;

namespace project_dc_system.Models
{
    public class Telefone
    {
        public int TelefoneId { get; set; }

        [Required(ErrorMessage = "Digite o telefone")]
        [Display(Name ="Telefone")]
        [TelefoneValidations(ErrorMessage = "Tamanho de telefone inválido")]
        public string Fone { get; set; }
        public int ClienteId { get; set; }
        public Cliente? Cliente { get; set; }
    }
}
