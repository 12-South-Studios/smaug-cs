namespace Library.Network.Mxp;

public class MxpEntity(string name, string value)
{
    public string Name { get; } = name;

    public string Value { get; } = value;

    public override string ToString() => $"!ELEMENT {Name} \"{Value}\"";
}