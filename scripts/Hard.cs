using Godot;
using System;

public class Hard : Node
{
    enum Sides{
        Left,
        Right
    }

    Sides sides;
    bool left_isLeft = true;
    bool right_isLeft = true;

    bool left_canSwitch = true;
    bool right_canSwitch = true;

    PackedScene ballScene = (PackedScene) GD.Load("res://scenes/Ball.tscn");

    Sprite leftLever;
    Sprite rightLever;

    Ball ball;
    Path2D mainPathLeft;
    Path2D mainPathRight;

    Timer leftTimer;
    Timer rightTimer;

    public override void _Ready()
    {
        leftLever = GetNode<Sprite>("LeverLeft");
        rightLever = GetNode<Sprite>("LeverRight");

        mainPathLeft = GetNode<Path2D>("MainPathLeft");
        mainPathRight = GetNode<Path2D>("MainPathRight");

        leftTimer = GetNode<Timer>("LeftTimer");
        rightTimer = GetNode<Timer>("RightTimer");

        GD.Randomize();
        int rand = (int) GD.RandRange(0,10);
        if(rand < 4){
            SpawnBall(Sides.Left);
        } else {
            SpawnBall(Sides.Right);
        }

    }

    public override void _Process(float delta){
        SetAndCheckLeftLever();
        SetAndCheckRightLever();
    }

    void SpawnBall(Sides _side){
        ball = (Ball) ballScene.Instance();
        sides = _side;
        if(sides == Sides.Left){
            mainPathLeft.AddChild(ball);
            ball.Connect("endMainPathLeft", this, nameof(_on_Ball_endMainPathLeft));
        } else {
            mainPathRight.AddChild(ball);
            ball.Connect("endMainPathRight", this, nameof(_on_Ball_endMainPathRight));
        }
        ball.Connect("reachEnd", this, nameof(_on_Ball_reachEnd));
        ball.Offset = 0;      
    }

    void SetAndCheckLeftLever(){
        mainPathLeft.Curve.SetPointPosition(2, leftLever.GetNode<Position2D>("Position2D").GlobalPosition);

        if(leftLever.RotationDegrees >= 40){
            left_isLeft = true;
        } else if(leftLever.RotationDegrees <= 0){
            left_isLeft = false;
        }
    }

    void SetAndCheckRightLever(){
        mainPathRight.Curve.SetPointPosition(2, rightLever.GetNode<Position2D>("Position2D").GlobalPosition);

        if(rightLever.RotationDegrees >= 40){
            right_isLeft = true;
        } else if(rightLever.RotationDegrees <= -40){
            right_isLeft = false;
        }
    }

    void _on_Ball_endMainPathLeft(Ball _ball){
        if(left_isLeft && left_canSwitch){
            mainPathLeft.RemoveChild(_ball);
            GetNode<Path2D>("LeftPath").AddChild(_ball);
            _ball.Offset = 0;
        } else if(!left_isLeft && left_canSwitch){
            mainPathLeft.RemoveChild(_ball);
            GetNode<Path2D>("MiddlePath1").AddChild(_ball);
            _ball.Offset = 0;
        }
    }

    void _on_Ball_endMainPathRight(Ball _ball){
        if(right_isLeft && right_canSwitch){
            mainPathRight.RemoveChild(_ball);
            GetNode<Path2D>("MiddlePath2").AddChild(_ball);
            _ball.Offset = 0;
        } else if(!right_isLeft && right_canSwitch){
            mainPathRight.RemoveChild(_ball);
            GetNode<Path2D>("RightPath").AddChild(_ball);
            _ball.Offset = 0;
        }

    }

    void _on_Ball_reachEnd(string side, Ball.Colors colors, Ball _ball, string newPath){

        if(side == "Left"){
            GetNode<Path>("LeftPath").CheckColor("Hard", "Left", colors, newPath, _ball.Modulate);
        } else if(side == "Middle"){
            if(_ball.GetParent().Name == "MiddlePath1"){
                GetNode<Path>("MiddlePath1").CheckColor("Hard", "Middle", colors, newPath, _ball.Modulate);
            } else if(_ball.GetParent().Name == "MiddlePath2"){
                GetNode<Path>("MiddlePath2").CheckColor("Hard", "Middle", colors, newPath, _ball.Modulate);
            }
        } else if(side == "Right"){
            GetNode<Path>("RightPath").CheckColor("Hard", "Right", colors, newPath, _ball.Modulate);
        }
        _ball.QueueFree();
    }

    void _on_LeftTimer_timeout(){
        SpawnBall(Sides.Left);
    }

    void _on_RightTimer_timeout(){
        SpawnBall(Sides.Right);
    }

    void _on_LeftArea_input_event(Node viewport, InputEvent @event, int index){
        if(@event is InputEventScreenTouch touch && touch.Pressed){

            if(left_isLeft && left_canSwitch){
                GetNode<Tween>("TweenLeft").InterpolateProperty(leftLever, "rotation_degrees", leftLever.RotationDegrees, 0, 0.3f);
                GetNode<Tween>("TweenLeft").Start();
                left_canSwitch = false;
            }
            else if(!left_isLeft && left_canSwitch){
                GetNode<Tween>("TweenLeft").InterpolateProperty(leftLever, "rotation_degrees", leftLever.RotationDegrees, 40, 0.3f);
                GetNode<Tween>("TweenLeft").Start();
                left_canSwitch = false;
            }
        }
    }

    void _on_RightArea_input_event(Node viewport, InputEvent @event, int index){
        if(@event is InputEventScreenTouch touch && touch.Pressed){

            if(right_isLeft && right_canSwitch){
                GetNode<Tween>("TweenRight").InterpolateProperty(rightLever, "rotation_degrees", rightLever.RotationDegrees, -40, 0.3f);
                GetNode<Tween>("TweenRight").Start();
                right_canSwitch = false;
            }
            else if(!right_isLeft && right_canSwitch){
                GetNode<Tween>("TweenRight").InterpolateProperty(rightLever, "rotation_degrees", rightLever.RotationDegrees, 40, 0.3f);
                GetNode<Tween>("TweenRight").Start();
                right_canSwitch = false;
            }
        }
    }

    void _on_TweenLeft_tween_completed(Node Obj, NodePath key){
        left_canSwitch = true;
    }

    void _on_TweenRight_tween_completed(Node obj, NodePath key){
        right_canSwitch = true;
    }
}
