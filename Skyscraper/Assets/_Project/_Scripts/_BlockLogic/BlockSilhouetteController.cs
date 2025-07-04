using System.Collections;
using ThreeDent.Helpers.Extensions;
using UnityEngine;

public class BlockSilhouetteController : MonoBehaviour
{
    [OnThis, SerializeField] private Collider2D blockCollider;
    [OnThis, SerializeField] private BlockLandingDetector landingDetector;
    [SerializeField] private int silhoetteIndex;
    [SerializeField] private LayerMask layersToCastSilhouetteOn;   

    private GameObject silhoette;

    void Awake()
    {
        landingDetector.OnBlockLanded += Hide;
    }

    private IEnumerator SilhoetteUpdateCycle()
    {
        while (true)
        {
            silhoette.transform.eulerAngles = transform.eulerAngles;

            var raycast = new RaycastHit2D[1];
            var filter = new ContactFilter2D { layerMask = layersToCastSilhouetteOn };
            blockCollider.Cast(Vector2.down, filter, raycast);
            silhoette.transform.position = transform.position.With(y: 0) + Vector3.zero.With(y: transform.position.y - raycast[0].distance);

            yield return null;
        }
    }

    public void Show()
    {
        silhoette = SilhoettePool.GetSilhoette(silhoetteIndex);
        silhoette.SetActive(true);
        StartCoroutine(SilhoetteUpdateCycle());
    }

    private void Hide()
    {
        StopAllCoroutines();
        silhoette.SetActive(false);
        silhoette = null;
    }
}
