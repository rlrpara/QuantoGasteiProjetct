namespace QuantoGastei.Domain.Entities.Base
{
    public class Nota : Attribute
    {
        public bool PrimaryKey { get; set; } = false;
        public bool UseDatabase { get; set; } = true;
        public bool UseToGet { get; set; } = true;
        public string ForeignKey { get; set; } = "";
        public bool TextMax { get; set; } = false;
        public bool UseIndex { get; set; } = false;
    }
}
