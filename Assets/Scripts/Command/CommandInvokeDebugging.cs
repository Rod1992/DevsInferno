using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Zenject;
using System.Threading.Tasks;
using System.Threading;
using System;

public class CommandInvokeDebugging : ICommandInvoker
{
    /// <summary>
    /// commands to do
    /// </summary>
    List<ICommand> history;

    Task undoTask;
    CancellationToken cancelToken;
    CancellationTokenSource tokenSource;

    [Inject]
    public void Constructor(LogsManager logsManager)
    {
        tokenSource = new CancellationTokenSource();
        cancelToken = tokenSource.Token;

        history = logsManager.LoadLogs();
        ExecuteCommands();
    }

    public void ExecuteCommands()
    {
        undoTask = TaskDoHistory();
    }

    public string ExportLogReportCommands()
    {
        //You are not supposed to return anything
        throw new System.NotImplementedException();
    }

    async Task TaskDoHistory()
    {
        //NoAlloc
        ICommand command;
        float currentTs;
        float oldTs = 0f;

        if (history.Count <= 0)
        {
            //nothing to do
            return;
        }

        for (int i = 0; i < history.Count; i++)
        {
            command = history[i];
            currentTs = command.GetTimeStamp();

            if (cancelToken.IsCancellationRequested)
            {
                Debug.Log("Canceled Taks");
                cancelToken.ThrowIfCancellationRequested();
            }

            if (currentTs != oldTs)
            {
                int msToWait = (int)((currentTs - oldTs) * 1000);
                //we wait
                await Task.Delay(msToWait, cancelToken);
                oldTs = currentTs;
            }

            if ( command.CanExecute())
            {
                try
                {
                    command.Execute();
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

        Debug.LogWarning("FINISHED SIMULATION");
        Debug.Break();
    }


    public void UndoUntilTimestamp(float ts, float tsStart)
    {
        //you can't do rollback in Debug Mode
        throw new System.NotImplementedException();
    }

    public void Add(ICommand command)
    {
        //We don't add anything to our lists
    }

    public bool IsUndoing()
    {
        //we are always "undoing" or replaying
        return true;
    }

    public void Dispose()
    {
        tokenSource.Cancel();
    }
}
