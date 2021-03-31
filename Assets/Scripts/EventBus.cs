using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

/// <summary>
/// class for implementing the messaging Pattern, also called EventBus
/// </summary>
public class EventBus
{
    static EventBus instance;

    //the events we have
    private Dictionary<string, UnityEvent> eventDictionary;

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

        instance.eventDictionary = new Dictionary<string,UnityEvent>();
    }

    public static void StartListening(string eventName, UnityAction listener)
    {
        UnityEvent thisEvent = null;
        if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new UnityEvent();
            thisEvent.AddListener(listener);
            Instance.eventDictionary.Add(eventName, thisEvent);
        }
    }

    public static void StopListening(string eventName, UnityAction listener)
    {
        UnityEvent thisEvent = null;
        if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
     }

    public static void TriggerEvent(string eventName)
    {
        UnityEvent thisEvent = null;
        if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke();
        }
    }


}
