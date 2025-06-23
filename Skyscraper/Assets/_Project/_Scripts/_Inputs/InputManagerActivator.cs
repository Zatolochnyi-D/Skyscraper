using UnityEngine;

namespace Skyscraper.Inputs
{
    public class InputManagerActivator : MonoBehaviour
    {
        [SerializeField] private InputModes mode;
        
        private void Awake()
        {
            InputManager.SwitchMode(mode);
        }
    }
}
