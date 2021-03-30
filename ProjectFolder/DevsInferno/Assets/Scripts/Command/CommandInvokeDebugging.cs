using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Zenject;

public class CommandInvokeDebugging : CommandInvoker
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

    public void UndoUntilTimestamp(int ts, int tsStart)
    {
        throw new System.NotImplementedException();
    }
}
