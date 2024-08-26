using UnityEngine;

namespace CheeseMods.CheeseDebugTools.CheeseDebugModules
{
    public class CheeseDebugModule_Help : CheeseDebugModule
    {
        public string helpString = @"Welcome to Cheese's Debug Mod!
If you need any help, ask me on the VTOL VR Modding Discord.


Insert Camera Controls (these can be used without my Debug mod):

Insert - Enables/Disables the insert camera
Right click and scroll - move the camera

[ - Next unit
] - Previous unit
. (>) - Increase timescale
, (<) - Decrease timescale
/ - Sets the time scale to 1
Tab - Enables / Disables debug gun (shoots with left click)
v - Toggles camera mode between free and chase

Camera Modifier - Left Shift
While holding down left shift, scrolling changes the FOV, and right click and dragging rotates the camera

Delete - End mission
End - Reload mission

Insert Camera Controls+ (these are from my Debug mod):

Middle click - Select unit";

        public CheeseDebugModule_Help(string name, KeyCode keyCode) : base(name, keyCode)
        {

        }

        protected override void WindowFunction(int windowID)
        {
            GUI.Label(new Rect(20, 20, 360, 460), helpString);
            GUI.DragWindow(new Rect(0, 0, 10000, 10000));
        }

        public override void Enable()
        {
            base.Enable();
            windowRect = new Rect(20, 20, 400, 500);
        }
    }
}