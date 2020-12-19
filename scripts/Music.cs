using Godot;
using System;

public class Music : Node
{


    public void Over()
    {
        GetNode<AudioStreamPlayer>("Over").Play();
    }

    public void PlaySound(){
        GetNode<AudioStreamPlayer>("SFX").Play();
    }

}
