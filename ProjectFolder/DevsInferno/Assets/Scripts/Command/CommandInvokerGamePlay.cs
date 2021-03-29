using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System.Linq;
using UniRx;
using System.Threading.Tasks;

public class CommandInvokerGamePlay : CommandInvoker
{
    /// <summary>
    /// commands to execute
    /// </summary>
    List<Command> commandsToInvoke;
    /// <summary>
    /// commands that we can undo
    /// </summary>
    List<Command> commandsInvoked;
    /// <summary>
    /// commands that will go to the logs
    /// </summary>
    List<Command> history;

    [Inject]
    public void Constructor()
    {
        commandsToInvoke = new List<Command>();
        commandsInvoked = new List<Command>();
        history = new List<Command>();
    }

    public void ExecuteCommands()
    {
        foreach (Command command in commandsToInvoke)
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

    public async void UndoUntilTimestamp(int tsToUndo, int tsStart)
    {
        //NoAlloc
        Command command;
        int currentTs;
        int oldTs = tsStart;

        //we order the commands using the ts
        commandsInvoked.Sort((x, y) => x.GetTimeStamp().CompareTo(y.GetTimeStamp()));

        if(commandsInvoked.Count <= 0)
        {
            //nothing to do
            return;
        }

        for (int i = commandsInvoked.Count - 1; i >= 0; i--)
        {
            command = commandsInvoked[i];
            currentTs = command.GetTimeStamp();

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


            //we wait , not correct, to finish
            await Task.Delay(oldTs - currentTs);

        }

        commandsInvoked.RemoveAll((x) => x.GetTimeStamp() >= tsToUndo);
    }

    public string ExportLogReportCommands()
    {
        //we aggreagate the names of all the commands
        return history.Select<Command, string>((command, s) => { return command.GetName(); }).Aggregate((x, y) => { return x + y; });
    }
}
