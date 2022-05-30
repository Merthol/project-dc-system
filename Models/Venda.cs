using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace project_dc_system.Models
{
    public class Venda
    {
        public int VendaId { get; set; }

        [Required(ErrorMessage = "Data da venda obrigatório")]
        [Display(Name = "Data da venda")]
        [DataType(DataType.Date)]
        public DateTime DataVenda { get; set; }

        [Required(ErrorMessage = "Valor da venda obrigatório")]
        [Column(TypeName = "decimal(8, 2)")]
        [Display(Name = "Valor da venda")]
        public decimal ValorVenda { get; set; }

        [Display(Name = "Cliente")]
        public int ClienteId { get; set; }
        public Cliente? Cliente { get; set; }
    }
}
