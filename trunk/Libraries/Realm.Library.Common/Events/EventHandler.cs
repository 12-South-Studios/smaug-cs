using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Management.Instrumentation;
using System.Reflection;
using System.Timers;
using Realm.Library.Common.Logging;
using Realm.Library.Common.Properties;

// ReSharper disable CheckNamespace
namespace Realm.Library.Common
// ReSharper restore CheckNamespace
{
    /// <summary>
    ///
    /// </summary>
    public class EventHandler : IEventHandler
    {
        private ITimer Timer { get; set; }
        private ILogWrapper _log { get; set; }

        // EventType => List of EventListeners (see EventListener.cs)
        private readonly ConcurrentDictionary<Type, IList<EventListener>> _events;

        // Queue of Events to be processed
        private readonly ConcurrentQueue<IEventBase> _eventQueue;

        ///  <summary>
        /// 
        ///  </summary>
        ///  <param name="timer"></param>
        ///  <param name="frequency"></param>
        /// <param name="logger"></param>
        public EventHandler(ITimer timer, int frequency, ILogWrapper logger)
        {
            Validation.IsNotNull(timer, "timer");
            Validation.Validate<ArgumentOutOfRangeException>(frequency >= 50, Resources.ERR_INVALID_FREQUENCY, frequency);
            Validation.IsNotNull(logger, "logger");

            _events = new ConcurrentDictionary<Type, IList<EventListener>>();
            _eventQueue = new ConcurrentQueue<IEventBase>();
            _log = logger;

            Timer = timer;
            Timer.Interval = frequency;
            Timer.Elapsed += TimerElapsed;

            _log.DebugFormat("Event Timer setup with Interval frequency of {0}", Timer.Interval);
        }

        /// <summary>
        /// Registers an object to listen to another object for a particular event
        /// and if that event is triggered to use a callback function
        /// </summary>
        public void RegisterListener(EventListener eventListener)
        {
            Validation.IsNotNull(eventListener, "eventLister");

            if (!_events.ContainsKey(eventListener.EventType))
                _events.TryAdd(eventListener.EventType, new List<EventListener> { eventListener });
            else
            {
                IList<EventListener> tupleList;
                _events.TryGetValue(eventListener.EventType, out tupleList);
                if (tupleList.IsNotNull())
                    tupleList.Add(eventListener);
            }
        }

        /// <summary>
        /// Is the object listening to the given event
        /// </summary>
        public bool IsListening(object listener, Type listenToEventType)
        {
            if (listener.IsNull()) return false;
            if (!_events.ContainsKey(listenToEventType)) return false;

            IList<EventListener> tupleList;
            _events.TryGetValue(listenToEventType, out tupleList);

            return tupleList.IsNotNull() && tupleList.Any(tuple => tuple.Listener == listener);
        }

        /// <summary>
        /// Is the object listening to the other object
        /// </summary>
        public bool IsListening(object listener, object listenTo, Type listenToEventType)
        {
            if (listener.IsNull() || listenTo.IsNull()) return false;
            if (!_events.ContainsKey(listenToEventType)) return false;

            IList<EventListener> tupleList;
            _events.TryGetValue(listenToEventType, out tupleList);

            return tupleList.IsNotNull() && tupleList.Any(tuple => tuple.Listener == listener && tuple.ListenTo == listenTo);
        }

        /// <summary>
        /// Removes an object that was being listened to by another object for a particular event
        /// </summary>
        public void StopListeningTo(object listener, object listenTo, Type listenToEventType)
        {
            if (!IsListening(listener, listenTo, listenToEventType)) return;

            IList<EventListener> tupleList;
            _events.TryGetValue(listenToEventType, out tupleList);
            if (tupleList.IsNull()) return;

            tupleList.Where(x => x.Listener == listener && x.ListenTo == listenTo && x.EventType == listenToEventType)
                     .ToList()
                     .ForEach(tuple => tupleList.Remove(tuple));
        }

        /// <summary>
        /// Removes an object that was listening to a particular event
        /// </summary>
        public void StopListening(object listener, Type listenToEventType)
        {
            if (!IsListening(listener, listenToEventType)) return;

            IList<EventListener> tupleList;
            _events.TryGetValue(listenToEventType, out tupleList);
            if (tupleList.IsNull()) return;

            tupleList.Where(x => x.Listener == listener && x.EventType == listenToEventType)
                     .ToList()
                     .ForEach(tuple => tupleList.Remove(tuple));
        }

        /// <summary>
        /// Removes an object that was listening to any event
        /// </summary>
        public void StopListening(object listener)
        {
            _events.Keys.ToList().ForEach(eventType =>
                                              {
                                                  IList<EventListener> tupleList;
                                                  _events.TryGetValue(eventType, out tupleList);
                                                  if (tupleList.IsNotNull())
                                                      tupleList.Where(x => x.Listener == listener)
                                                               .ToList()
                                                               .ForEach(tuple => tupleList.Remove(tuple));
                                              });
        }

        /// <summary>
        ///  Enqueues an event onto the event queue
        /// </summary>
        public void ThrowEvent(object sender, IEventBase thrownEvent)
        {
            if (sender.IsNull() || thrownEvent.IsNull()) return;
            if (!_events.ContainsKey(thrownEvent.GetType())) return;

            _log.DebugFormat("Locate Event to Throw {0}", thrownEvent.GetType());

            IList<EventListener> tupleList;
            _events.TryGetValue(thrownEvent.GetType(), out tupleList);
            if (tupleList.IsNull() || tupleList.Count == 0) return;

            _log.DebugFormat("ThrowEvent {0}/{1}", sender.GetType(), thrownEvent.GetType());

            thrownEvent.Sender = sender;
            _eventQueue.Enqueue(thrownEvent);

            if (Timer.IsNotNull() && !Timer.Enabled)
            {
                _log.DebugFormat("Starting the Event Timer with interval frequency of {0}", Timer.Interval);
                Timer.Start();
            }
        }

        /// <summary>
        /// Overload function for throwing an event
        /// </summary>
        public void ThrowEvent(object sender, IEventBase thrownEvent, RealmEventArgs args)
        {
            thrownEvent.Args = args;
            ThrowEvent(sender, thrownEvent);
        }

        /// <summary>
        /// Overload function for throwing an event
        /// </summary>
        public void ThrowEvent(object sender, IEventBase thrownEvent, EventTable table)
        {
            thrownEvent.Args = new RealmEventArgs(table);
            ThrowEvent(sender, thrownEvent);
        }

        /// <summary>
        /// Instantiates and throws an event of the given type
        /// </summary>
        public void ThrowEvent<T>(object sender, EventTable table) where T : EventBase
        {
            _log.DebugFormat("Create instance of Event to throw with Type {0}", typeof(T));

            EventBase evt;
            try
            {
                evt = Activator.CreateInstance<T>();
                if (evt.IsNull())
                    throw new InstanceNotFoundException(string.Format(Resources.ERR_EVENT_INSTANTIATE_FAILURE, typeof(T)));
            }
            catch
            {
                throw new InstanceNotFoundException(string.Format(Resources.ERR_EVENT_INSTANTIATE_FAILURE, typeof(T)));
            }

            evt.Args = new RealmEventArgs(table);
            ThrowEvent(sender, evt);
        }

        /// <summary>
        /// Instantiates and throws an event of the given type
        /// </summary>
        public void ThrowEvent<T>(object sender) where T : EventBase
        {
            _log.DebugFormat("Create instance of Event to throw with Type {0}", typeof(T));

            EventBase evt;
            try
            {
                evt = Activator.CreateInstance<T>();
                if (evt.IsNull())
                    throw new InstanceNotFoundException(string.Format(Resources.ERR_EVENT_INSTANTIATE_FAILURE, typeof(T)));
            }
            catch
            {
                throw new InstanceNotFoundException(string.Format(Resources.ERR_EVENT_INSTANTIATE_FAILURE, typeof(T)));
            }

            evt.Args = new RealmEventArgs();
            ThrowEvent(sender, evt);
        }

        /// <summary>
        /// Handles a timer event and processes objects listening to
        /// the given event and sender
        /// </summary>
        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            if (!_eventQueue.Any()) return;
            _log.DebugFormat("EventTimer found {0} events in the queue.", _eventQueue.Count);

            IEventBase thrownEvent;
            if (!_eventQueue.TryDequeue(out thrownEvent))
                throw new InvalidOperationException(Resources.ERR_EVENT_DEQUEUE_FAILURE);

            if (thrownEvent.IsNull()) return;
            _log.DebugFormat("EventHandler got Event {0} to process", thrownEvent.Name);

            if (!_events.ContainsKey(thrownEvent.GetType())) return;
            _log.DebugFormat("EventHandler got Event {0} and its in the event list.", thrownEvent.GetType());

            if (thrownEvent.Args.IsNull())
                thrownEvent.Args = new RealmEventArgs();
            thrownEvent.Args.Sender = thrownEvent.Sender;

            IList<EventListener> tupleList;
            _events.TryGetValue(thrownEvent.GetType(), out tupleList);
            if (tupleList.IsNull()) return;
            _log.DebugFormat("EventHandler got Event {0} and it has {1} listeners.", thrownEvent.GetType(),
                             tupleList.Count);

            try
            {
                tupleList.Where(x => (x.ListenTo == thrownEvent.Sender || x.ListenTo.IsNull())
                                     && x.Listener.IsNotNull())
                         .ToList()
                         .ForEach(tuple =>
                                      {
                                          _log.DebugFormat("Event {0} making a callback to listener {1}",
                                                           thrownEvent.GetType(), tuple.Listener.GetType());
                                          tuple.CallbackFunction.DynamicInvoke(thrownEvent.Args);
                                      });
            }
            catch (MemberAccessException ex)
            {
                ex.Handle(ExceptionHandlingOptions.RecordOnly, _log,
                          "Event Type {0}, EventSender Type {1}, TupleList {2}", thrownEvent.Name,
                          thrownEvent.Sender.GetType(), tupleList.ToString());
            }
            catch (ArgumentException ex)
            {
                ex.Handle(ExceptionHandlingOptions.RecordOnly, _log,
                          "Event Type {0}, EventSender Type {1}, TupleList {2}", thrownEvent.Name,
                          thrownEvent.Sender.GetType(), tupleList.ToString());
            }
            catch (TargetInvocationException ex)
            {
                ex.Handle(ExceptionHandlingOptions.RecordOnly, _log,
                          "Event Type {0}, EventSender Type {1}, TupleList {2}", thrownEvent.Name,
                          thrownEvent.Sender.GetType(), tupleList.ToString());
            }
        }
    }
}