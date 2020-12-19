using Godot;
using System;

public class Path : Path2D
{
    [Signal]
    public delegate void getScore(string side);
    [Signal]
    public delegate void die(string newPath, string level);

    public Godot.Collections.Array<Ball.Colors> colors = new Godot.Collections.Array<Ball.Colors>();
    Global global;


    public override void _Ready()
    {
        global = GetNode<Global>("/root/Global");
        // if(global.colorsData.Count < 6){
        //     global.colorsData.Clear();
        //     global.AddData();
        // }

        // if(global.mode == "Hard"){
        //     global.ResetMidColorsData();
        // }

        // if(global.spawnColorsData.Count >= 0){
        //     global.spawnColorsData.Clear();
        //     global.AddSpawnColorsData();
        // }

        // if(global.unusedColorsData.Count <= 6){
        //     global.ResetUnusedColorsData();
        // }
    }

    public void ClearData(){
        colors.Clear();
    }

    public void CheckColor(string level, string side, Ball.Colors _color, string newPath, Color _ballColor){
        if(level == "Easy"){
            WaveEasy(global.waveCount, level, side, _color, newPath, _ballColor);
        }

        if(level == "Hard"){
            WaveHard(global.waveCount, level, side, _color, newPath, _ballColor);
            // if(side == "Middle"){
            //     if(global.midColors == null && global.colorsData.Contains(_color)){
            //         global.colorsData.Remove(_color);
            //         global.midColors.Add(_color);
            //         EmitSignal(nameof(getScore), side);
            //     } else if(!global.midColors.Contains(_color) && global.midColors.Count < 2 && global.colorsData.Contains(_color)){
            //         global.colorsData.Remove(_color);
            //         global.midColors.Add(_color);
            //         EmitSignal(nameof(getScore), side);
            //     } else if(!global.midColors.Contains(_color) && global.midColors.Count < 2 && !global.colorsData.Contains(_color)){
            //         GD.Print("wrong place for " + _color);
            //         EmitSignal(nameof(die), newPath, level);
            //     } else if(!global.midColors.Contains(_color) && global.midColors.Count >= 2 && global.colorsData.Contains(_color)){
            //         GD.Print(_color + " doesnt belong " + side);
            //         EmitSignal(nameof(die), newPath, level);
            //     } else if(!global.midColors.Contains(_color) && global.midColors.Count >= 2 && !global.colorsData.Contains(_color)){
            //         GD.Print(_color + " shouldnt be " + side);
            //         EmitSignal(nameof(die), newPath, level);
            //     } else if(global.midColors.Contains(_color) && !global.colorsData.Contains(_color)){
            //         EmitSignal(nameof(getScore), side);
            //     }
            // } else {

            //     if(colors == null && global.colorsData.Contains(_color)){
            //         global.colorsData.Remove(_color);
            //         colors.Add(_color);
            //         EmitSignal(nameof(getScore), side);
            //     } else if(!colors.Contains(_color) && colors.Count < 2 && global.colorsData.Contains(_color)){
            //         global.colorsData.Remove(_color);
            //         colors.Add(_color);
            //         EmitSignal(nameof(getScore), side);
            //     } else if(!colors.Contains(_color) && colors.Count < 2 && !global.colorsData.Contains(_color)){
            //         GD.Print("wrong place for " + _color);
            //         EmitSignal(nameof(die), newPath, level);
            //     } else if(!colors.Contains(_color) && colors.Count >= 2 && global.colorsData.Contains(_color)){
            //         GD.Print(_color + " doesnt belong " + side);
            //         EmitSignal(nameof(die), newPath, level);
            //     } else if(!colors.Contains(_color) && colors.Count >= 2 && !global.colorsData.Contains(_color)){
            //         GD.Print(_color + " shouldnt be " + side);
            //         EmitSignal(nameof(die), newPath, level);
            //     } else if(colors.Contains(_color) && !global.colorsData.Contains(_color)){
            //         EmitSignal(nameof(getScore), side);
            //     }
            // }

            

            // if(colors.Contains(_color)){
            //     GD.Print(_color + " goes " + side);
            // }
            // if(global.midColors.Contains(_color)){
            //     GD.Print(_color + " goes " + side);
            // }
        }


    }

    void WaveEasy(int wave, string level, string side, Ball.Colors _color, string newPath, Color _ballColor){
        if(colors == null && global.unusedColorsData.Contains(_color)){
            global.unusedColorsData.Remove(_color);
            colors.Add(_color);
            EmitSignal(nameof(getScore), side);
        } else if(!colors.Contains(_color) && colors.Count < wave && global.unusedColorsData.Contains(_color)){
            global.unusedColorsData.Remove(_color);
            colors.Add(_color);
            EmitSignal(nameof(getScore), side);
        } else if(!colors.Contains(_color) && colors.Count < wave && !global.unusedColorsData.Contains(_color)){
            GD.Print("wrong place for " + _color);
            EmitSignal(nameof(die), newPath, level);
        } else if(!colors.Contains(_color) && colors.Count >= wave && global.unusedColorsData.Contains(_color)){
            GD.Print(_color + " doesnt belong " + side);
            EmitSignal(nameof(die), newPath, level);
        } else if(!colors.Contains(_color) && colors.Count >= wave && !global.unusedColorsData.Contains(_color)){
            GD.Print(_color + " shouldnt be " + side);
            EmitSignal(nameof(die), newPath, level);
        } else if(colors.Contains(_color) && !global.unusedColorsData.Contains(_color)){
                EmitSignal(nameof(getScore), side);
        }

        if(colors.Contains(_color)){
            GD.Print(_color + " goes " + side);
        }

        GD.Print("unused_colors_data : " + global.unusedColorsData);

    }

    void WaveHard(int wave, string level, string side, Ball.Colors _color, string newPath, Color _ballColor){
        if(side == "Middle"){
            if(global.waveCount < 2){
                if(global.midColors == null && global.unusedColorsData.Contains(_color)){
                    global.unusedColorsData.Remove(_color);
                    global.midColors.Add(_color);
                    EmitSignal(nameof(getScore), side);
                } else if(!global.midColors.Contains(_color) && global.midColors.Count < wave && global.unusedColorsData.Contains(_color)){
                    global.unusedColorsData.Remove(_color);
                    global.midColors.Add(_color);
                    EmitSignal(nameof(getScore), side);
                } else if(!global.midColors.Contains(_color) && global.midColors.Count < wave && !global.unusedColorsData.Contains(_color)){
                    GD.Print("wrong place for " + _color);
                    EmitSignal(nameof(die), newPath, level);
                } else if(!global.midColors.Contains(_color) && global.midColors.Count >= wave && global.unusedColorsData.Contains(_color)){
                    GD.Print(_color + " doesnt belong " + side);
                    EmitSignal(nameof(die), newPath, level);
                } else if(!global.midColors.Contains(_color) && global.midColors.Count >= wave && !global.unusedColorsData.Contains(_color)){
                    GD.Print(_color + " shouldnt be " + side);
                    EmitSignal(nameof(die), newPath, level);
                } else if(global.midColors.Contains(_color) && !global.unusedColorsData.Contains(_color)){
                    EmitSignal(nameof(getScore), side);
                }
            } else if(global.waveCount >= 2){
                if(global.midColors == null && global.unusedColorsData.Contains(_color)){
                    global.unusedColorsData.Remove(_color);
                    global.midColors.Add(_color);
                    EmitSignal(nameof(getScore), side);
                } else if(!global.midColors.Contains(_color) && global.midColors.Count < 2 && global.unusedColorsData.Contains(_color)){
                    global.unusedColorsData.Remove(_color);
                    global.midColors.Add(_color);
                    EmitSignal(nameof(getScore), side);
                } else if(!global.midColors.Contains(_color) && global.midColors.Count < 2 && !global.unusedColorsData.Contains(_color)){
                    GD.Print("wrong place for " + _color);
                    EmitSignal(nameof(die), newPath, level);
                } else if(!global.midColors.Contains(_color) && global.midColors.Count >= 2 && global.unusedColorsData.Contains(_color)){
                    GD.Print(_color + " doesnt belong " + side);
                    EmitSignal(nameof(die), newPath, level);
                } else if(!global.midColors.Contains(_color) && global.midColors.Count >= 2 && !global.unusedColorsData.Contains(_color)){
                    GD.Print(_color + " shouldnt be " + side);
                    EmitSignal(nameof(die), newPath, level);
                } else if(global.midColors.Contains(_color) && !global.unusedColorsData.Contains(_color)){
                    EmitSignal(nameof(getScore), side);
                }
            }

            if(colors.Contains(_color)){
                GD.Print(_color + " goes " + side);
            }

        } else {
            if(global.waveCount < 3){
                if(colors == null && global.unusedColorsData.Contains(_color)){
                    global.unusedColorsData.Remove(_color);
                    colors.Add(_color);
                    EmitSignal(nameof(getScore), side);
                } else if(!colors.Contains(_color) && colors.Count < 1 && global.unusedColorsData.Contains(_color)){
                    global.unusedColorsData.Remove(_color);
                    colors.Add(_color);
                    EmitSignal(nameof(getScore), side);
                } else if(!colors.Contains(_color) && colors.Count < 1 && !global.unusedColorsData.Contains(_color)){
                    GD.Print("wrong place for " + _color);
                    EmitSignal(nameof(die), newPath, level);
                } else if(!colors.Contains(_color) && colors.Count >= 1 && global.unusedColorsData.Contains(_color)){
                    GD.Print(_color + " doesnt belong " + side);
                    EmitSignal(nameof(die), newPath, level);
                } else if(!colors.Contains(_color) && colors.Count >= 1 && !global.unusedColorsData.Contains(_color)){
                    GD.Print(_color + " shouldnt be " + side);
                    EmitSignal(nameof(die), newPath, level);
                } else if(colors.Contains(_color) && !global.unusedColorsData.Contains(_color)){
                        EmitSignal(nameof(getScore), side);
                }

                if(colors.Contains(_color)){
                    GD.Print(_color + " goes " + side);
                }
            } else if(global.waveCount >= 3){
                if(colors == null && global.unusedColorsData.Contains(_color)){
                    global.unusedColorsData.Remove(_color);
                    colors.Add(_color);
                    EmitSignal(nameof(getScore), side);
                } else if(!colors.Contains(_color) && colors.Count < 2 && global.unusedColorsData.Contains(_color)){
                    global.unusedColorsData.Remove(_color);
                    colors.Add(_color);
                    EmitSignal(nameof(getScore), side);
                } else if(!colors.Contains(_color) && colors.Count < 2 && !global.unusedColorsData.Contains(_color)){
                    GD.Print("wrong place for " + _color);
                    EmitSignal(nameof(die), newPath, level);
                } else if(!colors.Contains(_color) && colors.Count >= 2 && global.unusedColorsData.Contains(_color)){
                    GD.Print(_color + " doesnt belong " + side);
                    EmitSignal(nameof(die), newPath, level);
                } else if(!colors.Contains(_color) && colors.Count >= 2 && !global.unusedColorsData.Contains(_color)){
                    GD.Print(_color + " shouldnt be " + side);
                    EmitSignal(nameof(die), newPath, level);
                } else if(colors.Contains(_color) && !global.unusedColorsData.Contains(_color)){
                        EmitSignal(nameof(getScore), side);
                }

                if(colors.Contains(_color)){
                    GD.Print(_color + " goes " + side);
                }
            }
        }
        

        GD.Print("unused_colors_data : " + global.unusedColorsData);

    }

}
