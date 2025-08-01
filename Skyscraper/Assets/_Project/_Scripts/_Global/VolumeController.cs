using ThreeDent.DevelopmentTools;
using ThreeDent.Helpers.Utils;
using UnityEngine;
using UnityEngine.Audio;

public class VolumeController : Singleton<VolumeController>
{
    [SerializeField] private AudioMixer mixer;
        
    private const string SoundKey = "Sound";
    public float SoundVolume { get => PlayerPrefs.GetFloat(SoundKey, 1f); set => SetSound(value); }

    private void SetSound(float value)
    {
        PlayerPrefs.SetFloat(SoundKey, value);
        mixer.SetFloat("SoundVolume", AudioUtils.NormalizedVolumeToAttenuation(value));
    }
}