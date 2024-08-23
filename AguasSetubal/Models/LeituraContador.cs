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
        public decimal M3Gastos { get; set; } // Consumo calculado (LeituraAtual - LeituraAnterior)

        public void CalcularValorPagar()
            {
                // Lógica de cálculo do valor a pagar
            }

            // Propriedade de navegação para Fatura
            public Fatura Fatura { get; set; }
        

    }
}







