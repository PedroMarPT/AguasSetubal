using System;

namespace AguasSetubal.Models
{
    public class LeituraContador
    {
        public int Id { get; set; }
        public DateTime DataLeitura { get; set; }
        public int Valor { get; set; }
        public int ClienteId { get; set; } // Chave estrangeira para Cliente
        public Cliente Cliente { get; set; } // Propriedade de navegação
    }
}

