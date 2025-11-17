using ThreeDent.DevelopmentTools.Attributes;
using ThreeDent.DevelopmentTools.Option;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSoundOnClick : MonoBehaviour
{
    [OnThis, SerializeField] private Button button;
    [SerializeField] private AudioClip sound;

    private void Awake()
    {
        button.onClick.AddListener(() => AudioPlayer.Instance.PlayGlobal(sound));
    }
}