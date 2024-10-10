using SmaugCS.DAL;
using SmaugCS.Logging;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using SmaugCS.DAL.Models;

namespace SmaugCS.Board;

public class BoardRepository(ILogManager logManager, IDbContext dbContext) : IBoardRepository
{
  public IEnumerable<BoardData> Boards { get; } = new List<BoardData>();

  public void Add(BoardData board) => Boards.ToList().Add(board);

  public void Load()
  {
    try
    {
      if (dbContext.Count<DAL.Models.Board>() == 0) return;

      foreach (DAL.Models.Board board in dbContext.GetAll<DAL.Models.Board>())
      {
        BoardData newBoard = new(board.Id, board.BoardType)
        {
          Name = board.Name,
          ReadGroup = board.ReadGroup,
          PostGroup = board.PostGroup,
          ExtraReaders = board.ExtraReaders,
          ExtraRemovers = board.ExtraRemovers,
          OTakeMessage = board.OTakeMessage,
          OPostMessage = board.OTakeMessage,
          ORemoveMessage = board.ORemoveMessage,
          OCopyMessage = board.OCopyMessage,
          OListMessage = board.OListMessage,
          PostMessage = board.PostMessage,
          OReadMessage = board.OReadMessage,
          MinimumReadLevel = board.MinimumReadLevel,
          MinimumPostLevel = board.MinimumPostLevel,
          MinimumRemoveLevel = board.MinimumRemoveLevel,
          MaximumPosts = board.MaximumPosts,
          BoardObjectId = board.BoardObjectId
        };
        Boards.ToList().Add(newBoard);

        foreach (NoteData newNote in board.Notes.Select(note => new NoteData(note.Id)
                 {
                   Sender = note.Sender,
                   DateSent = note.DateSent,
                   RecipientList = note.RecipientList,
                   Subject = note.Subject,
                   IsPoll = note.IsPoll,
                   Text = note.Text
                 }))
        {
          newBoard.Notes.ToList().Add(newNote);
        }
      }

      logManager.Boot("Loaded {0} Boards", Boards.Count());
    }
    catch (DbException ex)
    {
      logManager.Error(ex);
      throw;
    }
  }

  public void Save()
  {
    try
    {
      foreach (BoardData board in Boards.Where(x => !x.Saved).ToList())
      {
        DAL.Models.Board boardToSave = new()
        {
          Name = board.Name,
          ReadGroup = board.ReadGroup,
          PostGroup = board.PostGroup,
          ExtraReaders = board.ExtraReaders,
          ExtraRemovers = board.ExtraRemovers,
          OTakeMessage = board.OTakeMessage,
          OPostMessage = board.OPostMessage,
          ORemoveMessage = board.ORemoveMessage,
          OCopyMessage = board.OCopyMessage,
          OListMessage = board.OListMessage,
          PostMessage = board.PostMessage,
          OReadMessage = board.ORemoveMessage,
          MinimumReadLevel = board.MinimumReadLevel,
          MinimumPostLevel = board.MinimumPostLevel,
          MinimumRemoveLevel = board.MinimumRemoveLevel,
          MaximumPosts = board.MaximumPosts,
          BoardObjectId = board.BoardObjectId
        };
        board.Saved = true;

        foreach (NoteData note in board.Notes.Where(y => !y.Saved).ToList())
        {
          Note noteToSave = new()
          {
            DateSent = note.DateSent,
            IsPoll = note.IsPoll,
            RecipientList = note.RecipientList,
            Sender = note.Sender,
            Subject = note.Subject,
            Text = note.Text
          };
          note.Saved = true;
          boardToSave.Notes.Add(noteToSave);
        }

        dbContext.AddOrUpdate(boardToSave);
      }
    }
    catch (DbException ex)
    {
      logManager.Error(ex);
      throw;
    }
  }
}