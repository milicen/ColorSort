using Godot;
using System;

public class Ball : PathFollow2D
{
    public enum Colors{
        Red,
        Orange,
        Yellow,
        Green,
        Blue,
        Violet
    }

    public Colors colors {get; set;}
    Global global;
    RegEx regex = new RegEx();
    RegExMatch resPath;
    string newPath;


    [Signal] 
    public delegate void endMainPath(Ball ball);
    [Signal]
    public delegate void endMainPathLeft(Ball ball);
    [Signal]
    public delegate void endMainPathRight(Ball ball);

    [Signal]
    public delegate void reachEnd(string side, Colors colors, Ball ball, string newPath);
    [Export] float speed = 10f;

    public override void _Ready(){
        global = GetNode<Global>("/root/Global");
        GD.Randomize();

        if(global.mode == "Easy"){
            int rand = (int) GD.RandRange(0, global.spawnColorsData.Count);

            switch (global.spawnColorsData[rand]){
                case Ball.Colors.Red:
                    Modulate = new Color(0.97f, 0.1f, 0.1f, 1);
                    break;
                case Ball.Colors.Orange:
                    Modulate = new Color(1, 0.64f, 0.14f, 1);
                    break;
                case Ball.Colors.Yellow:
                    Modulate = new Color(1, 0.88f, 0.08f, 1);
                    break;
                case Ball.Colors.Green:
                    Modulate = new Color(0.18f, 0.86f, 0.02f, 1);
                    break;
                case Ball.Colors.Blue:
                    Modulate = new Color(0.07f, 0.99f, 0.94f, 1);
                    break;
                case Ball.Colors.Violet:
                    Modulate = new Color(0.83f, 0.11f, 1, 1);
                    break;
            }
        colors = global.spawnColorsData[rand];

        } else if(global.mode == "Hard"){
            if(GetParent().Name == "MainPathLeft"){
                int rand = (int) GD.RandRange(0, global.spawnColorsData_Left.Count);

                switch (global.spawnColorsData_Left[rand]){
                    case Ball.Colors.Red:
                        Modulate = new Color(0.88f, 0, 0, 1);
                        break;
                    case Ball.Colors.Orange:
                        Modulate = new Color(0.88f, 0.57f, 0, 1);
                        break;
                    case Ball.Colors.Yellow:
                        Modulate = new Color(0.93f, 0.93f, 0, 1);
                        break;
                    case Ball.Colors.Green:
                        Modulate = new Color(0.07f, 0.85f, 0, 1);
                        break;
                    case Ball.Colors.Blue:
                        Modulate = new Color(0, 0.88f, 0.91f, 1);
                        break;
                    case Ball.Colors.Violet:
                        Modulate = new Color(0.6f, 0, 1, 1);
                        break;
                }

                colors = global.spawnColorsData_Left[rand];
                
            } else if(GetParent().Name == "MainPathRight"){
                int rand = (int) GD.RandRange(0, global.spawnColorsData_Right.Count);

                switch (global.spawnColorsData_Right[rand]){
                    case Ball.Colors.Red:
                        Modulate = new Color(0.88f, 0, 0, 1);
                        break;
                    case Ball.Colors.Orange:
                        Modulate = new Color(0.88f, 0.57f, 0, 1);
                        break;
                    case Ball.Colors.Yellow:
                        Modulate = new Color(0.93f, 0.93f, 0, 1);
                        break;
                    case Ball.Colors.Green:
                        Modulate = new Color(0.07f, 0.85f, 0, 1);
                        break;
                    case Ball.Colors.Blue:
                        Modulate = new Color(0, 0.88f, 0.91f, 1);
                        break;
                    case Ball.Colors.Violet:
                        Modulate = new Color(0.6f, 0, 1, 1);
                        break;
                }

                colors = global.spawnColorsData_Right[rand];
            }
        }


    }

    public override void _Process(float delta){
        Offset += speed * delta;
        if(UnitOffset == 1 && GetParent().Name == "MainPath"){
            EmitSignal(nameof(endMainPath), this);
        } 
        else if(UnitOffset == 1 && GetParent().Name == "MainPathLeft"){
            EmitSignal(nameof(endMainPathLeft), this);
        }
        else if(UnitOffset == 1 && GetParent().Name == "MainPathRight"){
            EmitSignal(nameof(endMainPathRight), this);
        }

        if(global.setCount == 2){
            speed = 180f;
        } else if(global.setCount >= 3){
            speed = 200f;
        }

        if(global.mode == "Easy"){
            regex.Compile("@?Easy@?\\d*");
            resPath = regex.Search(GetPath());
            newPath = resPath.GetString();
            if(Position.y >= GetNode<Position2D>("/root/HUD/" + newPath + "/EndPosition_y").GlobalPosition.y && GetParent().Name == "LeftPath"){
                EmitSignal(nameof(reachEnd), "Left", colors, this, newPath);
            } else if(Position.y >= GetNode<Position2D>("/root/HUD/" + newPath +"/EndPosition_y").GlobalPosition.y && GetParent().Name == "RightPath"){
                EmitSignal(nameof(reachEnd), "Right", colors, this, newPath);
            }
        } else if(global.mode == "Hard"){
            regex.Compile("@?Hard@?\\d*");
            resPath = regex.Search(GetPath());
            newPath = resPath.GetString();
            if(Position.y >= GetNode<Position2D>("/root/HUD/" + newPath + "/LeftEndPosition").GlobalPosition.y && GetParent().Name == "LeftPath"){
                EmitSignal(nameof(reachEnd), "Left", colors, this, newPath);
            } else if(Position.y >= GetNode<Position2D>("/root/HUD/" + newPath + "/MidEndPosition").GlobalPosition.y && (GetParent().Name == "MiddlePath1" || GetParent().Name == "MiddlePath2")){
                EmitSignal(nameof(reachEnd), "Middle", colors, this, newPath);
            } else if(Position.y >= GetNode<Position2D>("/root/HUD/" + newPath + "/RightEndPosition").GlobalPosition.y && GetParent().Name == "RightPath"){
                EmitSignal(nameof(reachEnd), "Right", colors, this, newPath);
            }
        }

        if(Modulate.a < 0.3f){
            QueueFree();
        }

    }

    public void Fade(){
        var color = new Color(Modulate.r, Modulate.g, Modulate.b, 0);
        GetNode<Tween>("Tween").InterpolateProperty(this, "modulate", Modulate, color, 0.2f);
        GetNode<Tween>("Tween").Start();
    }

    void _on_Tween_tween_completed(Node obj, NodePath key){
        obj.QueueFree();
    }

}
