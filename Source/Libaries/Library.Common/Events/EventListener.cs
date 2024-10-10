using System;
using System.Text;

namespace Library.Common.Events;

/// <summary>
/// A listener class that defines what is being listened to, for and who is doing the listening
/// </summary>
public sealed class EventListener
{
  /// <summary>
  /// Constructor
  /// </summary>
  /// <param name="listener"></param>
  /// <param name="listenTo"></param>
  /// <param name="eventType"></param>
  /// <param name="callback"></param>
  public EventListener(object listener, object listenTo, Type eventType, EventCallback<RealmEventArgs> callback)
  {
    Listener = listener;
    ListenTo = listenTo;
    EventType = eventType;
    CallbackFunction = callback;
  }

  /// <summary>
  /// Who is the doing the listening
  /// </summary>
  public object Listener { get; }

  /// <summary>
  /// Who is being listened to
  /// </summary>
  public object ListenTo { get; }

  /// <summary>
  /// Type of event to listen for
  /// </summary>
  public Type EventType { get; }

  /// <summary>
  /// Function to call when the event is triggered
  /// </summary>
  public EventCallback<RealmEventArgs> CallbackFunction { get; }

  /// <summary>
  /// Override of the ToString function
  /// </summary>
  /// <returns></returns>
  public new string ToString()
  {
    StringBuilder sb = new();
    sb.Append($"Listener {Listener}, ");
    sb.Append($"ListenTo {ListenTo}, ");
    sb.Append($"EventType {EventType}, ");
    sb.Append($"CallbackFunction {CallbackFunction}");
    return sb.ToString();
  }
}