using Godot;
using System;

public class Easy : Node
{
    bool isRight = true;
    bool canSwitch = true;
    PackedScene ballScene = (PackedScene) GD.Load("res://scenes/Ball.tscn");

    Sprite lever;
    Position2D endCurvePosition;
    Path2D mainPath;

    Ball ball;
    Global global;

    Ball.Colors spawnBallColor;
    int sameColorCount = 0;

    public override void _Ready()
    {
        lever = GetNode<Sprite>("Lever");
        endCurvePosition = lever.GetNode<Position2D>("Position2D");

        mainPath = GetNode<Path2D>("MainPath");

        global = GetNode<Global>("/root/Global");
        
        if(global.colorsData.Count < 6){
            global.colorsData.Clear();
            global.AddData(global.colorsData);
        }

        if(global.unusedColorsData.Count <= 6){
            global.unusedColorsData.Clear();
            global.AddData(global.unusedColorsData);
        }

        if(global.spawnColorsData.Count >= 0){
            global.spawnColorsData.Clear();
            global.AddSpawnColorsData();
        }

        GD.Print("startover");

        global.setCount = 1;
        global.StartWaveTimer();
        
    }

    void _on_Timer_timeout(){
        SpawnBall();
    }

    void SpawnBall(){
        ball = (Ball) ballScene.Instance();
        mainPath.AddChild(ball);

        if(ball.colors != spawnBallColor){
            spawnBallColor = ball.colors;
            sameColorCount = 1;
        } else {
            sameColorCount++;
        }

        if(sameColorCount >= 3 && ball.colors == spawnBallColor){
            mainPath.RemoveChild(ball);
            ball.QueueFree();
            SpawnBall();
        } else if(sameColorCount < 3) {
            ball.Offset = 0;  
            ball.Connect("endMainPath", this, nameof(_on_Ball_endMainPath));
            ball.Connect("reachEnd", this, nameof(_on_Ball_reachEnd)); 

            GD.Print("ball_color : " + ball.colors);
            GD.Print("color : " + spawnBallColor);
            GD.Print("count : " + sameColorCount);
        }
        
        

    }

    public override void _Process(float delta){
        mainPath.Curve.SetPointPosition(2, endCurvePosition.GlobalPosition);

        if(lever.RotationDegrees <= -40){
            isRight = true;
        } else if(lever.RotationDegrees >= 40){
            isRight = false;
        }


        if(global.setCount == 2){
            GetNode<Timer>("Timer").WaitTime = 1.4f;
        } else if(global.setCount >= 3){
            GetNode<Timer>("Timer").WaitTime = 0.8f;
        }
    }

    void _on_Area2D_input_event(Node viewport, InputEvent @event, int index){
        if(@event is InputEventScreenTouch touch && touch.Pressed){
            if(isRight && canSwitch){
                GetNode<Tween>("Tween").InterpolateProperty(lever, "rotation_degrees", lever.RotationDegrees, 40, 0.3f);
                GetNode<Tween>("Tween").Start();
                canSwitch = false;
            } else if(!isRight && canSwitch){
                GetNode<Tween>("Tween").InterpolateProperty(lever, "rotation_degrees", lever.RotationDegrees, -40, 0.3f);
                GetNode<Tween>("Tween").Start();
                canSwitch = false;
            }
        }
    }

    void _on_Tween_tween_completed(Node obj, NodePath key){
        canSwitch = true;
    }

    void _on_Ball_endMainPath(Ball _ball){
        if(isRight && canSwitch){
            mainPath.RemoveChild(_ball);
            GetNode<Path2D>("RightPath").AddChild(_ball);
            _ball.Offset = 0;
        } else if(!isRight && canSwitch){
            mainPath.RemoveChild(_ball);
            GetNode<Path2D>("LeftPath").AddChild(_ball);
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
            else if(global.waveCount == 3 && GetNode<Path>("LeftPath").colors.Count < 3 && !GetNode<Path>("LeftPath").colors.Contains(colors) && global.unusedColorsData.Contains(colors) && GetNode<ColorDataContainer>("LeftPath/ColorDataContainer").colors.Count < 3){
                GetNode<TextureRect>("LeftPath/ColorDataContainer/VBoxContainer/HBoxContainer/Color3").Modulate = _ball.Modulate;
                GetNode<ColorDataContainer>("LeftPath/ColorDataContainer").colors.Add(colors);
            }
            
            GetNode<Path>("LeftPath").CheckColor("Easy", "Left", colors, newPath, _ball.Modulate);
        } else if(side == "Right"){
            if(GetNode<Path>("RightPath").colors.Count == 0 && global.unusedColorsData.Contains(colors)){
                GetNode<TextureRect>("RightPath/ColorDataContainer/VBoxContainer/Color1").Modulate = _ball.Modulate;
                GetNode<ColorDataContainer>("RightPath/ColorDataContainer").colors.Add(colors);
            }else if((global.waveCount == 2 || global.waveCount == 3) && GetNode<Path>("RightPath").colors.Count < 2 && !GetNode<Path>("RightPath").colors.Contains(colors) && global.unusedColorsData.Contains(colors) && GetNode<ColorDataContainer>("RightPath/ColorDataContainer").colors.Count < 2){
                GetNode<TextureRect>("RightPath/ColorDataContainer/VBoxContainer/HBoxContainer/Color2").Modulate = _ball.Modulate;
                GetNode<ColorDataContainer>("RightPath/ColorDataContainer").colors.Add(colors);
            } else if(global.waveCount == 3 && GetNode<Path>("RightPath").colors.Count < 3 && !GetNode<Path>("RightPath").colors.Contains(colors) && global.unusedColorsData.Contains(colors) && GetNode<ColorDataContainer>("RightPath/ColorDataContainer").colors.Count < 3){
                GetNode<TextureRect>("RightPath/ColorDataContainer/VBoxContainer/HBoxContainer/Color3").Modulate = _ball.Modulate;
                GetNode<ColorDataContainer>("RightPath/ColorDataContainer").colors.Add(colors);
            }

            GetNode<Path>("RightPath").CheckColor("Easy", "Right", colors, newPath, _ball.Modulate);
        }
        // GD.Print(_ball.Modulate);
        _ball.QueueFree();
    }
}
