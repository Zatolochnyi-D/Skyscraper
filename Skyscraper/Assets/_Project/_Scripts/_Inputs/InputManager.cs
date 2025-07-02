using System;
using ThreeDent.Helpers.Tools;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Skyscraper.Inputs
{
    public enum InputModes
    {
        Game,
    }

    public abstract class InputManager<TInputActions> : Singleton<InputManager<TInputActions>> where TInputActions : IInputActionCollection2, IDisposable, new()
    {
        protected TInputActions inputActions;

        protected override void Awake()
        {
            base.Awake();
            inputActions = new();
        }
    }

    public class InputManager : Singleton<InputManager>
    {
        public static event Action<Vector2> OnCameraMovement;
        public static event Action<float> OnMovement;
        public static event Action<float> OnRotation;
        public static event Action OnSpeedup;

        [SerializeField] private MouseOnEdgeDetector onEdgeDetector;

        private Inputs inputActions;

        protected override void Awake()
        {
            base.Awake();
            inputActions = new();
        }

        private void Update()
        {
            var cameraMovementValue = inputActions.Game.CameraMovement.ReadValue<Vector2>();
            cameraMovementValue = cameraMovementValue != Vector2.zero ? cameraMovementValue : onEdgeDetector.DirectionVector;
            if (cameraMovementValue != Vector2.zero)
                OnCameraMovement?.Invoke(cameraMovementValue);

            var movementValue = inputActions.Game.Movement.ReadValue<float>();
            if (movementValue != 0f)
                OnMovement?.Invoke(movementValue);

            var rotationValue = inputActions.Game.Rotation.ReadValue<float>();
            if (rotationValue != 0f)
                OnRotation?.Invoke(rotationValue);

            var speedupValue = inputActions.Game.SpeedupFall.ReadValue<float>();
            if (speedupValue != 0f)
                OnSpeedup?.Invoke();
        }

        public static void SwitchMode(InputModes mode)
        {
            switch (mode)
            {
                case InputModes.Game:
                    Instance.inputActions.Disable();
                    Instance.inputActions.Game.Enable();
                    break;
            }
        }
    }
}