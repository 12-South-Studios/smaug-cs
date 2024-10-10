using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using SmaugCS.DAL.Models;
using System;
using System.Threading.Tasks;

namespace SmaugCS.DAL;

public partial class DbContext
{
  public async Task<Character> GetCharacterAsync(long id)
  {
    try
    {
      FilterDefinition<Character> filter = Builders<Character>.Filter.Where(x => x.Id == id);
      IAsyncCursor<Character> results = await Characters.FindAsync(filter);
      return await results.FirstOrDefaultAsync();
    }
    catch (Exception ex)
    {
      _logger.LogError(ex.Message, ex);
      return null;
    }
  }

  public async Task<Character> GetCharacterAsync(string name)
  {
    try
    {
      FilterDefinition<Character> filter = Builders<Character>.Filter.Where(x => x.Name == name);
      IAsyncCursor<Character> results = await Characters.FindAsync(filter);
      return await results.FirstOrDefaultAsync();
    }
    catch (Exception ex)
    {
      _logger.LogError(ex.Message, ex);
      return null;
    }
  }

  public async Task<Character> AddOrUpdateCharacterAsync(Character character)
  {
    try
    {
      FilterDefinition<Character> filter = Builders<Character>.Filter.Where(x => x.Id == character.Id);
      IAsyncCursor<Character> results = await Characters.FindAsync(filter);

      if (!await results.AnyAsync())
      {
        await Characters.InsertOneAsync(character);
      }
      else
      {
        await Characters.ReplaceOneAsync(filter, character);
      }

      return character;
    }
    catch (Exception ex)
    {
      _logger.LogError(ex.Message, ex);
      return null;
    }
  }
}