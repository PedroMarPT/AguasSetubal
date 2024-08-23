using System;

namespace AguasSetubal.Models
{
    public class Fatura
    {
        public int Id { get; set; }

        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }

        public DateTime DataEmissao { get; set; }
        public string Endereco { get; set; }

        public int LeituraContadorId { get; set; }
        public LeituraContador LeituraContador { get; set; }

        public decimal LeituraAnterior { get; set; }
        public decimal LeituraAtual { get; set; }

        public decimal M3Gastos
        {
            get => LeituraAtual - LeituraAnterior;
        }

        public decimal ValorTotal { get; set; }
    }
}







