using System;
using System.Data;
using NUnit.Framework;

namespace SmaugCS.Board.Tests
{
    [TestFixture]
    public class BoardDataTests
    {
        private static DataTable BuildDataTable()
        {
            DataTable table = new DataTable("BoardData");
            table.Columns.Add("BoardId");
            table.Columns.Add("ReadGroup");
            table.Columns.Add("PostGroup");
            table.Columns.Add("ExtraReaders");
            table.Columns.Add("ExtraRemovers");
            table.Columns.Add("OTakeMessage");
            table.Columns.Add("OPostMessage");
            table.Columns.Add("ORemoveMessage");
            table.Columns.Add("OCopyMessage");
            table.Columns.Add("PostMessage");
            table.Columns.Add("OReadMessage");
            table.Columns.Add("MinimumReadLevel");
            table.Columns.Add("MinimumPostLevel");
            table.Columns.Add("MinimumRemoveLevel");
            table.Columns.Add("MaximumPosts");
            table.Columns.Add("BoardTypeName");
            table.Columns.Add("OListMessage");
            return table;
        }

        [Test]
        public void Translate_NoNulls_Test()
        {
            var table = BuildDataTable();

            var row = table.NewRow();
            row["BoardId"] = 1;
            row["ReadGroup"] = "GroupA";
            row["PostGroup"] = "GroupB";
            row["ExtraReaders"] = "Bob,John";
            row["ExtraRemovers"] = "Bill,Ted";
            row["OTakeMessage"] = "$N took a message.";
            row["OPostMessage"] = "$N posted a message.";
            row["ORemoveMessage"] = "$N removed a message.";
            row["OCopyMessage"] = "$N copied a message.";
            row["PostMessage"] = "What does this do?";
            row["OReadMessage"] = "$N read a message.";
            row["MinimumReadLevel"] = 1;
            row["MinimumPostLevel"] = 5;
            row["MinimumRemoveLevel"] = 10;
            row["MaximumPosts"] = 1000;
            row["BoardTypeName"] = "Note";
            row["OListMessage"] = "$N listed the messages.";

            var board = BoardData.Translate(row);

            Assert.That(board.Id, Is.EqualTo(1));
            Assert.That(board.ReadGroup, Is.EqualTo("GroupA"));
            Assert.That(board.PostGroup, Is.EqualTo("GroupB"));
            Assert.That(board.ExtraReaders, Is.EqualTo("Bob,John"));
            Assert.That(board.ExtraRemovers, Is.EqualTo("Bill,Ted"));
            Assert.That(board.OTakeMessage, Is.EqualTo("$N took a message."));
            Assert.That(board.OPostMessage, Is.EqualTo("$N posted a message."));
            Assert.That(board.ORemoveMessage, Is.EqualTo("$N removed a message."));
            Assert.That(board.OCopyMessage, Is.EqualTo("$N copied a message."));
            Assert.That(board.PostMessage, Is.EqualTo("What does this do?"));
            Assert.That(board.OReadMessage, Is.EqualTo("$N read a message."));
            Assert.That(board.MinimumReadLevel, Is.EqualTo(1));
            Assert.That(board.MinimumPostLevel, Is.EqualTo(5));
            Assert.That(board.MinimumRemoveLevel, Is.EqualTo(10));
            Assert.That(board.MaximumPosts, Is.EqualTo(1000));
            Assert.That(board.Type, Is.EqualTo(BoardTypes.Note));
            Assert.That(board.OListMessage, Is.EqualTo("$N listed the messages."));
        }

        [Test]
        public void Translate_Nulls_Test()
        {
            var table = BuildDataTable();

            var row = table.NewRow();
            row["BoardId"] = 1;
            row["MinimumReadLevel"] = 1;
            row["MinimumPostLevel"] = 5;
            row["MinimumRemoveLevel"] = 10;
            row["MaximumPosts"] = 1000;
            row["BoardTypeName"] = "Note";

            var board = BoardData.Translate(row);

            Assert.That(board.Id, Is.EqualTo(1));
            Assert.That(board.OPostMessage, Is.EqualTo(string.Empty));
        }
    }
}
