using Godot;
using System;

public class Global : Node
{
    public string mode{get; set;}
    public Godot.Collections.Array<Ball.Colors> colorsData = new Godot.Collections.Array<Ball.Colors>();
    public Godot.Collections.Array<Ball.Colors> midColors = new Godot.Collections.Array<Ball.Colors>();
    public Godot.Collections.Array<Ball.Colors> spawnColorsData = new Godot.Collections.Array<Ball.Colors>();
    public Godot.Collections.Array<Ball.Colors> unusedColorsData = new Godot.Collections.Array<Ball.Colors>();
    public Godot.Collections.Array<Ball.Colors> spawnColorsData_Left = new Godot.Collections.Array<Ball.Colors>();
    public Godot.Collections.Array<Ball.Colors> spawnColorsData_Right = new Godot.Collections.Array<Ball.Colors>();
    

    public bool firstLeft{get; set;}

    public Timer waveTimer = new Timer();
    public Timer delayTimer = new Timer();
    public float delayTime = 3;
    public float waveTime = 10;
    public int waveCount = 1;
    public int setCount = 1;

    public override void _Ready()
    {
        AddData(colorsData);
        AddChild(waveTimer);
        AddChild(delayTimer);
        delayTimer.Name = "DelayTimer";
        delayTimer.WaitTime = delayTime;
        waveTimer.Name = "WaveTimer";
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
        if(mode == "Easy"){ 
            waveCount++;
            if(waveCount < 4){
                AddSpawnColorsData();
            }          
            else if(waveCount == 4){
                ResetWaveTimer();
                setCount++;

                GetTree().CallGroup("ColorData", "ClearData");
                GetTree().CallGroup("Path", "ClearData");
                GetTree().CallGroup("Ball", "Fade");

                if(colorsData.Count != 0){
                    colorsData.Clear();
                }
                AddData(colorsData);

                if(unusedColorsData.Count != 0){
                    unusedColorsData.Clear();
                }
                AddData(unusedColorsData);

                spawnColorsData.Clear();
                AddSpawnColorsData();
                
                
                waveTimer.Start();

            }
        }

        if(mode == "Hard"){
            waveCount++;

            if(waveCount == 2 && firstLeft){
                firstLeft = !firstLeft;
                GetTree().CallGroup("LeftTimer", "stop");
                GetTree().CallGroup("RightTimer", "start");
                AddSpawnColorsData("Right");
            } else if(waveCount == 2 && !firstLeft){
                firstLeft = !firstLeft;
                GetTree().CallGroup("RightTimer", "stop");
                GetTree().CallGroup("LeftTimer", "start");
                AddSpawnColorsData("Left");
            } else if(waveCount == 3 && firstLeft){
                GetTree().CallGroup("RightTimer", "start");
                GetTree().CallGroup("LeftTimer", "start");
                AddSpawnColorsData("Right");
            } else if(waveCount == 3 && !firstLeft){
                GetTree().CallGroup("RightTimer", "start");
                GetTree().CallGroup("LeftTimer", "start");
                AddSpawnColorsData("Left");
            } else if(waveCount == 4){
                if(colorsData.Count > 0){
                    if(spawnColorsData_Left.Count < 3){
                        spawnColorsData_Left.Add(colorsData[0]);
                        colorsData.Clear();
                    } else if(spawnColorsData_Right.Count < 3){
                        spawnColorsData_Right.Add(colorsData[0]);
                        colorsData.Clear();
                    }

                }
            } else if(waveCount == 5){
                ResetWaveTimer();
                setCount++;

                GetTree().CallGroup("ColorData", "ClearData");
                GetTree().CallGroup("Path", "ClearData");
                GetTree().CallGroup("Ball", "Fade");

                if(colorsData.Count != 0){
                    colorsData.Clear();
                }
                AddData(colorsData);

                if(unusedColorsData.Count != 0){
                    unusedColorsData.Clear();
                }
                AddData(unusedColorsData);

                midColors.Clear();
                spawnColorsData_Left.Clear();
                spawnColorsData_Right.Clear();

                AddSpawnColorsData("Left");
                AddSpawnColorsData("Right");

                if(firstLeft){
                    GetTree().CallGroup("RightTimer", "stop");
                } else {
                    GetTree().CallGroup("LeftTimer", "stop");
                }
                
                             
                waveTimer.Start();
            }

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

    public void AddSpawnColorsData(string side = "none"){
        GD.Randomize();

        if(side == "none"){
            for(int i = 0; i < 2; i++){
                int rand = (int) GD.RandRange(0, colorsData.Count);
                spawnColorsData.Add(colorsData[rand]);
                colorsData.Remove(colorsData[rand]);
            }
        } else if(side == "Left" && waveCount == 1){
            for(int i = 0; i < 2; i++){
                int rand = (int) GD.RandRange(0, colorsData.Count);
                spawnColorsData_Left.Add(colorsData[rand]);
                colorsData.Remove(colorsData[rand]);

            }
        } else if(side == "Right" && waveCount == 1){
            for(int i = 0; i < 2; i++){
                int rand = (int) GD.RandRange(0, colorsData.Count);
                spawnColorsData_Right.Add(colorsData[rand]);
                colorsData.Remove(colorsData[rand]);

            }
        } else if(waveCount == 3){
            int rand1 = (int) GD.RandRange(0, colorsData.Count);
            spawnColorsData_Left.Add(colorsData[rand1]);
            colorsData.Remove(colorsData[rand1]);

            int rand2 = (int) GD.RandRange(0, colorsData.Count);
            spawnColorsData_Right.Add(colorsData[rand2]);
            colorsData.Remove(colorsData[rand2]);
        }

        // GD.Print("colors_data : " + colorsData);
        // GD.Print("spawn_colors_data_left " + spawnColorsData_Left);
        // GD.Print("spawn_colors_data_right " + spawnColorsData_Right);

    }

}
