using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmaugCS.Board
{
    public static class SqlProcedureStatics
    {
        public const string BoardGetAll =
            @"SELECT b.BoardId, b.ReadGroup, b.PostGroup, b.ExtraReaders, b.ExtraRemovers, 
                b.OTakeMessage, b.OPostMessage, b.ORemoveMessage, b.OCopyMessage, b.PostMessage, 
                b.OReadMessage, b.MinimumReadLevel, b.MinimumPostLevel, b.MinimumRemoveLevel, 
                b.MaximumPosts, bt.Name as BoardType FROM Boards b JOIN BoardTypes bt 
                ON b.BoardTypeId = bt.BoardTypeId;";


    }
}
