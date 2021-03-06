﻿using UnityEngine;
using System.Collections;
using InControl;

public class PlayerActions : PlayerActionSet
{
    public PlayerAction Left;
    public PlayerAction Right;
    public PlayerAction Up;
    public PlayerAction Down;
    public PlayerAction Jump;
    public PlayerAction Wave1;
    public PlayerAction Wave2;
    public PlayerAction Wave3;
    public PlayerAction Start;
    public PlayerTwoAxisAction Move;

    public PlayerActions()
    {
        Jump = CreatePlayerAction("Jump");
        Wave1 = CreatePlayerAction("Wave1");
        Wave2 = CreatePlayerAction("Wave2");
        Wave3 = CreatePlayerAction("Wave3");
        Start = CreatePlayerAction("Start");
        Left = CreatePlayerAction("Move Left");
        Right = CreatePlayerAction("Move Right");
        Up = CreatePlayerAction("Move Up");
        Down = CreatePlayerAction("Move Down");
        Move = CreateTwoAxisPlayerAction(Left, Right, Down, Up);
    }

    public static PlayerActions CreateWithDebugBindings()
    {
        var playerActions = new PlayerActions();

        playerActions.Start.AddDefaultBinding(InputControlType.Start);
        playerActions.Start.AddDefaultBinding(Key.Escape);

        playerActions.Jump.AddDefaultBinding(Key.Space);
        playerActions.Jump.AddDefaultBinding(InputControlType.Action1);
        playerActions.Jump.AddDefaultBinding(InputControlType.LeftStickUp);
        playerActions.Jump.AddDefaultBinding(InputControlType.DPadUp);

        playerActions.Wave1.AddDefaultBinding(InputControlType.Action2);
        playerActions.Wave1.AddDefaultBinding(Key.Key1);

        playerActions.Wave2.AddDefaultBinding(InputControlType.Action3);
        playerActions.Wave2.AddDefaultBinding(Key.Key2);

        playerActions.Wave3.AddDefaultBinding(InputControlType.Action4);
        playerActions.Wave3.AddDefaultBinding(Key.Key3);

        playerActions.Up.AddDefaultBinding(Key.UpArrow);
        playerActions.Down.AddDefaultBinding(Key.DownArrow);
        playerActions.Left.AddDefaultBinding(Key.LeftArrow);
        playerActions.Right.AddDefaultBinding(Key.RightArrow);

        playerActions.Up.AddDefaultBinding(Key.W);
        playerActions.Down.AddDefaultBinding(Key.S);
        playerActions.Left.AddDefaultBinding(Key.A);
        playerActions.Right.AddDefaultBinding(Key.D);

        playerActions.Left.AddBinding(new DeviceBindingSource());

        playerActions.Left.AddDefaultBinding(InputControlType.LeftStickLeft);
        playerActions.Right.AddDefaultBinding(InputControlType.LeftStickRight);
        playerActions.Up.AddDefaultBinding(InputControlType.LeftStickUp);
        playerActions.Down.AddDefaultBinding(InputControlType.LeftStickDown);

        playerActions.Left.AddDefaultBinding(InputControlType.DPadLeft);
        playerActions.Right.AddDefaultBinding(InputControlType.DPadRight);
        playerActions.Up.AddDefaultBinding(InputControlType.DPadUp);
        playerActions.Down.AddDefaultBinding(InputControlType.DPadDown);

        return playerActions;
    }

    public static PlayerActions CreateControllerBindings()
    {
        var playerActions = new PlayerActions();

        playerActions.Start.AddDefaultBinding(InputControlType.Start);

        playerActions.Jump.AddDefaultBinding(InputControlType.Action1);
        playerActions.Jump.AddDefaultBinding(InputControlType.DPadUp);

        playerActions.Wave1.AddDefaultBinding(InputControlType.Action2);

        playerActions.Wave2.AddDefaultBinding(InputControlType.Action3);

        playerActions.Wave3.AddDefaultBinding(InputControlType.Action4);

        playerActions.Left.AddDefaultBinding(InputControlType.LeftStickLeft);
        playerActions.Right.AddDefaultBinding(InputControlType.LeftStickRight);
        playerActions.Up.AddDefaultBinding(InputControlType.LeftStickUp);
        playerActions.Down.AddDefaultBinding(InputControlType.LeftStickDown);

        playerActions.Left.AddDefaultBinding(InputControlType.DPadLeft);
        playerActions.Right.AddDefaultBinding(InputControlType.DPadRight);
        playerActions.Up.AddDefaultBinding(InputControlType.DPadUp);
        playerActions.Down.AddDefaultBinding(InputControlType.DPadDown);

        return playerActions;
    }

    public static PlayerActions CreateWithPlayer1KeyboardBindings()
    {
        var playerActions = new PlayerActions();

        playerActions.Start.AddDefaultBinding(Key.Q);

        playerActions.Jump.AddDefaultBinding(Key.W);

        playerActions.Wave1.AddDefaultBinding(Key.Z);

        playerActions.Wave2.AddDefaultBinding(Key.X);

        playerActions.Wave3.AddDefaultBinding(Key.C);

        playerActions.Up.AddDefaultBinding(Key.W);
        playerActions.Down.AddDefaultBinding(Key.S);
        playerActions.Left.AddDefaultBinding(Key.A);
        playerActions.Right.AddDefaultBinding(Key.D);

        return playerActions;
    }

    public static PlayerActions CreateWithPlayer2KeyboardBindings()
    {
        var playerActions = new PlayerActions();

        playerActions.Start.AddDefaultBinding(Key.U);

        playerActions.Jump.AddDefaultBinding(Key.I);

        playerActions.Wave1.AddDefaultBinding(Key.M);

        playerActions.Wave2.AddDefaultBinding(Key.Comma);

        playerActions.Wave3.AddDefaultBinding(Key.Period);

        playerActions.Up.AddDefaultBinding(Key.I);
        playerActions.Down.AddDefaultBinding(Key.K);
        playerActions.Left.AddDefaultBinding(Key.J);
        playerActions.Right.AddDefaultBinding(Key.L);

        return playerActions;
    }
}
