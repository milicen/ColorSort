using Godot;
using System;

public class Global : Node
{
    public string mode{get; set;}
    public Godot.Collections.Array<Ball.Colors> colorsData = new Godot.Collections.Array<Ball.Colors>();
    public Godot.Collections.Array<Ball.Colors> midColors = new Godot.Collections.Array<Ball.Colors>();
    public Godot.Collections.Array<Ball.Colors> spawnColorsData = new Godot.Collections.Array<Ball.Colors>();
    public Godot.Collections.Array<Ball.Colors> unusedColorsData = new Godot.Collections.Array<Ball.Colors>();


    public Timer waveTimer = new Timer();
    public float waveTime = 20;
    public int waveCount = 1;

    public override void _Ready()
    {
        AddData(colorsData);
        AddChild(waveTimer);
        waveTimer.WaitTime = waveTime;
        waveTimer.Connect("timeout", this, nameof(_on_Timer_timeout));
    }

    public void StartWaveTimer(){
        waveTimer.Start();
    }

    public void StopWaveTimer(){
        waveTimer.Stop();
    }

    public void ResetWaveTimer(){
        StopWaveTimer();
        waveTimer.WaitTime = waveTime;
        waveCount = 1;
    }

    void _on_Timer_timeout(){
        if(waveCount < 3){
            waveCount++;
            AddSpawnColorsData();
        } else {
            waveTimer.Stop();
        }
    }

    public void AddData(Godot.Collections.Array<Ball.Colors> data){
        data.Add(Ball.Colors.Red);
        data.Add(Ball.Colors.Orange);
        data.Add(Ball.Colors.Yellow);
        data.Add(Ball.Colors.Green);
        data.Add(Ball.Colors.Blue);
        data.Add(Ball.Colors.Violet);
    }

    public void ResetMidColorsData(){
        midColors.Clear();
    }

    // public void ResetUnusedColorsData(){
    //     unusedColorsData = colorsData;
    // }

    public void AddSpawnColorsData(){
        GD.Randomize();

        for(int i = 0; i < 2; i++){
            int rand = (int) GD.RandRange(0, colorsData.Count);
            spawnColorsData.Add(colorsData[rand]);
            // unusedColorsData.Remove(colorsData[rand]);
            colorsData.Remove(colorsData[rand]);
        }
 


        // colorsData.Clear();
        // AddData();

        GD.Print("spawn_colors_data : " + spawnColorsData);
        GD.Print("colors_data : " + colorsData);

    }

}
