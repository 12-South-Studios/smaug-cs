using Library.Common.Objects;

namespace SmaugCS.Data;

public class SocialData(long id, string name) : Entity(id, name)
{
    public string CharNoArg { get; set; }
    public string OthersNoArg { get; set; }
    public string CharFound { get; set; }
    public string OthersFound { get; set; }
    public string VictFound { get; set; }
    public string CharAuto { get; set; }
    public string OthersAuto { get; set; }
}