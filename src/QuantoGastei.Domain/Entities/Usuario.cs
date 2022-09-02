using QuantoGastei.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuantoGastei.Domain.Entities
{
    [Table(name: "USUARIO")]
    public class Usuario : EntityBase
    {
        [Column(name: "NOME", Order = 2)]
        [Nota(UseIndex = true)]
        public string? Nome { get; set; }

        [Column(name: "EMAIL", Order = 3)]
        [Nota(UseIndex = true)]
        public string? Email { get; set; }

        [Column(name: "SENHA", Order = 4)]
        [Nota()]
        public string? Senha { get; set; }

    }
}
