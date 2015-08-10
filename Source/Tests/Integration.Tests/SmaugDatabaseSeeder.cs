using Ninject;
using SmaugCS.DAL.Interfaces;

namespace Integration.Tests
{
    public static class SmaugDatabaseSeeder
    {
        public static IKernel Kernel;

        public static void Seed()
        {
            using (var context = Kernel.Get<ISmaugDbContext>())
            {
                // todo seed something
                context.SaveChanges();
            }

            // todo add something
        }
    }
}
