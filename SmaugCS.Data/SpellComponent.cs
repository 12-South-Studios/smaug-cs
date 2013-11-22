using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
