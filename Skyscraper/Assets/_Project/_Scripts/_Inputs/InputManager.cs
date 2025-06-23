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

        public static event Action<float> OnMovement;
        public static event Action<float> OnRotation;
        public static event Action OnSpeedup;

        protected override void Awake()
        {
            base.Awake();
            inputs = new();
        }

        private void Update()
        {
            var movementValue = inputs.Game.Movement.ReadValue<float>();
            if (movementValue != 0f)
                OnMovement?.Invoke(movementValue);

            var rotationValue = inputs.Game.Rotation.ReadValue<float>();
            if (rotationValue != 0f)
                OnRotation?.Invoke(rotationValue);

            var speedupValue = inputs.Game.SpeedupFall.ReadValue<float>();
            if (speedupValue != 0f)
                OnSpeedup?.Invoke();
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