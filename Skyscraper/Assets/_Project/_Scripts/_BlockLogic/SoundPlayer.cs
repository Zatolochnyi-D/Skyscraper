using ThreeDent.DevelopmentTools.Attributes;
using ThreeDent.DevelopmentTools.Option;
using ThreeDent.Helpers.Extensions;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    [OnThis, SerializeField] private BlockLandingDetector landingDetector;
    [SerializeField] private AudioClip[] hitSounds;
    // [SerializeField] private AudioClip fallSound;
    [SerializeField] private LayerMask ignoreCollisionLayers;
    [SerializeField] private float maxSpeedOfWeakSound = 5f;
    [SerializeField] private float maxSpeedOfMediumSound = 15f;
    [SerializeField] private float soundDistance = 20f;
    [SerializeField] private float strongVolume = 1f;
    [SerializeField] private float mediumVolume = 0.75f;
    [SerializeField] private float weakVolume = 0.45f;

    private void Awake()
    {
        landingDetector.OnBlockFirstCollision += PlayFirstSound;
    }
    
    private void PlayFirstSound(Collision2D collision)
    {
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
        {
            GlobalSoundPlayer.Instance.RegisterCollision(collision);
        }
    }
}