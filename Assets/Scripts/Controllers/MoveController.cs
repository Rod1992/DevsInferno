using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UniRx;
using UniRx.Triggers;
using System;

public class MoveController
{
    public const float SPEED = 0.01f;
    public const float SPEEDROTATION = 0.2f;

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

        Observable.EveryFixedUpdate().Where(x=> !playerController.HitFloor).Subscribe(x => { new GravityCommand(); });
    }

    public void MoveUpwardsOrDownWard(bool isDown)
    {
        playerController.MoveInDir(Model.transform.up * SPEED * 20 * (isDown ? -1 : 1));
    }

    public void MoveLeftOrRight( bool isLeft)
    {
        playerController.MoveInDir(Model.transform.right *  SPEED * (isLeft ? -1 : 1));
    }

    public void MoveForwardOrBackwards(bool isBackWards)
    {
        playerController.MoveInDir((Model.transform.forward * SPEED * (isBackWards ? -1 : 1)));
    }

    public void Rotate(bool isLeft)
    {
        Quaternion currentRotation = Model.transform.rotation;
        Vector3 euler = currentRotation.eulerAngles + new Vector3(0, SPEEDROTATION * (isLeft ? -1 : 1), 0);
        PlayerRigidbody.MoveRotation(Quaternion.Euler(euler));
    }

    public void ApplyGravity(bool reverse = false)
    {
        playerController.MoveInDir((Physics.gravity * SPEED * (reverse ? -1 : 1)));
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
    int index = -1;

    bool isLeft;

    public RotateCommand(bool _isLeft)
    {
        isLeft = _isLeft;
        Game.Instance.AddCommand(this);
    }

    public RotateCommand(float _ts, params string[] _isLeft)
    {
        ts = _ts;
        isLeft = bool.Parse(_isLeft.GetValue(0).ToString());
    }

    public bool CanExecute()
    {
        return Game.Instance.MoveControl != null;
    }

    public bool CanUndo()
    {
        return CanExecute();
    }

    public void Execute()
    {
        MoveController moveController = Game.Instance.MoveControl;
#if !DEBUGMODE
        ts = Time.time;
#endif
        moveController.Rotate(isLeft);
    }

    public int GetIndexOrder()
    {
        return index;
    }

    public string GetName()
    {
        return this.GetType().Name +
            "$" + ts.ToString() +
            "$" + isLeft.ToString();
    }

    public float GetTimeStamp()
    {
        return ts;
    }

    public void SetIndexOrder(int _index)
    {
        index = _index;
    }

    public void Undo()
    {
        MoveController moveController = Game.Instance.MoveControl;

        moveController.Rotate(!isLeft);
    }
}

public class MoveCommand : ICommand
{
    MoveType direction;
    float ts;
    int index = -1;

    public MoveCommand(MoveType moveType)
    {
        direction = moveType;
        Game.Instance.AddCommand(this);
    }

    public MoveCommand(float _ts, params string[] moveType)
    {
        ts = _ts;
        direction = (MoveType) Enum.Parse( typeof(MoveType), moveType.GetValue(0).ToString());
    }

    public bool CanExecute()
    {
        return Game.Instance.MoveControl != null &&
            direction != MoveType.None &&
            !(direction.HasFlag(MoveType.Forward) && direction.HasFlag(MoveType.Backward)) &&
            !(direction.HasFlag(MoveType.Left) && direction.HasFlag(MoveType.Right));
    }

    public bool CanUndo()
    {
        return CanExecute();
    }

    public void Execute()
    {
#if !DEBUGMODE
        ts = Time.time;
#endif
        ApplyMovement(false);
    }

    public string GetName()
    {
        return this.GetType().Name +
            "$" + ts.ToString() +
            "$" + direction.ToString();
    }

    public float GetTimeStamp()
    {
        return ts;
    }

    public void Undo()
    {
        ApplyMovement(true);
    }

    /// <summary>
    /// Apply a movement given a direcction
    /// </summary>
    /// <param name="isInverted">means if we are reversing the movement</param>
    public void ApplyMovement(bool isInverted)
    {
        MoveController moveController = Game.Instance.MoveControl;
        if (direction.HasFlag(MoveType.Forward))
            moveController.MoveForwardOrBackwards(isInverted);

        if (direction.HasFlag(MoveType.Backward))
            moveController.MoveForwardOrBackwards(!isInverted);

        if (direction.HasFlag(MoveType.Left))
            moveController.MoveLeftOrRight(!isInverted);

        if (direction.HasFlag(MoveType.Right))
            moveController.MoveLeftOrRight(isInverted);
    }

    public int GetIndexOrder()
    {
        return index;
    }

    public void SetIndexOrder(int _index)
    {
        index = _index;
    }
}

public class GravityCommand : ICommand
{
    float ts;
    int indexOrder;

    public GravityCommand()
    {
        Game.Instance.AddCommand(this);
    }

    public GravityCommand(float _ts)
    {
        ts = _ts;
    }

    public bool CanExecute()
    {
        return Game.Instance.MoveControl != null;
    }

    public bool CanUndo()
    {
        return Game.Instance.MoveControl != null;
    }

    public void Execute()
    {
#if !DEBUGMODE
        ts = Time.time;
#endif
        Game.Instance.MoveControl.ApplyGravity();
    }

    public int GetIndexOrder()
    {
        return indexOrder;
    }

    public string GetName()
    {
        return this.GetType().Name +
            "$" + ts.ToString();
    }

    public float GetTimeStamp()
    {
        return ts;
    }

    public void SetIndexOrder(int index)
    {
        indexOrder = index;
    }

    public void Undo()
    {
        Game.Instance.MoveControl.ApplyGravity(true);
    }
}

public class JumpCommand : ICommand
{
    float secondsJump;
    float startTs;
    float executionTs;
    int index;

    public JumpCommand(float _secondsJump)
    {
        secondsJump = _secondsJump;
        startTs = Time.time;
        Game.Instance.AddCommand(this);
    }

    public JumpCommand(float _ts, params string[] _secondsJump)
    {
        secondsJump = float.Parse(_secondsJump.GetValue(0).ToString());
        executionTs = _ts;
    }

    public bool CanExecute()
    {
        return Game.Instance.MoveControl != null;
    }

    public bool CanUndo()
    {
        return CanExecute();
    }

    public void Execute()
    {
#if !DEBUGMODE
        executionTs = Time.time;
#endif

        Game.Instance.MoveControl.MoveUpwardsOrDownWard(false);
#if !DEBUGMODE
        float newTime = secondsJump - (executionTs - startTs);

        if(newTime > 0f)
        new JumpCommand(newTime);
#endif
    }

    public int GetIndexOrder()
    {
        return index;
    }

    public string GetName()
    {
        return this.GetType().Name +
            "$" + executionTs.ToString() +
            "$" + secondsJump.ToString();
    }

    public float GetTimeStamp()
    {
        return executionTs;
    }

    public void SetIndexOrder(int _index)
    {
        index = _index;
    }

    public void Undo()
    {
        Game.Instance.MoveControl.MoveUpwardsOrDownWard(true);
    }
}
#endregion
