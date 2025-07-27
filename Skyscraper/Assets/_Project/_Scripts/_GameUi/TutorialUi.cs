using Skyscraper.Inputs;
using UnityEngine;
using UnityEngine.UI;

public class TutorialUi : MonoBehaviour
{
    [SerializeField] private Button hideButton;

    private void Awake()
    {
        hideButton.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
            Time.timeScale = 1f;
            InputManager.SwitchMode(InputModes.Game);
        });
    }

    private void Start()
    {
        Time.timeScale = 0f;
        InputManager.SwitchMode(InputModes.None);
    }
}
