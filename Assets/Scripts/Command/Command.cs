public interface ICommand
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns> the timestamp this command was executed</returns>
    float GetTimeStamp();

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

    void Undo(float ts);

    /// <summary>
    /// Can we undo the command?, first it should have been executed
    /// </summary>
    /// <returns></returns>
    bool CanUndo();

    bool CanUndo(float ts);
}

public interface ICommandInvoker
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
    void UndoUntilTimestamp(float ts, float tsStart);

    /// <summary>
    /// 
    /// </summary>
    /// <returns>A log with all the executed commands up to this moment</returns>
    string ExportLogReportCommands();

    /// <summary>
    /// Add a Command to the queue
    /// </summary>
    /// <param name="command"></param>
    void Add(ICommand command);

    /// <summary>
    /// 
    /// </summary>
    /// <returns> if are we right now undoing commands</returns>
    bool IsUndoing();
}
