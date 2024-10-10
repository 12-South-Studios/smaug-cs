namespace Library.Network.Mxp;

public static class MxpGlobals
{
    public static string MxpTag(string input, params object[] parameters) => input.MxpTag(parameters);
}