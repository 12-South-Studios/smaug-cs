using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using SmaugCS.Common.Enumerations;

namespace SmaugCS.DAL.Models
{
    [Table("Boards")]
    public class Board : Entity
    {
        public BoardTypes BoardType { get; set; }

        public string Name { get; set; }

        public string ReadGroup { get; set; }

        public string PostGroup { get; set; }

        public string ExtraReaders { get; set; }

        public string ExtraRemovers { get; set; }

        public string OTakeMessage { get; set; }

        public string OPostMessage { get; set; }

        public string ORemoveMessage { get; set; }

        public string OCopyMessage { get; set; }

        public string OListMessage { get; set; }

        public string PostMessage { get; set; }

        public string OReadMessage { get; set; }

        public int MinimumReadLevel { get; set; }

        public int MinimumPostLevel { get; set; }

        public int MinimumRemoveLevel { get; set; }

        public int MaximumPosts { get; set; }

        public long BoardObjectId { get; set; }

        public virtual ICollection<Note> Notes { get; set; }
    }
}
