using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmaugCS.DAL.Models
{
    [Table("CharacterFriends")]
    public class CharacterFriend : Entity
    {
        [Required]
        public int CharacterId { get; set; }

        [ForeignKey("CharacterId")]
        public virtual Character Character { get; set; }

        public string FriendName { get; set; }

        public DateTime AddedOn { get; set; }
    }
}
