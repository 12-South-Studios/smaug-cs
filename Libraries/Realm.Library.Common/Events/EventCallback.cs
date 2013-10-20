using System;

// ReSharper disable CheckNamespace
namespace Realm.Library.Common
// ReSharper restore CheckNamespace
{
    /// <summary>
    /// Format of an event callback
    /// </summary>
    /// <param name="args"></param>
    public delegate void EventCallback<in T>(T args) where T : EventArgs;
}