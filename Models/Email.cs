using System.ComponentModel.DataAnnotations;

namespace project_dc_system.Models
{
    public class Email
    {
        public int EmailId { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Informe o seu email")]
        [RegularExpression(".+\\@.+\\..+", ErrorMessage = "Informe um email válido...")]
        public string EmailAdress { get; set; }
        public int ClienteId { get; set; }
        public Cliente? Cliente { get; set; }
    }
}
