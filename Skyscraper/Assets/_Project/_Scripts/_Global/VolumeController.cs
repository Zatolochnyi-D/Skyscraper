using ThreeDent.DevelopmentTools;
using ThreeDent.Helpers.Utils;
using UnityEngine;
using UnityEngine.Audio;

public class VolumeController : Singleton<VolumeController>
{
    [SerializeField] private AudioMixer mixer;

    private const string SoundKey = "Sound";
    public float SoundVolume
    {
        get => PlayerPrefs.GetFloat(SoundKey, 1f);
        set
        {
            PlayerPrefs.SetFloat(SoundKey, value);
            mixer.SetFloat("SoundVolume", AudioUtils.NormalizedVolumeToAttenuation(value));
        }
    }

    private const string MusicKey = "Music";
    public float MusicVolume
    {
        get => PlayerPrefs.GetFloat(MusicKey, 1f);
        set
        {
            PlayerPrefs.SetFloat(MusicKey, value);
            mixer.SetFloat("MusicVolume", AudioUtils.NormalizedVolumeToAttenuation(value));
        }
    }

    protected override void Awake()
    {
        mixer.SetFloat("SoundVolume", AudioUtils.NormalizedVolumeToAttenuation(SoundVolume));
        mixer.SetFloat("MusicVolume", AudioUtils.NormalizedVolumeToAttenuation(MusicVolume));
    }
}