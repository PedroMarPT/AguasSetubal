using System;

namespace AguasSetubal.Models
{
    public class LeituraContador
    {
        // Chave primária da tabela LeituraContador
        public int Id { get; set; }

        // Data da leitura atual
        public DateTime DataLeitura { get; set; }

        // Valor da leitura atual em metros cúbicos (m³)
        public int Valor { get; set; }

        // Chave estrangeira que referencia o Cliente
        public int ClienteId { get; set; }

        // Propriedade de navegação para acessar o Cliente relacionado
        public Cliente Cliente { get; set; }

        // Data da leitura anterior
        public DateTime DataLeituraAnterior { get; set; }

        // Valor da leitura anterior em metros cúbicos (m³)
        public int LeituraAnterior { get; set; }

        // Propriedade calculada que retorna o consumo em m³ (leitura atual - leitura anterior)
        public int Consumo => Valor - LeituraAnterior;

        // Valor a pagar com base no consumo, calculado automaticamente
        public decimal ValorPagar { get; set; }

        // Método para calcular o valor a pagar com base no consumo
        public void CalcularValorPagar()
        {
            // Exemplo de cálculo baseado em escalões de consumo
            if (Consumo <= 5)
            {
                ValorPagar = Consumo * 0.30m; // Até 5 m³, o custo é 0.30 por m³
            }
            else if (Consumo <= 15)
            {
                ValorPagar = (5 * 0.30m) + (Consumo - 5) * 0.50m; // De 6 a 15 m³, o custo é 0.50 por m³
            }
            else if (Consumo <= 25)
            {
                ValorPagar = (5 * 0.30m) + (10 * 0.50m) + (Consumo - 15) * 0.75m; // De 16 a 25 m³, o custo é 0.75 por m³
            }
            else
            {
                ValorPagar = (5 * 0.30m) + (10 * 0.50m) + (10 * 0.75m) + (Consumo - 25) * 1.00m; // Acima de 25 m³, o custo é 1.00 por m³
            }
        }
    }
}




