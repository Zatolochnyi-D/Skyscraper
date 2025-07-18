using ThreeDent.DevelopmentTools.Option;
using ThreeDent.Helpers.Extensions;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    // [SerializeField] private AudioClip[] hitSounds;
    // [SerializeField] private AudioClip fallSound;
    [SerializeField] private LayerMask ignoreCollisionLayers;

    // void Start()
    // {
    //     this.InvokeOnce(() => AudioPlayer.Instance.PlayDistant2d(hitSounds[0], Vector2.left * 9f, maxDistance: 10f), 2f);
    //     this.InvokeOnce(() => AudioPlayer.Instance.PlayDistant2d(hitSounds[0], Vector2.zero, maxDistance: 10f), 3f);
    //     this.InvokeOnce(() => AudioPlayer.Instance.PlayDistant2d(hitSounds[0], Vector2.right * 9f, maxDistance: 10f), 4f);
    // }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.otherCollider.gameObject.layer != ignoreCollisionLayers)
        {
            Debug.Log("Block contact");
        }
    }
}