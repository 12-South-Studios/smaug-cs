namespace SmaugCS.Board.Tests;

public class BoardDataTests
{
    //private static DataTable BuildDataTable()
    //{
    //    DataTable table = new DataTable("BoardData");
    //    table.Columns.Add("BoardId");
    //    table.Columns.Add("ReadGroup");
    //    table.Columns.Add("PostGroup");
    //    table.Columns.Add("ExtraReaders");
    //    table.Columns.Add("ExtraRemovers");
    //    table.Columns.Add("OTakeMessage");
    //    table.Columns.Add("OPostMessage");
    //    table.Columns.Add("ORemoveMessage");
    //    table.Columns.Add("OCopyMessage");
    //    table.Columns.Add("PostMessage");
    //    table.Columns.Add("OReadMessage");
    //    table.Columns.Add("MinimumReadLevel");
    //    table.Columns.Add("MinimumPostLevel");
    //    table.Columns.Add("MinimumRemoveLevel");
    //    table.Columns.Add("MaximumPosts");
    //    table.Columns.Add("BoardTypeName");
    //    table.Columns.Add("OListMessage");
    //    return table;
    //}

    //[Fact]
    //public void Translate_NoNulls_Test()
    //{
    //    var table = BuildDataTable();

    //    var row = table.NewRow();
    //    row["BoardId"] = 1;
    //    row["ReadGroup"] = "GroupA";
    //    row["PostGroup"] = "GroupB";
    //    row["ExtraReaders"] = "Bob,John";
    //    row["ExtraRemovers"] = "Bill,Ted";
    //    row["OTakeMessage"] = "$N took a message.";
    //    row["OPostMessage"] = "$N posted a message.";
    //    row["ORemoveMessage"] = "$N removed a message.";
    //    row["OCopyMessage"] = "$N copied a message.";
    //    row["PostMessage"] = "What does this do?";
    //    row["OReadMessage"] = "$N read a message.";
    //    row["MinimumReadLevel"] = 1;
    //    row["MinimumPostLevel"] = 5;
    //    row["MinimumRemoveLevel"] = 10;
    //    row["MaximumPosts"] = 1000;
    //    row["BoardTypeName"] = "Note";
    //    row["OListMessage"] = "$N listed the messages.";

    //    var board = BoardData.Translate(row);

    //    board.Id.Should().Be(1));
    //    board.ReadGroup.Should().Be("GroupA"));
    //    board.PostGroup.Should().Be("GroupB"));
    //    board.ExtraReaders.Should().Be("Bob,John"));
    //    board.ExtraRemovers.Should().Be("Bill,Ted"));
    //    board.OTakeMessage.Should().Be("$N took a message."));
    //    board.OPostMessage.Should().Be("$N posted a message."));
    //    board.ORemoveMessage.Should().Be("$N removed a message."));
    //    board.OCopyMessage.Should().Be("$N copied a message."));
    //    board.PostMessage.Should().Be("What does this do?"));
    //    board.OReadMessage.Should().Be("$N read a message."));
    //    board.MinimumReadLevel.Should().Be(1));
    //    board.MinimumPostLevel.Should().Be(5));
    //    board.MinimumRemoveLevel.Should().Be(10));
    //    board.MaximumPosts.Should().Be(1000));
    //    board.Type.Should().Be(BoardTypes.Note));
    //    board.OListMessage.Should().Be("$N listed the messages."));
    //}

    //[Fact]
    //public void Translate_Nulls_Test()
    //{
    //    var table = BuildDataTable();

    //    var row = table.NewRow();
    //    row["BoardId"] = 1;
    //    row["MinimumReadLevel"] = 1;
    //    row["MinimumPostLevel"] = 5;
    //    row["MinimumRemoveLevel"] = 10;
    //    row["MaximumPosts"] = 1000;
    //    row["BoardTypeName"] = "Note";

    //    var board = BoardData.Translate(row);

    //    board.Id.Should().Be(1));
    //    board.OPostMessage.Should().Be(string.Empty));
    //}
}