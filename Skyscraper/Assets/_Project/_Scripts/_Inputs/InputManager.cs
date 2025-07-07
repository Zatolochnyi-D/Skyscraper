using System;
using UnityEngine;
using UnityEngine.InputSystem;
using ThreeDent.DevelopmentTools;

namespace Skyscraper.Inputs
{
    public enum InputModes
    {
        Game,
    }

    public class InputManager : Singleton<InputManager>
    {
        public static event Action<Vector2> OnCameraMovement;
        public static event Action<float> OnMovement;
        public static event Action<float> OnRotation;
        public static event Action<float> OnScroll;
        public static event Action OnSpeedupStarted;
        public static event Action OnSpeedupCanceled;

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

            var scrollValue = inputActions.Game.Scroll.ReadValue<float>();
            if (scrollValue != 0)
                OnScroll?.Invoke(scrollValue);

            inputActions.Game.SpeedupFall.started += (_) => OnSpeedupStarted?.Invoke();
            inputActions.Game.SpeedupFall.canceled += (_) => OnSpeedupCanceled?.Invoke();
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