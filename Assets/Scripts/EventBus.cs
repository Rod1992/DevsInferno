using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

/// <summary>
/// class for implementing the messaging Pattern, also called EventBus
/// </summary>
public class EventBus : MonoBehaviour
{
    static EventBus instance;
    ICommandInvoker commandInvoker;

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
    public static void Constructor(EventBus eventBus, ICommandInvoker _commandInvoker)
    {
        instance = eventBus;
        instance.commandInvoker = _commandInvoker;

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
