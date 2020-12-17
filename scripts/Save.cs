using Godot;
using System;

public class Save : Node
{   
    string filePath = "user://highscore.data";
    public int highscore {get; set;}
    public int highscore_hard {get; set;}


    public Godot.Collections.Dictionary<string, int> highscores = new Godot.Collections.Dictionary<string, int>(){
        {"highscore", 0}, {"highscore_hard", 0}
    };
    string path = "user://highscores.data";


    public void LoadHighScores(){

        var file = new File();
        if(!file.FileExists(path)){
            return;
        }
        file.Open(path, File.ModeFlags.Read);
        var readFile = GD.Str2Var(file.GetAsText());
        GD.Print("getvar : " + file.GetVar());


        file.Close();

        GD.Print("highscores : " + highscores);
    }

    public void SaveHighScores(){
        // if(highscores == null){
        //     highscores.Add("highscore", highscore);
        //     highscores.Add("highscore_hard", highscore_hard);
        // }

        var file = new File();
        file.Open(path, File.ModeFlags.Write);
        file.StoreVar(highscores);
        file.Close();
        // GD.Print(highscores);
    }
        




    public override void _Ready()
    {
        // SaveHighScores();
        LoadHighScores();
    }

    public void LoadHighScore(){
        var file = new File();
        if(!file.FileExists(filePath)){
            return;
        } 
        file.Open(filePath, File.ModeFlags.Read);
        highscore = (int) file.GetVar();
        file.Close();
    }

    public void SaveHighScore(){
        var file = new File();
        file.Open(filePath, File.ModeFlags.Write);
        file.StoreVar(highscore);
        file.Close();
    }

    public void SetHighScore(int newScore){
        highscores["highscore"] = newScore;
        highscore = highscores["highscore"];
        SaveHighScores();
    }

    public void SetHighScore_Hard(int newScore){
        highscores["highscore_hard"] = newScore;
        highscore_hard = highscores["highscore_hard"];

        SaveHighScores();
    }


}
