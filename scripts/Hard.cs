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

    Ball.Colors spawnBallColorLeft;
    Ball.Colors spawnBallColorRight;

    int sameColorCOuntLeft;
    int sameColorCOuntRight;


    PackedScene ballScene = (PackedScene) GD.Load("res://scenes/Ball.tscn");

    Sprite leftLever;
    Sprite rightLever;

    Ball ball;
    Path2D mainPathLeft;
    Path2D mainPathRight;

    Timer leftTimer;
    Timer rightTimer;

    Global global;

    public override void _Ready()
    {
        leftLever = GetNode<Sprite>("LeverLeft");
        rightLever = GetNode<Sprite>("LeverRight");

        mainPathLeft = GetNode<Path2D>("MainPathLeft");
        mainPathRight = GetNode<Path2D>("MainPathRight");

        leftTimer = GetNode<Timer>("LeftTimer");
        rightTimer = GetNode<Timer>("RightTimer");

        global = GetNode<Global>("/root/Global");

        GD.Randomize();

        if(global.colorsData.Count < 6){
            global.colorsData.Clear();
            global.AddData(global.colorsData);
        }

        if(global.unusedColorsData.Count <= 6){
            global.unusedColorsData.Clear();
            global.AddData(global.unusedColorsData);
        }

        if(global.spawnColorsData_Left.Count >= 0){
            global.spawnColorsData_Left.Clear();
            global.AddSpawnColorsData("Left");
        }

        if(global.spawnColorsData_Right.Count >= 0){
            global.spawnColorsData_Right.Clear();
            global.AddSpawnColorsData("Right");
        }

        if(global.midColors.Count >= 0){
            global.midColors.Clear();
        }

        int rand = (int) GD.RandRange(0,10);
        if(rand < 4){
            leftTimer.Start();
            global.firstLeft = true;
        } else {
            rightTimer.Start();
            global.firstLeft = false;
        }

        GD.Print("startover");

        global.setCount = 1;
        global.StartWaveTimer();

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

            if(ball.colors != spawnBallColorLeft){
                spawnBallColorLeft = ball.colors;
                sameColorCOuntLeft = 1;
            } else {
                sameColorCOuntLeft++;
            }

            if(sameColorCOuntLeft >= 3 && ball.colors == spawnBallColorLeft){
                mainPathLeft.RemoveChild(ball);
                ball.QueueFree();
                SpawnBall(Sides.Left);
            } else if(sameColorCOuntLeft < 3){
                ball.Connect("endMainPathLeft", this, nameof(_on_Ball_endMainPathLeft));
                ball.Connect("reachEnd", this, nameof(_on_Ball_reachEnd));
                ball.Offset = 0;  
            }
        } else {
            mainPathRight.AddChild(ball);

            if(ball.colors != spawnBallColorRight){
                spawnBallColorRight = ball.colors;
                sameColorCOuntRight = 1;
            } else {
                sameColorCOuntRight++;
            }

            if(sameColorCOuntRight >= 3 && ball.colors == spawnBallColorRight){
                mainPathRight.RemoveChild(ball);
                ball.QueueFree();
                SpawnBall(Sides.Right);
            } else if(sameColorCOuntRight < 3){
                ball.Connect("endMainPathRight", this, nameof(_on_Ball_endMainPathRight));
                ball.Connect("reachEnd", this, nameof(_on_Ball_reachEnd));
                ball.Offset = 0;  
            }
        }
        // ball.Connect("reachEnd", this, nameof(_on_Ball_reachEnd));
        // ball.Offset = 0;      

        // GD.Print("ball_color : " + ball.colors);
        // GD.Print("color : " + spawnBallColor);
        // GD.Print("count : " + sameColorCount);
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
            if(GetNode<Path>("LeftPath").colors.Count == 0 && global.unusedColorsData.Contains(colors)){
                GetNode<TextureRect>("LeftPath/ColorDataContainer/VBoxContainer/Color1").Modulate = _ball.Modulate;
                GetNode<ColorDataContainer>("LeftPath/ColorDataContainer").colors.Add(colors);
            }
            else if((global.waveCount == 2 || global.waveCount == 3) && GetNode<Path>("LeftPath").colors.Count < 2 && !GetNode<Path>("LeftPath").colors.Contains(colors) && global.unusedColorsData.Contains(colors) && GetNode<ColorDataContainer>("LeftPath/ColorDataContainer").colors.Count < 2){
                GetNode<TextureRect>("LeftPath/ColorDataContainer/VBoxContainer/HBoxContainer/Color2").Modulate = _ball.Modulate;
                GetNode<ColorDataContainer>("LeftPath/ColorDataContainer").colors.Add(colors);
            }

            GetNode<Path>("LeftPath").CheckColor("Hard", "Left", colors, newPath, _ball.Modulate);
        } 
        else if(side == "Middle"){
            if(global.midColors.Count == 0 && global.unusedColorsData.Contains(colors)){
                GetNode<TextureRect>("MiddlePath1/ColorDataContainer/VBoxContainer/Color1").Modulate = _ball.Modulate;
                GetNode<ColorDataContainer>("MiddlePath1/ColorDataContainer").colors.Add(colors);
            }else if(global.waveCount > 1 && global.midColors.Count < 2 && !global.midColors.Contains(colors) && global.unusedColorsData.Contains(colors) && GetNode<ColorDataContainer>("MiddlePath1/ColorDataContainer").colors.Count < 2){
                GetNode<TextureRect>("MiddlePath1/ColorDataContainer/VBoxContainer/HBoxContainer/Color2").Modulate = _ball.Modulate;
                GetNode<ColorDataContainer>("MiddlePath1/ColorDataContainer").colors.Add(colors);
            }

            if(_ball.GetParent().Name == "MiddlePath1"){
                GetNode<Path>("MiddlePath1").CheckColor("Hard", "Middle", colors, newPath, _ball.Modulate);
            } else if(_ball.GetParent().Name == "MiddlePath2"){
                GetNode<Path>("MiddlePath2").CheckColor("Hard", "Middle", colors, newPath, _ball.Modulate);
            }
        } 
        else if(side == "Right"){
            if(GetNode<Path>("RightPath").colors.Count == 0 && global.unusedColorsData.Contains(colors)){
                GetNode<TextureRect>("RightPath/ColorDataContainer/VBoxContainer/Color1").Modulate = _ball.Modulate;
                GetNode<ColorDataContainer>("RightPath/ColorDataContainer").colors.Add(colors);
            }else if(global.waveCount > 1 && GetNode<Path>("RightPath").colors.Count < 2 && !GetNode<Path>("RightPath").colors.Contains(colors) && global.unusedColorsData.Contains(colors) && GetNode<ColorDataContainer>("RightPath/ColorDataContainer").colors.Count < 2){
                GetNode<TextureRect>("RightPath/ColorDataContainer/VBoxContainer/HBoxContainer/Color2").Modulate = _ball.Modulate;
                GetNode<ColorDataContainer>("RightPath/ColorDataContainer").colors.Add(colors);
            }

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
