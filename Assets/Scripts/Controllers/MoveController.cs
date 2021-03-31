using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UniRx;
using System;

public class MoveController
{
    PlayerController playerController;
    
    GameObject Model
    {
        get
        {
            return playerController.Model;
        }
    }

    Rigidbody PlayerRigidbody
    {
        get
        {
            return playerController.Rigidbody;
        }
    }

    [Inject]
    public void Constructor(PlayerController _playerController)
    {        
        playerController = _playerController;
        
        
    }

    public void MoveLeftOrRight( bool isLeft)
    {
        PlayerRigidbody.MovePosition(Model.transform.position + (Model.transform.right * Time.fixedDeltaTime * (isLeft ? -1 : 1)));
    }

    public void MoveForwardOrBackwards(bool isBackWards)
    {
        PlayerRigidbody.MovePosition(Model.transform.position + (Model.transform.forward * Time.fixedDeltaTime * (isBackWards ? -1 : 1)));
    }

    public void Rotate(bool isLeft)
    {
        Quaternion currentRotation = Model.transform.rotation;
        Vector3 euler = currentRotation.eulerAngles + new Vector3(0, 20 * Time.fixedDeltaTime * (isLeft ? -1 : 1), 0);
        PlayerRigidbody.MoveRotation(Quaternion.Euler(euler));
    }
}

//maybe useless to put as a flag
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

#region Commands
public class RotateCommand : ICommand
{
    float ts;

    bool isLeft;

    public RotateCommand(bool _isLeft)
    {
        isLeft = _isLeft;
        Game.Instance.AddCommand(this);
    }

    public bool CanExecute()
    {
        return Game.Instance.moveController != null;
    }

    public bool CanUndo()
    {
        return CanExecute();
    }

    public bool CanUndo(float ts)
    {
        throw new NotImplementedException();
    }

    public void Execute()
    {
        MoveController moveController = Game.Instance.moveController;
        ts = Time.time;
        moveController.Rotate(isLeft);
    }

    public string GetName()
    {
        return "Rotate " + (isLeft ? "Left" : "Right");
    }

    public float GetTimeStamp()
    {
        return ts;
    }

    public void Undo()
    {
        MoveController moveController = Game.Instance.moveController;

        moveController.Rotate(!isLeft);
    }

    public void Undo(float ts)
    {
        throw new NotImplementedException();
    }
}

public class MoveCommand : ICommand
{
    MoveType direction;
    float ts;

    public MoveCommand(MoveType moveType)
    {
        direction = moveType;
        Game.Instance.AddCommand(this);
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
        ts = Time.time;
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
#endregion
