using System;

namespace AguasSetubal.Models
{
    public class Fatura
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public decimal ValorTotal { get; set; }
        public int LeituraContadorId { get; set; }
        public LeituraContador LeituraContador { get; set; }
        public Cliente Cliente { get; set; }
        public DateTime DataEmissao { get; set; }
        public decimal LeituraAnterior { get; set; }  // Nova propriedade para armazenar a leitura anterior
        public decimal LeituraAtual { get; set; }     // Nova propriedade para armazenar a leitura atual

        // Propriedade calculada que retorna a diferença entre a leitura atual e a anterior (M3 consumidos)
       // public int M3 => LeituraAtual - LeituraAnterior;
        public decimal M3Gastos { get; set; }  // Propriedade para armazenar M3Gastos
        public string Endereco { get; set; } // Endereço do cliente

       
    }
}







