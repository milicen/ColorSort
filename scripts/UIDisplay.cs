using Godot;
using System;

public class UIDisplay : CanvasLayer
{

    public Music music;
    public override void _Ready()
    {
        GetNode<ColorRect>("ColorRect").Hide();
        music = GetNode<Music>("/root/Music");
    }

    public virtual void _on_Home_pressed(){
        music.PlaySound();
        if(HasNode("/root/HUD/Easy")){
            GetNode<Easy>("/root/HUD/Easy").QueueFree();
        } else if(HasNode("/root/HUD/Hard")){
            GetNode<Hard>("/root/HUD/Hard").QueueFree();
        }
        GetNode<HUD>("/root/HUD").ResetScore();
        GetNode<MarginContainer>("/root/HUD/MainMenu/MarginContainer").Show();
        GetNode<MarginContainer>("/root/HUD/PlayDisplay/MarginContainer").Hide();
        GetNode<ColorRect>("ColorRect").Hide();
        GetTree().Paused = false;
    }

    public virtual void _on_Return_pressed(){
        music.PlaySound();
        GetNode<ColorRect>("ColorRect").Hide();
        GetTree().Paused = false;
    }

}
