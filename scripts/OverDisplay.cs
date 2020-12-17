using Godot;
using System;

public class OverDisplay : UIDisplay
{

    public string newPath {get; set;}
    public string level {get; set;}
    Global global;
    public override void _Ready(){
        base._Ready();
        global = GetNode<Global>("/root/Global");
    }

    public override void _on_Home_pressed(){
        if(HasNode("/root/HUD/" + newPath)){
            GetNode("/root/HUD/" + newPath).QueueFree();
        }
        
        GetNode<HUD>("/root/HUD").ResetScore();
        GetNode<MarginContainer>("/root/HUD/MainMenu/MarginContainer").Show();
        GetNode<MarginContainer>("/root/HUD/PlayDisplay/MarginContainer").Hide();
        GetNode<ColorRect>("ColorRect").Hide();
        GetTree().Paused = false;
    }

    public override void _on_Return_pressed(){
        base._on_Return_pressed();
        if(HasNode("/root/HUD/" + newPath)){
            GetNode("/root/HUD/" + newPath).QueueFree();
        }

        GetNode<HUD>("/root/HUD").ResetScore();

        if(global.mode == "Easy"){
            GetNode<HUD>("/root/HUD").LoadEasyMode(GetNode<HUD>("/root/HUD"));
        } else if(global.mode == "Hard"){
            GetNode<HUD>("/root/HUD").LoadHardMode(GetNode<HUD>("/root/HUD"));
        }
    }

}
