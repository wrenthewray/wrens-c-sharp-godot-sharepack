namespace Shared.Models.Data
{
    /// <summary>
    /// A class that defines data to be used when saving the input map.
    /// </summary>
    public class InputData
    {
        public InputType inputType;
        public int buttonIndex;

        public InputData() { }
        public InputData(InputType inputType, int buttonIndex)
        {
            this.inputType = inputType;
            this.buttonIndex = buttonIndex;
        }
    }
    public enum InputType
    {
        Invalid,
        Joypad,
        Keyboard,
        Mouse,
        JoyAxisLeftX,
        JoyAxisLeftY,
        JoyAxisRightX,
        JoyAxisRightY,
        TriggerX,
        TriggerY,
    }
}