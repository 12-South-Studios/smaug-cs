using Realm.Library.Common.Attributes;
using Realm.Library.Common.Extensions;

namespace SmaugCS.Constants.Enums
{
    public static class AnsiCodeExtensions
    {
        public static string MakeBlink(this AnsiCodes code)
        {
            string name = code.GetName();
            if (name.Contains("[0;3"))
                return name.Replace("[0;3", "[0;5;3");
            return name.Contains("[1;3") ? name.Replace("[1;3", "[1;5;3") : name;
        }
    }

    public enum AnsiCodes
    {
        [Name("\033[0;30m")]
        ForegroundBlack = 0,

        [Name("\033[0;31m")]
        ForegroundDarkRed,

        [Name("\033[0;32m")]
        ForegroundDarkGreen,

        [Name("\033[0;33m")]
        ForegroundOrange,

        [Name("\033[0;34m")]
        ForegroundDarkBlue,

        [Name("\033[0;35m")]
        ForegroundPurple = 5,

        [Name("\033[0;36m")]
        ForegroundCyan,

        [Name("\033[0;37m")]
        ForegroundGrey,

        [Name("\033[1;30m")]
        ForegroundDarkGrey,

        [Name("\033[1;31m")]
        ForegroundRed,

        [Name("\033[1;32m")]
        ForegroundGreen = 10,

        [Name("\033[1;33m")]
        ForegroundYellow,

        [Name("\033[1;34m")]
        ForegroundBlue,

        [Name("\033[1;35m")]
        ForegroundPink,

        [Name("\033[1;36m")]
        ForegroundLightBlue,

        [Name("\033[1;37m")]
        ForegroundWhite = 15,

        [Name("\033[40m")]
        BackgroundBlack,

        [Name("\033[41m")]
        BackgroundDarkRed,

        [Name("\033[42m")]
        BackgroundDarkGreen,

        [Name("\033[43m")]
        BackgroundOrange,

        [Name("\033[44m")]
        BackgroundDarkBlue = 20,

        [Name("\033[45m")]
        BackgroundPurple,

        [Name("\033[46m")]
        BackgroundCyan,

        [Name("\033[47m")]
        BackgroundGrey,

        [Name("\033[50m")]
        BackgroundDarkGrey,

        [Name("\033[51m")]
        BackgroundRed = 25,

        [Name("\033[52m")]
        BackgroundGreen,

        [Name("\033[53m")]
        BackgroundYellow,

        [Name("\033[54m")]
        BackgroundBlue,

        [Name("\033[55m")]
        BackgroundPink,

        [Name("\033[56m")]
        BackgroundLightBlue = 30,

        [Name("\033[57m")]
        BackgroundWhite,

        [Name("\033[0m")]
        Reset,

        [Name("\033[1m")]
        Bold,

        [Name("\033[3m")]
        Italic,

        [Name("\033[4m")]
        Underline = 35,

        [Name("\033[5m")]
        Blink,

        [Name("\033[7m")]
        Reverse,

        [Name("\033[9m")]
        Strikeout
    }
}
