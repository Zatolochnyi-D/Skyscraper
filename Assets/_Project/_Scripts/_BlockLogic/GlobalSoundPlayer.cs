using System.Collections.Generic;
using System.Linq;
using ThreeDent.DevelopmentTools;
using ThreeDent.DevelopmentTools.Option;
using ThreeDent.Helpers.Extensions;
using UnityEngine;

public class GlobalSoundPlayer : Singleton<GlobalSoundPlayer>
{
    [SerializeField] private AudioClip[] hitSounds;
    [SerializeField] private float soundDistance = 10f;
    [SerializeField] private float volume = 0.8f;
    [SerializeField] private float velocityCutoff = 0.002f;

    private Queue<Collision2D> collisions = new();
    private int counter = 0;

    private void Update()
    {
        var colls = new Collision2D[counter];
        for (int i = 0; i < counter; i++)
        {
            colls[i] = collisions.Dequeue();
        }
        counter = 0;

        colls.Distinct().Where(x => x.relativeVelocity.magnitude > velocityCutoff).ForEach(x => AudioPlayer.Instance.PlayDistant2d(hitSounds.PeekItem(), x.contacts[0].point, volume, soundDistance));
    }

    public void RegisterCollision(Collision2D collision)
    {
        collisions.Enqueue(collision);
        counter++;
    }
}
