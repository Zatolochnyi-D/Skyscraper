using UnityEngine;
using ThreeDent.Helpers.Extensions;
using UnityEngine.Audio;

namespace ThreeDent.DevelopmentTools.Option
{
    public class AudioPlayer : Singleton<AudioPlayer>
    {
        [SerializeField] private GameObject globalSource;
        [SerializeField] private GameObject distantSource2d;
        [SerializeField] private AudioMixerGroup soundGroup;
        [SerializeField] private AudioMixerGroup musicGroup;

        private GameObjectPool globalSourcePool;
        private GameObjectPool distantSource2dPool;

        protected override void Awake()
        {
            base.Awake();

            globalSourcePool = new(globalSource, parent: transform);
            distantSource2dPool = new(distantSource2d, parent: transform);
        }

        public void PlayGlobal(AudioClip audio, float localVolume = 1f)
        {
            var sourceObject = globalSourcePool.Get();
            sourceObject.SetActive(true);
            var sourceComponent = sourceObject.GetComponent<AudioSource>();
            sourceComponent.clip = audio;
            sourceComponent.volume = localVolume;
            sourceComponent.outputAudioMixerGroup = soundGroup;
            sourceComponent.Play();
            this.InvokeOnce(() => sourceObject.SetActive(false), audio.length);
        }

        // Sets audio source on position coordinates with z equal to camera z. Works fine with default set up (where z is depth axis). 
        public void PlayDistant2d(AudioClip audio, Vector2 position, Option<Camera> camera, float localVolume = 1f, float maxDistance = 100f)
        {
            var sourceObject = distantSource2dPool.Get();
            sourceObject.SetActive(true);
            sourceObject.transform.position = ((Vector3)position).With(z: camera.DefaultWith(Camera.main).transform.position.z);
            var sourceComponent = sourceObject.GetComponent<AudioSource>();
            sourceComponent.clip = audio;
            sourceComponent.volume = localVolume;
            sourceComponent.maxDistance = maxDistance;
            sourceComponent.outputAudioMixerGroup = soundGroup;
            sourceComponent.Play();
            this.InvokeOnce(() => sourceObject.SetActive(false), audio.length);
        }

        public void PlayDistant2d(AudioClip audio, Vector2 position, float localVolume = 1f, float maxDistance = 100f)
        {
            PlayDistant2d(audio, position, Option.None<Camera>(), localVolume, maxDistance);
        }

        public AudioSource BorrowSource()
        {
            return distantSource2dPool.Get().GetComponent<AudioSource>();
        }

        public void ReleaseSource(AudioSource source)
        {
            source.transform.parent = transform;
            source.gameObject.SetActive(false);
        }
    }
}