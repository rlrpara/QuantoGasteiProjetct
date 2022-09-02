using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuantoGastei.Domain.Entities.Base
{
    public class EntityBase
    {
        [Key]
        [Column(name: "ID", Order = 1)]
        [Nota(PrimaryKey = true, UseIndex = true)]
        public int Codigo { get; set; }

        [Column(name: "CRIADO_EM", Order = 99)]
        [Nota()]
        public DateTime CriadoEm { get; set; } = DateTime.Now;

        [Column(name: "ATUALZIADO_EM", Order = 100)]
        [Nota()]
        public DateTime AtualizadoEm { get; set; } = DateTime.Now;
    }
}
