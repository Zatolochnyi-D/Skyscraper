using UnityEngine;
using UnityEngine.UI;

public class SettingsUi : MonoBehaviour
{
    [SerializeField] private Button back;
    [SerializeField] private Slider soundSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private MainMenuUi mainMenu;

    private void Awake()
    {
        Hide();
        back.onClick.AddListener(() =>
        {
            Hide();
            mainMenu.Show();
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
