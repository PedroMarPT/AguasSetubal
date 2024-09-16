using AguasSetubal.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AguasSetubal.Models
{
    public class Cliente : IEntity 
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Endereco { get; set; }
        public string Morada { get; set; }
        public string NumeroCartaoCidadao { get; set; }
        public string NIF { get; set; }
        public string ContactoTelefonico { get; set; }
        public string NumeroContrato { get; set; }
        public decimal LeituraAtualContador { get; set; }
        public decimal LeituraAnteriorContador { get; set; }
        public string NumeroContador { get; set; } // Adicione esta linha

        public ICollection<Fatura> Faturas { get; set; }
        public ICollection<LeituraContador> Leituras { get; set; }
    }
}






