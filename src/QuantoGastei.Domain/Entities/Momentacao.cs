using QuantoGastei.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuantoGastei.Domain.Entities
{
    [Table(name: "CATEGORIA")]
    public class Momentacao : EntityBase
    {
        [Column(name: "DESCRICAO", Order = 2)]
        [Nota(UseIndex = true)]
        public string? Descricao { get; set; }

        [Column(name: "NR_PARCELA", Order = 3)]
        [Nota()]
        public int NumeroParcela { get; set; }

        [Column(name: "TOTAL_PARCELAS", Order = 4)]
        [Nota()]
        public int TotalParcelas { get; set; }

        [Column(name: "DATA_LANCAMENTO", Order = 5)]
        [Nota(UseIndex = true)]
        public DateTime DataLancamento { get; set; }

        [Column(name: "DATA_PAGAMENTO", Order = 6)]
        [Nota(UseIndex = true)]
        public DateTime DataPagamento { get; set; }

        [Column(name: "TIPO_LANCAMENTO", Order = 7)]
        [Nota()]
        public int TipoLancamento { get; set; }

        [Column(name: "VALOR", Order = 8)]
        [Nota()]
        public double Valor { get; set; }
        [Nota()]

        [Column(name: "ID_USUARIO", Order = 9)]
        public int CodigoUsuario { get; set; }
    }
}
