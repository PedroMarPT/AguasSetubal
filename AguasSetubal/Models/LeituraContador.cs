using System;

namespace AguasSetubal.Models
{
    public class LeituraContador
    {
        public int Id { get; set; }
        public decimal LeituraAnterior { get; set; } // Mantido
        public decimal LeituraAtual { get; set; } // Corrigido para decimal
        public decimal Valor { get; set; }
        public decimal Consumo { get; set; }
        public DateTime DataLeitura { get; set; }
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }
        public DateTime DataLeituraAnterior { get; set; }
        public decimal ValorPagar { get; set; }

        public void CalcularValorPagar()
        {
            // Lógica para calcular o valor a pagar com base no consumo
            ValorPagar = Consumo * 1.5m; // Exemplo: Multiplicando o consumo por 1,5 (valor por m³)
        }

        public Fatura Fatura { get; set; }
    }
}







