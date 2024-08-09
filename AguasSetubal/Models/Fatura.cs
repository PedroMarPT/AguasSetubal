using System;

namespace AguasSetubal.Models
{
    public class Fatura
    {
        public int Id { get; set; }
        public DateTime Data { get; set; }
        public decimal Valor { get; set; }
        public int ClienteId { get; set; } // Chave estrangeira para Cliente
        public Cliente Cliente { get; set; } // Propriedade de navegação
    }
}
