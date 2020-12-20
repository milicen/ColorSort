using Godot;
using System;

public class Save : Node
{   
    string filePath_Easy = "user://highscore_easy.data";
    string filePath_Hard = "user://highscore_hard.data";
    public int highscore {get; set;}
    public int highscore_hard {get; set;}


    public override void _Ready(){
        LoadHighScore_Easy();
        LoadHighScore_Hard();
    }


    public void LoadHighScore_Easy(){
        var file = new File();
        if(!file.FileExists(filePath_Easy)){
            return;
        } 
        file.Open(filePath_Easy, File.ModeFlags.Read);
        highscore = (int) file.GetVar();
        file.Close();
    }

    public void SaveHighScore_Easy(){
        var file = new File();
        file.Open(filePath_Easy, File.ModeFlags.Write);
        file.StoreVar(highscore);
        file.Close();
    }

    public void SetHighScore_Easy(int newScore){
        highscore = newScore;
        SaveHighScore_Easy();
    }

    public void LoadHighScore_Hard(){
        var file = new File();
        if(!file.FileExists(filePath_Hard)){
            return;
        } 
        file.Open(filePath_Hard, File.ModeFlags.Read);
        highscore_hard = (int) file.GetVar();
        file.Close();
    }

    public void SaveHighScore_Hard(){
        var file = new File();
        file.Open(filePath_Hard, File.ModeFlags.Write);
        file.StoreVar(highscore_hard);
        file.Close();
    }

    public void SetHighScore_Hard(int newScore){
        highscore_hard = newScore;
        SaveHighScore_Hard();
    }


}
