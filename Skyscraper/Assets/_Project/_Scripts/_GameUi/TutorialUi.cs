using Skyscraper.Inputs;
using UnityEngine;
using UnityEngine.UI;

public class TutorialUi : MonoBehaviour
{
    private const string DontShowAgainSave = "DNSA";

    [SerializeField] private Toggle dontShowAgainToggle;
    [SerializeField] private Button hideButton;

    private void Awake()
    {
        hideButton.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
            Time.timeScale = 1f;
            InputManager.SwitchMode(InputModes.Game);
            PlayerPrefs.SetInt(DontShowAgainSave, dontShowAgainToggle.isOn ? 1 : 0);
        });
    }

    private void Start()
    {
        if (PlayerPrefs.GetInt(DontShowAgainSave, 0) == 0)
        {
            Time.timeScale = 0f;
            InputManager.SwitchMode(InputModes.None);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
