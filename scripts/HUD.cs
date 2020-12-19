using Godot;
using System;

public class HUD : Node
{
    [Signal]
    public delegate void easyModeOn(Easy easyMode);
    [Signal]
    public delegate void hardModeOn(Hard hardMode);
    Button playButton;
    Button quitButton;
    Button easyButton;
    Button hardButton;
    Button backButton;

    public int score = 0;
    public Label scoreLabel;

    public string _newPath;
    Save file;
    Global global;
    Music music;

    public override void _Ready()
    {
        playButton = GetNode<Button>("MainMenu/MarginContainer/VBoxContainer/MarginContainer/VBoxContainer/Play");

        quitButton = GetNode<Button>("MainMenu/MarginContainer/VBoxContainer/MarginContainer/VBoxContainer/Quit");

        easyButton = GetNode<Button>("MainMenu/MarginContainer/VBoxContainer/MarginContainer/VBoxContainer/Easy");

        hardButton = GetNode<Button>("MainMenu/MarginContainer/VBoxContainer/MarginContainer/VBoxContainer/Hard");

        backButton = GetNode<Button>("MainMenu/MarginContainer/VBoxContainer/MarginContainer/VBoxContainer/Back");

        file = GetNode<Save>("/root/Save");
        global = GetNode<Global>("/root/Global");
        music = GetNode<Music>("/root/Music");
        scoreLabel = GetNode<Label>("PlayDisplay/MarginContainer/HBoxContainer/Score");

        ShowButtons("Ready");
    }

    public void ResetScore(){
        score = 0;
        scoreLabel.Text = score.ToString();
    }

    public void LoadEasyMode(HUD hud){
        var easyModeScene = (PackedScene) GD.Load("res://scenes/Easy.tscn");
        var easyMode = easyModeScene.Instance();
        hud.AddChild(easyMode);

        easyMode.GetNode<Path>("LeftPath").Connect("getScore", hud, nameof(_on_Path_getScore));
        easyMode.GetNode<Path>("RightPath").Connect("getScore", hud, nameof(_on_Path_getScore));

        easyMode.GetNode<Path>("LeftPath").Connect("die", hud, nameof(_on_Path_die));
        easyMode.GetNode<Path>("RightPath").Connect("die", hud, nameof(_on_Path_die));
    }

    public void LoadHardMode(HUD hud){
        var hardModeScene = (PackedScene) GD.Load("res://scenes/Hard.tscn");
        var hardMode = hardModeScene.Instance();
        hud.AddChild(hardMode);

        hardMode.GetNode<Path>("LeftPath").Connect("getScore", hud, nameof(_on_Path_getScore));
        hardMode.GetNode<Path>("RightPath").Connect("getScore", hud, nameof(_on_Path_getScore));
        hardMode.GetNode<Path>("MiddlePath1").Connect("getScore", hud, nameof(_on_Path_getScore));
        hardMode.GetNode<Path>("MiddlePath2").Connect("getScore", hud, nameof(_on_Path_getScore));

        hardMode.GetNode<Path>("LeftPath").Connect("die", hud, nameof(_on_Path_die));
        hardMode.GetNode<Path>("RightPath").Connect("die", hud, nameof(_on_Path_die));
        hardMode.GetNode<Path>("MiddlePath1").Connect("die", hud, nameof(_on_Path_die));
        hardMode.GetNode<Path>("MiddlePath2").Connect("die", hud, nameof(_on_Path_die));
    }

    void _on_Play_pressed(){
        ShowButtons("Play");
        music.PlaySound();
    }

    void _on_Quit_pressed(){
        GetTree().Quit();
    }

    void _on_Easy_pressed(){
        music.PlaySound();
        LoadEasyMode(this);
        global.mode = "Easy";

        ShowButtons("Ready");
        GetNode<MarginContainer>("MainMenu/MarginContainer").Hide();
        GetNode<MarginContainer>("PlayDisplay/MarginContainer").Show();
    }

    void _on_Hard_pressed(){
        music.PlaySound();
        LoadHardMode(this);
        global.mode = "Hard";

        ShowButtons("Ready");
        GetNode<MarginContainer>("MainMenu/MarginContainer").Hide();
        GetNode<MarginContainer>("PlayDisplay/MarginContainer").Show();
    }

    void _on_Back_pressed(){
        music.PlaySound();
        ShowButtons("Ready");
    }

    void _on_Pause_pressed(){
        music.PlaySound();
        GetNode<ColorRect>("PauseDisplay/ColorRect").Show();
        GetTree().Paused = true;
    }

    

    void _on_Path_getScore(string side){
        score++;
        GetNode<Label>("PlayDisplay/MarginContainer/HBoxContainer/Score").Text = score.ToString();
    }

    void _on_Path_die(string newPath, string level){
        music.Over();
        global.ResetWaveTimer();

        GetTree().Paused = true;
        GetNode<ColorRect>("OverDisplay/ColorRect").Show();
        GetNode<OverDisplay>("OverDisplay").newPath = newPath;
        GetNode<OverDisplay>("OverDisplay").level = level;
        GetNode<Label>("OverDisplay/ColorRect/MarginContainer/VBoxContainer/ScoreContainer/Score/ScoreNum").Text = score.ToString();
        if(level == "Easy"){
            if(score >= file.highscore){
                file.SetHighScore_Easy(score);
            }
            GetNode<Label>("OverDisplay/ColorRect/MarginContainer/VBoxContainer/ScoreContainer/HighScore/ScoreNum").Text = file.highscore.ToString(); 
        } else {
            if(score >= file.highscore_hard){
                file.SetHighScore_Hard(score);
            }
            GetNode<Label>("OverDisplay/ColorRect/MarginContainer/VBoxContainer/ScoreContainer/HighScore/ScoreNum").Text = file.highscore_hard.ToString(); 
        }
    }

    void ShowButtons(string ready){
        if(ready == "Ready"){
            playButton.Show();
            quitButton.Show();
            easyButton.Hide();
            hardButton.Hide();
            backButton.Hide();
        } else if(ready == "Play"){
            playButton.Hide();
            quitButton.Hide();
            easyButton.Show();
            hardButton.Show();
            backButton.Show();
        }
    }
}
