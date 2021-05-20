using System;

namespace Realm.Library.Common.Events

{
    /// <summary>
    /// Format of an event callback
    /// </summary>
    /// <param name="args"></param>
    public delegate void EventCallback<in T>(T args) where T : EventArgs;
}