using System.ComponentModel.DataAnnotations;
using System;

namespace AguasSetubal.Models.ViewModels
{
    public class LeituraContadorViewModel
    {
        [Required(ErrorMessage = "Leitura anterior é de preenchimento obrigatório")]
        [Display(Name = "Leitura anterior")]
        public decimal LeituraAnterior { get; set; }

        [Required(ErrorMessage = "Leitura atual é de preenchimento obrigatório")]
        [Display(Name = "Leitura atual")]
        public decimal LeituraAtual { get; set; }

        [Required(ErrorMessage = "Consumo é de preenchimento obrigatório")]
        [Display(Name = "Consumo")]
        public decimal Consumo { get; set; }

        [Required(ErrorMessage = "Data da leitura é de preenchimento obrigatório")]
        [Display(Name = "Data da leitura")]
        public DateTime DataLeitura { get; set; }

        [Display(Name = "Está faturado?")]
        public bool IsInvoiced { get; set; }

        [Required(ErrorMessage = "Cliente é de preenchimento obrigatório")]
        public int? ClienteId { get; set; }

        public int? FaturaId { get; set; }

        [Required(ErrorMessage = "Contador é de preenchimento obrigatório")]
        public int ContadorId { get; set; }

        public Contador Contador { get; set; }

        public Fatura Fatura { get; set; }

        public Cliente Cliente { get; set; }

        public string UserId { get; set; }
    }
}
