using Library.Common;
using Library.Common.Extensions;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Templates;
using System;

namespace SmaugCS.Repository;

public class RoomRepository : Patterns.Repository.Repository<long, RoomTemplate>, ITemplateRepository<RoomTemplate>
{
  private RoomTemplate LastRoom { get; set; }

  public RoomTemplate Create(long id, long cloneId, string name)
  {
    throw new NotImplementedException();
  }

  public RoomTemplate Create(long id, string name)
  {
    Validation.Validate(id >= 1);
    Validation.Validate(() =>
    {
      if (Contains(id))
        throw new DuplicateIndexException("Invalid ID {0}, Index already exists", id);
    });

    RoomTemplate newRoom = new RoomTemplate(id, name.IsNullOrEmpty() ? "Floating in a Void" : name);
    newRoom.Flags.SetBit(RoomFlags.Prototype);

    Add(id, newRoom);
    return newRoom;
  }
}