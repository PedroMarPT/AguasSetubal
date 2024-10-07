using System;
using System.ComponentModel.DataAnnotations;

namespace AguasSetubal.Models
{
    public class RequisicaoContador
    {

        public int Id { get; set; }

        [Required(ErrorMessage = "Nome do proprietário é de preenchimento obrigatório")]
        [Display(Name = "Nome do proprietário")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Morada do local a instalar é de preenchimento obrigatório")]
        [Display(Name = "Morada do local a instalar")]
        public string Morada { get; set; }

        [Required(ErrorMessage = "Contato telefónico é de preenchimento obrigatório")]
        [Display(Name = "Contato telefónico")]
        public string Contato { get; set; }

        [Required(ErrorMessage = "E-mail é de preenchimento obrigatório")]
        [EmailAddress]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Display(Name = "Residencial?")]
        public bool IsResidencial { get; set; }

        [Display(Name = "Empresarial?")]
        public bool IsEmpresarial { get; set; }

        [Display(Name = "Data da requisição")]
        public DateTime DataRequisicao { get; set; }

        [Display(Name = "Validado? Pode ser encaminhado para o admin?")]
        public bool IsValid { get; set; }

        [Display(Name = "Está cliente criado e requisitado?")]
        public bool IsRequested { get; set; }

    }
}
