using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class CheeseDebugModule_Help : CheeseDebugModule
{
    public CheeseDebugModule_Help(string name, KeyCode keyCode) : base(name, keyCode)
    {

    }

    public override void GetDebugText(ref string debugString, Actor actor)
    {
        debugString += "Welcome to Cheese's AI Debug Mod!";
        debugString += "\n";
        debugString += "If you need any help, ask me on the VTOL VR Modding Discord.";

        debugString += "\n\n";

        debugString += "Insert Camera Controls (these can be used without the AI Debug mod):";
        debugString += "\n";
        debugString +=
@"Insert - Enables/Disables the insert camera
Right click and scroll - move the camera

[ - Next unit
] - Previous unit
. (>) - Increase timescale
, (<) - Decrease timescale
/ - Sets the time scale to 1
Tab - Enables / Disables debug gun (shoots with left click)
v - Toggles camera mode between free and chase

Camera Modifier - Left Shift
While holding down left shift, scrolling changes the FOV, and right click and dragging rotates the camera";
    }
}
