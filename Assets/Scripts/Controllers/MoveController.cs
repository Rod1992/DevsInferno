using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UniRx;
using System;

public class MoveController
{
    GameObject character;

    [Inject]
    public void Constructor()
    {
        character = GameObject.Instantiate(Resources.Load<GameObject>("Male_01_V01"), Vector3.zero, Quaternion.Euler(0,0,0));
    }

    public void MoveLeftOrRight( bool isLeft)
    {
        character.transform.position += character.transform.right * Time.deltaTime * (isLeft ? -1 : 1);
    }

    public void MoveForwardOrBackwards(bool isBackWards)
    {
        character.transform.position += character.transform.forward * Time.deltaTime * (isBackWards ? -1 : 1);
    }
}

[Flags]
public enum MoveType : short
{
    None = 0,
    Jump = 1,
    Forward = 2,
    Backward = 4,
    Left = 8,
    Right = 16,
}

public class MoveCommand : ICommand
{
    MoveType direction;

    public MoveCommand(MoveType moveType)
    {
        direction = moveType;

        Game.Instance.commandInvoker.Add(this);
    }

    public bool CanExecute()
    {
        return Game.Instance.moveController != null &&
            direction != MoveType.None &&
            !(direction.HasFlag(MoveType.Forward) && direction.HasFlag(MoveType.Backward)) &&
            !(direction.HasFlag(MoveType.Left) && direction.HasFlag(MoveType.Right));
    }

    public bool CanUndo()
    {
        throw new System.NotImplementedException();
    }

    public bool CanUndo(int ts)
    {
        throw new System.NotImplementedException();
    }

    public void Execute()
    {
        MoveController moveController = Game.Instance.moveController;
        if (direction.HasFlag(MoveType.Forward))
            moveController.MoveForwardOrBackwards(false);

        if (direction.HasFlag(MoveType.Backward))
            moveController.MoveForwardOrBackwards(true);

        if (direction.HasFlag(MoveType.Left))
            moveController.MoveLeftOrRight(true);

        if (direction.HasFlag(MoveType.Right))
            moveController.MoveLeftOrRight(false);
    }

    public string GetName()
    {
        return "Move" + direction.ToString();
    }

    public int GetTimeStamp()
    {
        throw new System.NotImplementedException();
    }

    public void Undo()
    {
        throw new System.NotImplementedException();
    }

    public void Undo(int ts)
    {
        throw new System.NotImplementedException();
    }
}
