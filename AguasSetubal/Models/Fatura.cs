using AguasSetubal.Data;
using Org.BouncyCastle.Bcpg.Sig;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AguasSetubal.Models
{
    public class Fatura : IEntity
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Cliente é de preenchimento obrigatório")]
        public int ClienteId { get; set; }

        public Cliente Cliente { get; set; }

        [Required(ErrorMessage = "Contador é de preenchimento obrigatório")]
        public int ContadorId { get; set; }

        public Contador Contador { get; set; }

        [Required(ErrorMessage = "Data início é de preenchimento obrigatório")]
        [Display(Name = "Data início")]
        public DateTime DataInicio { get; set; }

        [Required(ErrorMessage = "Data fim é de preenchimento obrigatório")]
        [Display(Name = "Data fim")]
        public DateTime DataFim { get; set; }

        [Display(Name = "Data emissão")]
        public DateTime DataEmissao { get; set; }

        [Display(Name = "Descritivo")]
        public string Descritivo { get; set; }

        [Display(Name = "Consumo total")]
        public decimal ConsumoTotal { get; set; }

        [Display(Name = "Valor unitário")]
        public decimal ValorUnitario { get; set; }

        [Display(Name = "Valor total")]
        public decimal ValorTotal { get; set; }

        public List<LeituraContador> LeiturasContador { get; set; }

        public Fatura()
        {
            LeiturasContador = new List<LeituraContador>();
        }

    }
}







