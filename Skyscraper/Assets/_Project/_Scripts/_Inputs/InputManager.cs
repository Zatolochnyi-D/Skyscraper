using ThreeDent.Helpers.Tools;
using UnityEngine;

namespace Skyscraper.Inputs
{
    public enum InputModes
    {
        Game,
    }

    public class InputManager : Singleton<InputManager>
    {
        private Inputs inputs;

        protected override void Awake()
        {
            base.Awake();
            inputs = new();
        }

        public static void SwitchMode(InputModes mode)
        {
            switch (mode)
            {
                case InputModes.Game:
                    Instance.inputs.Disable();
                    Instance.inputs.Game.Enable();
                    break;
            }
        }
    }
}