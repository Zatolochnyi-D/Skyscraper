using System;
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

        public static event Action<float> OnSideMovement;

        protected override void Awake()
        {
            base.Awake();
            inputs = new();
        }

        private void Update()
        {
            var sideMovementValue = inputs.Game.SideMovement.ReadValue<float>();
            if (sideMovementValue != 0f)
                OnSideMovement?.Invoke(sideMovementValue);
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