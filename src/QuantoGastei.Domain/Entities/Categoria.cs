using QuantoGastei.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuantoGastei.Domain.Entities
{
    [Table(name: "CATEGORIA")]
    public class Categoria : EntityBase
    {
        [Column(name: "DESCRICAO", Order = 2)]
        [Nota(UseIndex = true)]
        public string? Descricao { get; set; }
    }
}
