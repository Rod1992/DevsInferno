using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System.Linq;
using UniRx;
using System.Threading.Tasks;

public class CommandInvokerGamePlay : ICommandInvoker
{
    /// <summary>
    /// commands to execute
    /// </summary>
    List<ICommand> commandsToInvoke;
    /// <summary>
    /// commands that we can undo
    /// </summary>
    List<ICommand> commandsInvoked;
    /// <summary>
    /// commands that will go to the logs
    /// </summary>
    List<ICommand> history;
    /// <summary>
    /// are we undoing commands
    /// </summary>
    bool isUndoing;
    int indexCommand = 0; 

    [Inject]
    public void Constructor()
    {
        commandsToInvoke = new List<ICommand>();
        commandsInvoked = new List<ICommand>();
        history = new List<ICommand>();
        isUndoing = false;
    }

    public void Add(ICommand command)
    {
        commandsToInvoke.Add(command);
    }

    public void ExecuteCommands()
    {
        foreach (ICommand command in commandsToInvoke)
        {
            if (command.CanExecute())
            {
                try
                {
                    command.Execute();
                }
                catch(Exception e)
                {
                    Debug.LogError("There was a problem with the command " + command.GetName() + " The error was "+ e.Message);
                }
                command.SetIndexOrder(indexCommand++);
                commandsInvoked.Add(command);
                history.Add(command);
            }
            else
            {
                Debug.LogError("Couldn't execute the command" + command.GetName());
            }
        }

        commandsToInvoke.Clear();
    }

    public async void UndoUntilTimestamp(float tsToUndo, float tsStart)
    {
        //NoAlloc
        ICommand command;
        float currentTs;
        float oldTs = tsStart;
        isUndoing = true;

        if (commandsInvoked.Count <= 0)
        {
            //nothing to do
            isUndoing = false;
            return;
        }
        EventBus.TriggerEvent(EventMessage.StartRollBackTime, null);

        //we order the commands using the ts
        commandsInvoked.Sort((x, y) => x.GetIndexOrder().CompareTo(y.GetIndexOrder()));

        for (int i = commandsInvoked.Count - 1; i >= 0; i--)
        {
            command = commandsInvoked[i];
            currentTs = command.GetTimeStamp();
            
            if (currentTs != oldTs) {
                
                //we wait
                await Task.Delay((int)((oldTs - currentTs ) * 1000));
                oldTs = currentTs;
            }

            if (currentTs >= tsToUndo && command.CanUndo())
            {
                try
                {
                    command.Undo();
                }
                catch (Exception e)
                {
                    Debug.LogError("There was a problem with the command " + command.GetName() + " The error was " + e.Message);
                }
             }
            else if(currentTs < tsToUndo && command.CanUndo(tsToUndo))
            {
                //it's a command that started before our ts but that has executed during 
                try
                {
                    command.Undo(tsToUndo);
                }
                catch (Exception e)
                {
                    Debug.LogError("There was a problem with the command " + command.GetName() + " The error was " + e.Message);
                }
            }
            else
            {
                Debug.LogError("Couldn't undo the command" + command.GetName());
            }

        }

        commandsInvoked.RemoveAll((x) => x.GetTimeStamp() >= tsToUndo);
        isUndoing = false;
        EventBus.TriggerEvent(EventMessage.EndRollBackTime, null);
    }

    public string ExportLogReportCommands()
    {
        //we aggreagate the names of all the commands
        return history.Select<ICommand, string>((command, s) => { return command.GetName(); }).Aggregate((x, y) => { return x + y; });
    }

    public bool IsUndoing()
    {
        return isUndoing;
    }
}
