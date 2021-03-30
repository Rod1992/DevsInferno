public interface Command
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns> the timestamp this command was executed</returns>
    int GetTimeStamp();

    /// <summary>
    /// Get the name of the command, ideally it would have the type and a guid
    /// </summary>
    /// <returns></returns>
    string GetName();

    /// <summary>
    /// Execute the command
    /// </summary>
    void Execute();

    /// <summary>
    /// can we execute this command?
    /// </summary>
    /// <returns>if it is possible to do this command</returns>
    bool CanExecute();

    /// <summary>
    /// Undo the command, first it should have been executed
    /// </summary>
    void Undo();

    void Undo(int ts);

    /// <summary>
    /// Can we undo the command?, first it should have been executed
    /// </summary>
    /// <returns></returns>
    bool CanUndo();

    bool CanUndo(int ts);
}

public interface CommandInvoker
{
    /// <summary>
    /// Initiaize the Invoker
    /// </summary>
    void Constructor();

    /// <summary>
    /// Execute all the commands
    /// </summary>
    void ExecuteCommands();

    /// <summary>
    /// Undo the commands until a certain timestamp 
    /// </summary>
    /// <param name="ts">timestamp</param>
    void UndoUntilTimestamp(int ts, int tsStart);

    /// <summary>
    /// 
    /// </summary>
    /// <returns>A log with all the executed commands up to this moment</returns>
    string ExportLogReportCommands();

    /// <summary>
    /// Add a Command to the queue
    /// </summary>
    /// <param name="command"></param>
    void Add(Command command);
}
