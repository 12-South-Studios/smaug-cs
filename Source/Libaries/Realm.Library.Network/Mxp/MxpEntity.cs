namespace Realm.Library.Network.Mxp
{
    public class MxpEntity
    {
        public string Name { get; }

        public MxpEntity(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public string Value { get; }

        public override string ToString() => $"!ELEMENT {Name} \"{Value}\"";
    }
}