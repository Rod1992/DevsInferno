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
    EndRollBackTime = 2
}
#endregion

/// <summary>
/// class for implementing the messaging Pattern, also called EventBus
/// </summary>
public class EventBus
{
    static EventBus instance;

    //the events we have
    private Dictionary<EventMessage, UnityEvent<ParamArrayAttribute>> eventDictionary;

    public static EventBus Instance
    {
        get
        {
            return instance;
        }
    }

    [Inject]
    public static void Constructor(EventBus eventBus)
    {
        instance = eventBus;

        instance.eventDictionary = new Dictionary<EventMessage, UnityEvent<ParamArrayAttribute>>();
    }

    public static void StartListening(EventMessage eventName, UnityAction<ParamArrayAttribute> listener)
    {
        UnityEvent<ParamArrayAttribute> thisEvent = null;
        if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new UnityEvent<ParamArrayAttribute>();
            thisEvent.AddListener(listener);
            Instance.eventDictionary.Add(eventName, thisEvent);
        }
    }

    public static void StopListening(EventMessage eventName, UnityAction<ParamArrayAttribute> listener)
    {
        UnityEvent<ParamArrayAttribute> thisEvent = null;
        if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
     }

    public static void TriggerEvent(EventMessage eventName , ParamArrayAttribute parameters)
    {
        UnityEvent<ParamArrayAttribute> thisEvent = null;
        if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke(parameters);
        }
    }


}
