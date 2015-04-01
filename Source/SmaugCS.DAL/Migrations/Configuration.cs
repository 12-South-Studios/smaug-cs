using System;
using System.Data.Entity.Migrations;

namespace SmaugCS.DAL.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<SmaugDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(SmaugDbContext context)
        {
            try
            {

            }
            catch (Exception ex)
            {
                
            }
        }

    }
}