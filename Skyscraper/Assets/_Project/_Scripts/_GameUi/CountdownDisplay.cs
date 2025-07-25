using ThreeDent.DevelopmentTools.Attributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CountdownDisplay : MonoBehaviour
{
    [OnChild, SerializeField] private Image imageDisplay;
    [OnChild, SerializeField] private TextMeshProUGUI textDisplay;

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

    private void UpdateDisplay(float time, float normalizedTime)
    {
        imageDisplay.fillAmount = normalizedTime;
        textDisplay.text = $"{Mathf.Ceil(time)}";
    }

    private void Show()
    {
        imageDisplay.gameObject.SetActive(true);
        textDisplay.gameObject.SetActive(true);
    }

    private void Hide()
    {
        imageDisplay.gameObject.SetActive(false);
        textDisplay.gameObject.SetActive(false);
    }
}
