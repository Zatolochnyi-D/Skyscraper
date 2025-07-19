using ThreeDent.DevelopmentTools.Attributes;
using ThreeDent.DevelopmentTools.Option;
using ThreeDent.Helpers.Extensions;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    [OnThis, SerializeField] private BlockLandingDetector landingDetector;
    [SerializeField] private AudioClip[] hitSounds;
    [SerializeField] private AudioClip fallSound;
    [SerializeField] private LayerMask ignoreCollisionLayers;
    [SerializeField] private float maxSpeedOfWeakSound = 5f;
    [SerializeField] private float maxSpeedOfMediumSound = 15f;
    [SerializeField] private float soundDistance = 20f;
    [SerializeField] private float strongVolume = 1f;
    [SerializeField] private float mediumVolume = 0.75f;
    [SerializeField] private float weakVolume = 0.45f;
    [SerializeField] private float fallSoundDistance = 50f;

    private AudioSource borrowedSource;

    private void Awake()
    {
        landingDetector.OnBlockFirstCollision += PlayFirstSound;
    }

    private void OnEnable()
    {
        borrowedSource = AudioPlayer.Instance.BorrowSource();
        borrowedSource.gameObject.SetActive(true);
        borrowedSource.clip = fallSound;
        borrowedSource.loop = true;
        borrowedSource.maxDistance = fallSoundDistance;
        borrowedSource.transform.position = new Vector3(0f, 0f, Camera.main.transform.position.z);
        borrowedSource.transform.parent = transform;
        borrowedSource.Play();
    }

    private void PlayFirstSound(Collision2D collision)
    {
        borrowedSource.Stop();
        borrowedSource.loop = false;
        AudioPlayer.Instance.ReleaseSource(borrowedSource);
        borrowedSource = null;
        
        var velocity = collision.relativeVelocity.magnitude;
        float volume;
        if (velocity >= maxSpeedOfMediumSound)
            volume = strongVolume;
        else if (velocity >= maxSpeedOfWeakSound)
            volume = mediumVolume;
        else
            volume = weakVolume;
        AudioPlayer.Instance.PlayDistant2d(hitSounds.PeekItem(), transform.position, volume, soundDistance);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.otherCollider.gameObject.layer != ignoreCollisionLayers)
            GlobalSoundPlayer.Instance.RegisterCollision(collision);
    }
}