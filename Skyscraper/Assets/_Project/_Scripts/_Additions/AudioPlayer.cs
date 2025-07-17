using UnityEngine;
using ThreeDent.Helpers.Extensions;

namespace ThreeDent.DevelopmentTools.Option
{
    public class AudioPlayer : Singleton<AudioPlayer>
    {
        [SerializeField] private GameObject globalSource;
        [SerializeField] private GameObject distantSource2d;

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
            var sourceScript = sourceObject.GetComponent<AudioSource>();
            sourceScript.clip = audio;
            sourceScript.volume = localVolume;
            sourceScript.Play();
            this.InvokeOnce(() => sourceObject.SetActive(false), audio.length);
        }

        // Sets audio source on position coordinates with z equal to camera z. Works fine with default set up (where z is depth axis). 
        public void PlayDistant2d(AudioClip audio, Vector2 position, Option<Camera> camera, float localVolume = 1f, float maxDistance = 100f)
        {
            var sourceObject = distantSource2dPool.Get();
            sourceObject.SetActive(true);
            sourceObject.transform.position = ((Vector3)position).With(z: camera.DefaultWith(Camera.main).transform.position.z);
            var sourceScript = sourceObject.GetComponent<AudioSource>();
            sourceScript.clip = audio;
            sourceScript.volume = localVolume;
            sourceScript.maxDistance = maxDistance;
            sourceScript.Play();
            this.InvokeOnce(() => sourceObject.SetActive(false), audio.length);
        }

        public void PlayDistant2d(AudioClip audio, Vector2 position, float localVolume = 1f, float maxDistance = 100f)
        {
            PlayDistant2d(audio, position, Option.None<Camera>(), localVolume, maxDistance);
        }
    }
}