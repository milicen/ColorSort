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

        int rand = (int) GD.RandRange(0, global.spawnColorsData.Count);

        switch (global.spawnColorsData[rand]){
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

        colors = global.spawnColorsData[rand];

        // int rand = (int) GD.RandRange(0, 6);
        // switch (rand){
        //     case 1 :
        //         colors = Colors.Red;
        //         var color1 = new Color(0.88f, 0, 0, 1);
        //         Modulate = color1;
        //         break;
        //     case 2 :
        //         colors = Colors.Orange;
        //         var color2 = new Color(0.88f, 0.57f, 0, 1);
        //         Modulate = color2;
        //         break;
        //     case 3 :
        //         colors = Colors.Yellow;
        //         var color3 = new Color(0.93f, 0.93f, 0, 1);
        //         Modulate = color3;
        //         break;
        //     case 4 :
        //         colors = Colors.Green;
        //         var color4 = new Color(0.07f, 0.85f, 0, 1);
        //         Modulate = color4;
        //         break;
        //     case 5 :
        //         colors = Colors.Blue;
        //         var color5 = new Color(0, 0.88f, 0.91f, 1);
        //         Modulate = color5;
        //         break;
        //     default :
        //         colors = Colors.Violet;
        //         var color6 = new Color(0.6f, 0, 1, 1);
        //         Modulate = color6;
        //         break;                   
        // }
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

        //easy mode
        if(global.mode == "Easy"){
            // var regex = new RegEx();
            // if(HasNode("/root/HUD/" + newPath) /*&& global.mode == "Easy"*/){
            regex.Compile("@?Easy@?\\d*");
            resPath = regex.Search(GetPath());
            newPath = resPath.GetString();
            if(Position.y >= GetNode<Position2D>("/root/HUD/" + newPath + "/EndPosition_y").GlobalPosition.y && GetParent().Name == "LeftPath"){
                EmitSignal(nameof(reachEnd), "Left", colors, this, newPath);
            } else if(Position.y >= GetNode<Position2D>("/root/HUD/" + newPath +"/EndPosition_y").GlobalPosition.y && GetParent().Name == "RightPath"){
                EmitSignal(nameof(reachEnd), "Right", colors, this, newPath);
            }
            // }
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

            
        
        //hard mode
        // var regex1 = new RegEx();
        // regex1.Compile("@?Hard@?\\d*");
        // var resPath1 = regex1.Search(GetPath());
        // var newPath1 = resPath1.GetString();

        // if(HasNode("/root/HUD/Hard")){
        // }

    }

}
