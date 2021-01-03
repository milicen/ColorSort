using Godot;
using System;

public class Tutorial : MarginContainer
{
    int index;
    Texture image1 = (Texture) GD.Load("res://assets/Tutorial1.png");
    Texture image2 = (Texture) GD.Load("res://assets/Tutorial2.png");
    Texture image3 = (Texture) GD.Load("res://assets/Tutorial3.png");
    Texture image4 = (Texture) GD.Load("res://assets/Tutorial4.png");

    TextureRect image;
    Label description;

    Button previous;
    Button next;

    public override void _Ready()
    {
        image = GetNode<TextureRect>("VBoxContainer/Image");
        description = GetNode<Label>("VBoxContainer/Description");

        previous = GetNode<Button>("VBoxContainer/Buttons/Previous");
        next = GetNode<Button>("VBoxContainer/Buttons/Next");   

        index = 1;
        image.Texture = image1;

        previous.Disabled = true;
    }



    public override void _Process(float delta)
    {
        switch(index){
            case 1 :
                image.Texture = image1;

                description.Text = "Firstly you can see at the bottom of each path has a circle with no color assigned.";

                previous.Disabled = true;
                next.Disabled = false;
                break;
            case 2 :
                image.Texture = image2;

                description.Text = "Colored balls will appear from the top of the path. Since the circle below each path hasn't being assigned to a specific color, you can redirect a ball of specific color to either path by clicking anywhere on the screen, which will move the lever.";

                previous.Disabled = false;
                next.Disabled = false;
                break;
            case 3 :
                image.Texture = image3;

                description.Text = "After the circles are assigned to a color, all upcoming balls with that color have to be redirected to the path which leads to that circle.";

                previous.Disabled = false;
                next.Disabled = false;
                break;
            case 4 :
                image.Texture = image4;

                description.Text = "The preceding tutorials are based on the \"Easy\" mode. In \"Hard\" mode, however, all the mechanics are the same, except, instead of clicking anywhere on the screen to move the lever, you will need to click on the left side of the screen to move the left lever, and the right screen for right lever.";

                previous.Disabled = false;
                next.Disabled = true;
                break;          
        }
    }

    void _on_Previous_pressed(){
        index--;
    }

    void _on_Next_pressed(){
        index++;
    }

    void _on_Home_pressed(){
        GetNode<HUD>("/root/HUD").ShowUI(true);
        // GetNode<MarginContainer>("/root/HUD/MainMenu/MarginContainer").Show();
        QueueFree();
    }
}
