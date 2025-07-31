using ThreeDent.SceneManagement;
using ThreeDent.SceneManagement.Operations;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUi : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private SettingsUi settings;

    void Awake()
    {
        startButton.onClick.AddListener(() =>
        {
            ScenesManager.LoadSequenceAsync(new LoadOperation(3), new UnloadOperation(2), new JumpWaitOperation(1f), new LoadOperation(1), new UnloadOperation(3));
        });
        settingsButton.onClick.AddListener(() =>
        {
            Hide();
            settings.Show();
        });
        quitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
