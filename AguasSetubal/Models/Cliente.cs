using AguasSetubal.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AguasSetubal.Models
{
    public class Cliente : IEntity
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nome é de preenchimento obrigatório")]
        [Display(Name = "Nome")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Morada é de preenchimento obrigatório")]
        [Display(Name = "Morada")]
        public string MoradaFaturacao { get; set; }

        [Display(Name = "CC")]
        public string NumeroCartaoCidadao { get; set; }

        [Required(ErrorMessage = "NIF é de preenchimento obrigatório")]
        [Display(Name = "NIF")]
        public string NIF { get; set; }

        [Required(ErrorMessage = "Telefone é de preenchimento obrigatório")]
        [Display(Name = "Telefone")]
        public string ContactoTelefonico { get; set; }

        [Required(ErrorMessage = "Email é de preenchimento obrigatório")]
        [EmailAddress]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        public List<Contador> Contadores { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }

        public Cliente()
        {
            Contadores = new List<Contador>();
        }
    }
}






