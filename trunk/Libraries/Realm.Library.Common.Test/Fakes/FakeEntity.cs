namespace Realm.Library.Common.Test.Fakes
{
    public class FakeEntity : Entity
    {
        public FakeEntity(long id, string name)
            : base(id, name)
        {
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;

            FakeEntity fake = obj as FakeEntity;
            return fake != null && fake.Name == Name;
        }

        public override int GetHashCode()
        {
            int hash = 13;
            hash = (hash * 7) + Name.GetHashCode();
            return hash;
        }

        public override string ToString()
        {
            return string.Format("{0}", Name);
        }
    }
}
