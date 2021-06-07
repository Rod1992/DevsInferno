using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

#region Messages
public enum EventMessage
{
    None = 0,
    StartRollBackTime = 1,
    EndRollBackTime = 2,
    Pause = 3,
    Unpause = 4,
}
#endregion

/// <summary>
/// class for implementing the messaging Pattern, also called EventBus
/// </summary>
public class EventBus
{
    static EventBus instance;

    //the events we have
    private Dictionary<EventMessage, UnityEvent<object>> eventDictionary;

    public static EventBus Instance
    {
        get
        {
            return instance;
        }
    }

    [Inject]
    public void Constructor(EventBus eventBus)
    {
        instance = eventBus;

        eventDictionary = new Dictionary<EventMessage, UnityEvent<object>>();
    }

    public static void StartListening(EventMessage eventName, UnityAction<object> listener)
    {
        UnityEvent<object> thisEvent = null;
        if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new UnityEvent<object>();
            thisEvent.AddListener(listener);
            Instance.eventDictionary.Add(eventName, thisEvent);
        }
    }

    public static void StopListening(EventMessage eventName, UnityAction<object> listener)
    {
        UnityEvent<object> thisEvent = null;
        if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
     }

    public static void TriggerEvent(EventMessage eventName , object parameters)
    {
        UnityEvent<object> thisEvent = null;
        if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke(parameters);
        }
    }


}
