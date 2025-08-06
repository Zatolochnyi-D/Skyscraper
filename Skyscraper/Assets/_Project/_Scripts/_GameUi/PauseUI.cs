using Skyscraper.Inputs;
using ThreeDent.DevelopmentTools.Attributes;
using ThreeDent.EventBroker;
using ThreeDent.SceneManagement;
using ThreeDent.SceneManagement.Operations;
using UnityEngine;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    [OnChild, SerializeField] private Button resume;
    [OnChild(1), SerializeField] private Button restartButton;
    [OnChild(2), SerializeField] private Button toMainMenu;
    [SerializeField] private Button pause;
    [SerializeField] private Slider soundSlider;
    [SerializeField] private Slider musicSlider;

    private void Awake()
    {
        gameObject.SetActive(false);

        pause.onClick.AddListener(Pause);
        resume.onClick.AddListener(Unpause);
        restartButton.onClick.AddListener(() =>
        {
            Time.timeScale = 1f;
            EventBroker.Invoke<SceneSwitchEvent>();
            ScenesManager.LoadSequenceAsync(new UnloadOperation(1), new LoadOperation(3), new LoadOperation(1), new UnloadOperation(3));
        });
        toMainMenu.onClick.AddListener(() =>
        {
            Time.timeScale = 1f;
            EventBroker.Invoke<SceneSwitchEvent>();
            ScenesManager.LoadSequenceAsync(new UnloadOperation(1), new LoadOperation(3), new LoadOperation(2), new UnloadOperation(3));
        });
        soundSlider.value = VolumeController.Instance.SoundVolume;
        soundSlider.onValueChanged.AddListener(value =>
        {
            VolumeController.Instance.SoundVolume = value;
        });
        musicSlider.value = VolumeController.Instance.MusicVolume;
        musicSlider.onValueChanged.AddListener(value =>
        {
            VolumeController.Instance.MusicVolume = value;
        });

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

public class SceneSwitchEvent : IEvent { }