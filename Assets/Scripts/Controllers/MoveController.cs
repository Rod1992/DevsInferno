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
    float ts;

    public MoveCommand(MoveType moveType)
    {
        direction = moveType;
        ts = Time.time;

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
        return CanExecute();
    }

    public bool CanUndo(float ts)
    {
        throw new System.NotImplementedException();
    }

    public void Execute()
    {
        ApplyMovement(false);
    }

    public string GetName()
    {
        return "Move" + direction.ToString();
    }

    public float GetTimeStamp()
    {
        return ts;
    }

    public void Undo()
    {
        ApplyMovement(true);
    }

    public void Undo(float ts)
    {
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// Apply a movement given a direcction
    /// </summary>
    /// <param name="isInverted">means if we are reversing the movement</param>
    public void ApplyMovement(bool isInverted)
    {
        MoveController moveController = Game.Instance.moveController;
        if (direction.HasFlag(MoveType.Forward))
            moveController.MoveForwardOrBackwards(isInverted);

        if (direction.HasFlag(MoveType.Backward))
            moveController.MoveForwardOrBackwards(!isInverted);

        if (direction.HasFlag(MoveType.Left))
            moveController.MoveLeftOrRight(!isInverted);

        if (direction.HasFlag(MoveType.Right))
            moveController.MoveLeftOrRight(isInverted);
    }
}
