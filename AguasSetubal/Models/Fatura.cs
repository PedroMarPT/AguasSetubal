using System;

namespace AguasSetubal.Models
{
    public class Fatura
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }
        public DateTime DataEmissao { get; set; }  // Coluna 'DataEmissao'
        public int LeituraContadorId { get; set; } // Coluna 'LeituraContadorId'
        public LeituraContador LeituraContador { get; set; }
        public decimal ValorTotal { get; set; }    // Coluna 'ValorTotal'
        public string Endereco { get; set; }       // Coluna 'Endereco'

       
    }
}



