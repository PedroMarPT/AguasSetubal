using AguasSetubal.Data;
using System.ComponentModel.DataAnnotations;

namespace AguasSetubal.Models
{
    public class TabelaPrecos : IEntity
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Escalão é de preenchimento obrigatório")]
        [Display(Name = "Escalão")]
        public string NomeEscalao { get; set; }

        [Required(ErrorMessage = "Limite inferior é de preenchimento obrigatório")]
        [Display(Name = "Limite inferior")]
        public int LimiteInferior { get; set; }

        [Required(ErrorMessage = "Limite superior é de preenchimento obrigatório")]
        [Display(Name = "Limite superior")]
        public int LimiteSuperior { get; set; }

        [Required(ErrorMessage = "Valor unitário é de preenchimento obrigatório")]
        [Display(Name = "Valor unitário")]
        public decimal ValorUnitario { get; set; }
    }
}
