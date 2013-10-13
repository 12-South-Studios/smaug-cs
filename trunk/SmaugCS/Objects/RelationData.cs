using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmaugCS.Enums;


namespace SmaugCS.Objects
{
    public class RelationData
    {
        public object Actor { get; set; }
        public object Subject { get; set; }
        public RelationTypes Types { get; set; }
    }
}
