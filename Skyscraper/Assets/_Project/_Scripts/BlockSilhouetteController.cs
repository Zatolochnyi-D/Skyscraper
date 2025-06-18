using System.Collections;
using ThreeDent.EventBroker;
using ThreeDent.Helpers.Extensions;
using UnityEngine;

public class BlockSilhouetteController : MonoBehaviour
{
    [SerializeField] private int silhoetteIndex;
    [SerializeField] private Collider2D blockCollider;

    private GameObject silhoette;

    void Awake()
    {
        EventBroker.Subscribe<BlockLandedEvent>(Hide);
    }

    private IEnumerator SilhoetteUpdateCycle()
    {
        while (true)
        {
            silhoette.transform.eulerAngles = transform.eulerAngles;

            var raycast = new RaycastHit2D[1];
            blockCollider.Cast(Vector2.down, raycast);
            silhoette.transform.position = transform.position.With(y: 0) + Vector3.zero.With(y: transform.position.y - raycast[0].distance);

            yield return null;
        }
    }

    private void Hide(BlockLandedEvent args)
    {
        if (args.sender == gameObject)
        {
            StopAllCoroutines();
            silhoette.SetActive(false);
        }
    }

    public void Show()
    {
        silhoette = SilhoettePool.GetSilhoette(silhoetteIndex);
        silhoette.SetActive(true);
        StartCoroutine(SilhoetteUpdateCycle());
    }
}
