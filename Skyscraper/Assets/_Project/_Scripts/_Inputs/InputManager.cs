using System;
using UnityEngine;
using ThreeDent.DevelopmentTools;

namespace Skyscraper.Inputs
{
    public enum InputModes
    {
        Game,
        Menu,
    }

    public class InputManager : Singleton<InputManager>
    {
        [SerializeField] private AnimationCurve acceleration;

        public static event Action<Vector2> OnCameraMovement;
        public static event Action<float> OnMovement;
        public static event Action<float> OnRotation;
        public static event Action<float> OnScroll;
        public static event Action OnSpeedupStarted;
        public static event Action OnSpeedupCanceled;
        public static event Action OnCyclePressed;
        public static event Action OnPausePerformed;
        public static event Action OnMenuBackPerformed;

        [SerializeField] private MouseOnEdgeDetector onEdgeDetector;

        private Inputs inputActions;
        private float moveHoldTime = 0f;
        private float rotationHoldTime = 0f;

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
            {
                moveHoldTime += Time.deltaTime;
                OnMovement?.Invoke(movementValue * acceleration.Evaluate(moveHoldTime));
            }
            else
            {
                moveHoldTime = 0f;
            }

            var rotationValue = inputActions.Game.Rotation.ReadValue<float>();
            if (rotationValue != 0f)
            {
                rotationHoldTime += Time.deltaTime;
                OnRotation?.Invoke(rotationValue * acceleration.Evaluate(rotationHoldTime));
            }
            else
            {
                rotationHoldTime = 0f;
            }

            var scrollValue = inputActions.Game.Scroll.ReadValue<float>();
            if (scrollValue != 0)
                OnScroll?.Invoke(scrollValue);

            inputActions.Game.SpeedupFall.started += (_) => OnSpeedupStarted?.Invoke();
            inputActions.Game.SpeedupFall.canceled += (_) => OnSpeedupCanceled?.Invoke();
            inputActions.Game.CycleSelection.performed += (_) => OnCyclePressed?.Invoke();
            inputActions.Game.Pause.performed += (_) => OnPausePerformed?.Invoke();

            inputActions.Menu.Back.performed += (_) => OnMenuBackPerformed?.Invoke();
        }

        public static void SwitchMode(InputModes mode)
        {
            switch (mode)
            {
                case InputModes.Game:
                    Instance.inputActions.Disable();
                    Instance.inputActions.Game.Enable();
                    break;
                case InputModes.Menu:
                    Instance.inputActions.Disable();
                    Instance.inputActions.Menu.Enable();
                    break;
            }
        }
    }
}