using System;

namespace Library.Common;

[Flags]
public enum ArticleAppendOptions
{
    None = 0,
    NewLineToEnd = 1,
    TheToFront = 2,
    CapitalizeFirstLetter = 4
}