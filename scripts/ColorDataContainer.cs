using Godot;
using System;

public class ColorDataContainer : Node
{
    TextureRect color1;
    TextureRect color2;
    TextureRect color3;

    public Godot.Collections.Array<Ball.Colors> colors = new Godot.Collections.Array<Ball.Colors>();

    Global global;

    public override void _Ready()
    {
        global = GetNode<Global>("/root/Global");

        color1 = GetNode<TextureRect>("VBoxContainer/Color1");
        color2 = GetNode<TextureRect>("VBoxContainer/HBoxContainer/Color2");
        color3 = GetNode<TextureRect>("VBoxContainer/HBoxContainer/Color3");

        color3.Hide();
        GetNode<HBoxContainer>("VBoxContainer/HBoxContainer").Hide();

    }

    public override void _Process(float delta){
        if(global.waveCount == 2){
            GetNode<HBoxContainer>("VBoxContainer/HBoxContainer").Show();
        }
        if(global.waveCount == 3){
            color3.Show();
        }
    }

}
