using AguasSetubal.Data;
using Org.BouncyCastle.Bcpg.Sig;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AguasSetubal.Models
{
    public class Contador : IEntity
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nº contrato é de preenchimento obrigatório")]
        [Display(Name = "Nº contrato")]
        public string NumeroContrato { get; set; }

        [Required(ErrorMessage = "Morada do local é de preenchimento obrigatório")]
        [Display(Name = "Morada do local")]
        public string MoradaLocal { get; set; }

        [Required(ErrorMessage = "Nº contador é de preenchimento obrigatório")]
        [Display(Name = "Nº contador")]
        public string NumeroContador { get; set; }

        [Required(ErrorMessage = "Data da instalação é de preenchimento obrigatório")]
        [Display(Name = "Data instalação")]
        public DateTime DataInstalacao { get; set; }

        public List<LeituraContador> LeiturasContador { get; set; }

        public int ClienteId { get; set; }

        public Cliente Cliente { get; set; }

        public Contador()
        {
            LeiturasContador = new List<LeituraContador>();
        }

    }
}