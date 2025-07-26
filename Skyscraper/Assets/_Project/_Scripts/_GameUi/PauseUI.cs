using Skyscraper.Inputs;
using ThreeDent.DevelopmentTools.Attributes;
using UnityEngine;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    [OnChild, SerializeField] private Button resume;
    [OnChild(1), SerializeField] private Button toMainMenu;
    [SerializeField] private Button pause;

    private void Awake()
    {
        gameObject.SetActive(false);

        pause.onClick.AddListener(Pause);
        resume.onClick.AddListener(Unpause);

        InputManager.OnPausePerformed += Pause;
        InputManager.OnMenuBackPerformed += Unpause;
    }

    private void OnDestroy()
    {
        InputManager.OnPausePerformed -= Pause;
        InputManager.OnMenuBackPerformed -= Unpause;
    }

    private void Pause()
    {
        gameObject.SetActive(true);
        Time.timeScale = 0f;
        InputManager.SwitchMode(InputModes.Menu);
    }

    private void Unpause()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1f;
        InputManager.SwitchMode(InputModes.Game);
    }
}
