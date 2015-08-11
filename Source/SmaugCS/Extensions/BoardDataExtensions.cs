using System;
using Realm.Library.Common;
using SmaugCS.Board;
using SmaugCS.Data.Instances;

namespace SmaugCS.Extensions
{
    public static class BoardDataExtensions
    {
        public static bool CanPost(this BoardData board, PlayerInstance ch)
        {
            if (ch.Trust >= board.MinimumPostLevel) return true;
            if (string.IsNullOrWhiteSpace(board.PostGroup)) return false;
            if (ch.PlayerData.Clan != null
                && ch.PlayerData.Clan.Name.Equals(board.PostGroup, StringComparison.OrdinalIgnoreCase))
                return true;
            return ch.PlayerData.Council != null
                   && ch.PlayerData.Council.Name.Equals(board.PostGroup, StringComparison.OrdinalIgnoreCase);
        }

        public static bool CanRemove(this BoardData board, PlayerInstance ch)
        {
            if (ch.Trust >= board.MinimumRemoveLevel) return true;
            return !string.IsNullOrWhiteSpace(board.ExtraRemovers) && ch.Name.IsEqual(board.ExtraRemovers);
        }

        public static bool CanRead(this BoardData board, PlayerInstance ch)
        {
            if (ch.Trust >= board.MinimumReadLevel) return true;

            if (string.IsNullOrWhiteSpace(board.ReadGroup))
                return !string.IsNullOrWhiteSpace(board.ExtraReaders) && ch.Name.IsEqual(board.ExtraReaders);
            if (ch.PlayerData.Clan != null
                && ch.PlayerData.Clan.Name.Equals(board.ReadGroup, StringComparison.OrdinalIgnoreCase))
                return true;
            if (ch.PlayerData.Council != null
                && ch.PlayerData.Council.Name.Equals(board.ReadGroup, StringComparison.OrdinalIgnoreCase))
                return true;
            return !string.IsNullOrWhiteSpace(board.ExtraReaders) && ch.Name.IsEqual(board.ExtraReaders);
        }
    }
}
