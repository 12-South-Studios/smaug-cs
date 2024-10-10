using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;

namespace SmaugCS.Extensions.Player;

public static class Comm
{
  private static void SendFileToBuffer(this PlayerInstance ch, SystemFileTypes fileType)
  {
    // TODO
    //var path = SystemConstants.GetSystemFile(fileType);
    //using (var proxy = new TextReaderProxy(new StreamReader(path)))
    //{
    //    var buffer = proxy.ReadToEnd();
    //    ch.Descriptor.WriteToBuffer(buffer, buffer.Length);
    //}
  }

  public static void SendRIPScreen(this PlayerInstance ch)
  {
    SendFileToBuffer(ch, SystemFileTypes.RIPScreen);
  }

  public static void SendRIPTitle(this PlayerInstance ch)
  {
    SendFileToBuffer(ch, SystemFileTypes.RIPTitle);
  }

  public static void SendANSITitle(this PlayerInstance ch)
  {
    SendFileToBuffer(ch, SystemFileTypes.ANSITitle);
  }

  public static void SendASCIITitle(this PlayerInstance ch)
  {
    SendFileToBuffer(ch, SystemFileTypes.ASCTitle);
  }
}