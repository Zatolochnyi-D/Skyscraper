using ThreeDent.DevelopmentTools.Attributes;
using ThreeDent.DevelopmentTools.Option;
using ThreeDent.Helpers.Extensions;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip[] hitSounds;
    // [SerializeField] private AudioClip fallSound;
    [OnThis, SerializeField] private BlockLandingDetector landingDetector;
    [SerializeField] private LayerMask ignoreCollisionLayers;
    [SerializeField] private float maxSpeedOfWeakSound = 5f;
    [SerializeField] private float maxSpeedOfMediumSound = 15f;
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
        if (velocity >= maxSpeedOfMediumSound)
            AudioPlayer.Instance.PlayGlobal(hitSounds.PeekItem(), strongVolume);
        else if (velocity >= maxSpeedOfWeakSound)
            AudioPlayer.Instance.PlayGlobal(hitSounds.PeekItem(), mediumVolume);
        else
            AudioPlayer.Instance.PlayGlobal(hitSounds.PeekItem(), weakVolume);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.otherCollider.gameObject.layer != ignoreCollisionLayers)
        {
            
        }
    }
}