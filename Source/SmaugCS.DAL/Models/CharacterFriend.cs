using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmaugCS.DAL.Models
{
    [Table("CharacterFriends")]
    public class CharacterFriend : Entity
    {
        public string FriendName { get; set; }

        public DateTime AddedOn { get; set; }
    }
}
