using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmaugCS.Board
{
    public static class SqlProcedureStatics
    {
        #region Boards
        public const string BoardGetAll =
            @"SELECT b.BoardId, b.ReadGroup, b.PostGroup, b.ExtraReaders, b.ExtraRemovers, 
                b.OTakeMessage, b.OPostMessage, b.ORemoveMessage, b.OCopyMessage, b.PostMessage, 
                b.OReadMessage, b.OListMessage, b.MinimumReadLevel, b.MinimumPostLevel, 
                b.MinimumRemoveLevel, b.MaximumPosts, bt.Name as BoardType FROM Boards b 
                JOIN BoardTypes bt ON b.BoardTypeId = bt.BoardTypeId;";

        public const string BoardGetNotes =
            @"SELECT n.NoteId, n.Sender, n.DateSent, n.RecipientList, n.Subject, n.Voting, 
                n.YesVotes, n.NoVotes, n.Abstentions, n.Text FROM BoardNoteMap bnm 
                JOIN Notes n ON bnm.NoteId = n.NoteId WHERE bnm.BoardId = @BoardId;";

        public const string BoardSave =
            @"INSERT INTO Boards (ReadGroup, PostGroup, ExtraReaders, ExtraRemovers, 
                OTakeMessage, OPostMessage, ORemoveMessage, OCopyMessage, PostMessage, 
                OReadMessage, MinimumReadLevel, MinimumPostLevel, MinimumRemoveLevel, 
                MaximumPosts, BoardTypeId, OListMessage) VALUES (@ReadGroup, @PostGroup, 
                @ExtraReaders, @ExtraRemovers, @OTakeMessage, @OPostMessage, @ORemoveMessage, 
                @OCopyMessage, @PostMessage, @OReadMessage, @MinReadLevel, @MinPostLevel, 
                @MinRemoveLevel, @MaxPosts, @BoardTypeId, @OListMessage);";

        public const string BoardSaveNote =
            @"INSERT INTO BoardNoteMap (BoardId, NoteId) VALUES (@BoardId, @NoteId);";

        public const string BoardDelete =
            @"DELETE FROM Boards WHERE BoardId = @BoardId;";

        public const string BoardDeleteNote =
            @"DELETE FROM BoardNoteMap WHERE BoardId = @BoardId AND NoteId = @NoteId;";

        public const string BoardDeleteNotes =
            @"DELETE FROM BoardNoteMap WHERE BoardId = @BoardId;";

        public const string BoardUpdate =
            @"UPDATE Boards SET ReadGroup = @ReadGroup, PostGroup = @PostGroup, 
                ExtraReaders = @ExtraReaders, ExtraRemovers = @ExtraRemovers, 
                OTakeMessage = @OTakeMessage, OPostMessage = @OPostMessage, 
                ORemoveMessage = @ORemoveMessage, OCopyMessage = @OCopyMessage, 
                PostMessage = @PostMessage, OReadMessage = @OReadMessage, 
                MinimumReadLevel = @MinReadLevel, MinimumPostLevel = @MinPostLevel, 
                MinimumRemoveLevel = @MinRemoveLevel, MaximumPosts = @MaxPosts, 
                BoardTypeId = @BoardTypeId, OListMessage = @OListMessage WHERE 
                BoardId = @BoardId;";
        #endregion

        #region Notes
        public const string NoteSave =
            @"INSERT INTO Notes (Sender, DateSent, RecipientList, Subject, Voting, 
                YesVotes, NoVotes, Abstentions, Text) VALUES (@Sender, @DateSent, 
                @RecipientList, @Subject, @Voting, @YesVotes, @NoVotes, @Abstentions, 
                @Text);";

        public const string NoteUpdate =
            @"UPDATE Notes SET Sender = @Sender, DateSent = @DateSent, RecipientList = 
                @RecipientList, Subject = @Subject, Voting = @Voting, YesVotes = @YesVotes, 
                NoVotes = @NoVotes, Abstentions = @Abstentions, Text = @Text WHERE 
                NoteId = @NoteId;";

        public const string NoteDelete =
            @"DELETE FROM Notes WHERE NoteId = @NoteId;";
        #endregion

    }
}
