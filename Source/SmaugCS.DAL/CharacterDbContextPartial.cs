using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using SmaugCS.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmaugCS.DAL
{
    public partial class DbContext
    {
        public async Task<Character> GetCharacterAsync(long id)
        {
            try
            {
                var filter = Builders<Character>.Filter.Where(x => x.Id == id);
                var results = await Characters.FindAsync(filter);
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
                var filter = Builders<Character>.Filter.Where(x => x.Name == name);
                var results = await Characters.FindAsync(filter);
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
                var filter = Builders<Character>.Filter.Where(x => x.Id == character.Id);
                var results = await Characters.FindAsync(filter);

                if (!results.Any())
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
}
