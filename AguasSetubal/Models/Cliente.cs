using System.ComponentModel.DataAnnotations;

namespace AguasSetubal.Models
{
    public class Cliente
    {
        public int Id { get; set; }  // Chave primária

        [Required]
        [StringLength(100)]
        public string Nome { get; set; }  // Nome do Cliente

        public string Endereco { get; set; }  // Endereço 

        [Required]
        [StringLength(200)]
        public string Morada { get; set; }  // Morada do Cliente

        [Required]
        [StringLength(20)]
        public string NumeroCartaoCidadao { get; set; }  // Número de Cartão de Cidadão

        [Required]
        [StringLength(15)]
        public string NIF { get; set; }  // Número de Identificação Fiscal

        [Required]
        [StringLength(15)]
        [Display(Name = "Contacto Telefónico")]
        public string ContactoTelefonico { get; set; }  // Contacto Telefónico

        [Required]
        [StringLength(20)]
        public string NumeroContrato { get; set; }  // Número de Contrato

        [Required]
        [StringLength(20)]
        public string NumeroContador { get; set; }  // Número de Contador

        [Required]
        [Display(Name = "Leitura Atual do Contador")]
        public decimal LeituraAtualContador { get; set; }  // Leitura Atual do Contador
    }
}



