using ThreeDent.DevelopmentTools.Attributes;
using TMPro;
using UnityEngine;

public class CountdownDisplay : MonoBehaviour
{
    [OnChild, SerializeField] private TextMeshProUGUI display;

    private void Awake()
    {
        Hide();
    }

    private void Start()
    {
        BlocksSpawner.Instance.OnCountdownStart += Show;
        BlocksSpawner.Instance.OnCountdown += UpdateDisplay;
        BlocksSpawner.Instance.OnCountdownEnd += Hide;
    }

    private void UpdateDisplay(float time)
    {
        display.text = $"{Mathf.Ceil(time)}";
    }

    private void Show()
    {
        display.gameObject.SetActive(true);
    }

    private void Hide()
    {
        display.gameObject.SetActive(false);
    }
}
