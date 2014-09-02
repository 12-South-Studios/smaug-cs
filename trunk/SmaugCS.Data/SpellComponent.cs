using SmaugCS.Constants.Enums;

namespace SmaugCS.Data
{
    public class SpellComponent
    {
        public ComponentRequiredTypes RequiredType { get; set; }
        public string RequiredData { get; set; }
        public ComponentOperatorTypes OperatorType { get; set; }
    }
}
