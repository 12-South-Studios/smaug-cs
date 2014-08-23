namespace Realm.Library.Common.Test.Fakes
{
    public class FakeContext : EntityContext<FakeEntity>
    {
        public FakeContext(FakeEntity parent)
            : base(parent)
        {
        }
    }
}
