using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Zenject;

public class CommandInvokeDebugging : ICommandInvoker
{
    [Inject]
    public void Constructor()
    {
        
    }

    public void ExecuteCommands()
    {
        
    }

    public string ExportLogReportCommands()
    {
        throw new System.NotImplementedException();
    }

    public void UndoUntilTimestamp(float ts, float tsStart)
    {
        throw new System.NotImplementedException();
    }

    public void Add(ICommand command)
    {

    }
}
