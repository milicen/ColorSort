using Godot;
using System;

public class ColorDataContainer : Node
{
    TextureRect color1;
    TextureRect color2;
    TextureRect color3;

    public Godot.Collections.Array<Ball.Colors> colors = new Godot.Collections.Array<Ball.Colors>();

    Global global;
    Color initialColor = new Color(0.47f, 0.47f, 0.47f, 1);

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
        if(global.mode == "Easy"){
            if(global.waveCount == 1){
                color3.Hide();
                GetNode<HBoxContainer>("VBoxContainer/HBoxContainer").Hide();
            }
            if(global.waveCount == 2){
                GetNode<HBoxContainer>("VBoxContainer/HBoxContainer").Show();
            }
            if(global.waveCount == 3){
                color3.Show();
            }
        }

        if(global.mode == "Hard"){
            if(global.waveCount == 1){
                color3.Hide();
                GetNode<HBoxContainer>("VBoxContainer/HBoxContainer").Hide();
            }
            if(global.waveCount == 2){
                if(GetParent().Name == "MiddlePath1"){
                    GetNode<HBoxContainer>("VBoxContainer/HBoxContainer").Show();
                }
            }
            // if(global.waveCount == 3 && global.firstLeft && GetParent().Name == "LeftPath"){
            //     GetNode<HBoxContainer>("VBoxContainer/HBoxContainer").Show();
            // } else if(global.waveCount == 3 && !global.firstLeft && GetParent().Name == "RightPath"){
            //     GetNode<HBoxContainer>("VBoxContainer/HBoxContainer").Show();
            // }
            if(global.waveCount >= 3){
                GetNode<HBoxContainer>("VBoxContainer/HBoxContainer").Show();
            }
        }
    }

    public void ClearData(){
        colors.Clear();
        color1.Modulate = initialColor;
        color2.Modulate = initialColor;
        color3.Modulate = initialColor;
    }
}
