using UnityEngine;
using ThreeDent.EventBroker;
using System.Collections;
using System;
using ThreeDent.DevelopmentTools.Attributes;
using ThreeDent.DevelopmentTools.Option;

public class BlockLandingDetector : MonoBehaviour
{
    public event Action OnBlockLanded;
    public event Action<Collision2D> OnBlockFirstCollision;

    [OnThis, SerializeField] private Rigidbody2D physicalBody;
    [OnThis, SerializeField] private BlockSilhouetteController silhouetteController;
    [SerializeField] private float speedEpsilon = 0.0002f;
    [SerializeField] private float timeToStandNotMoved = 4f;
    [SerializeField] private LayerMask ignoreCollisionLayer;

    private bool firstCollisionHappened;
    private Coroutine checkLandCoroutine;

    private void Awake()
    {
        EventBroker.Subscribe<ForceBlockSpawnEvent>(DisableCheck);
    }

    private void OnDestroy()
    {
        EventBroker.Unsubscribe<ForceBlockSpawnEvent>(DisableCheck);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!firstCollisionHappened && collision.collider.gameObject.layer.NotIn(ignoreCollisionLayer))
        {
            // Should initiate object movement check to ensure it actually landed and not just accidentaly touched other object.
            firstCollisionHappened = true;
            silhouetteController.Hide();
            ActiveBlockManager.Instance.RemoveActiveBlock();
            checkLandCoroutine = StartCoroutine(MovementCheckCycle());
            OnBlockFirstCollision?.Invoke(collision);
            EventBroker.Invoke<BlockFirstCollisionEvent>();
        }
    }

    private void DisableCheck()
    {
        if (checkLandCoroutine != null)
            StopCoroutine(checkLandCoroutine);
        TriggerBlockLand();
    }

    private IEnumerator MovementCheckCycle()
    {
        float timeElapsed = 0f;
        while (true)
        {
            yield return null;

            float currentSpeed = physicalBody.linearVelocity.magnitude + physicalBody.angularVelocity;
            if (currentSpeed <= speedEpsilon)
                timeElapsed += Time.deltaTime;
            if (timeElapsed >= timeToStandNotMoved)
                break;
        }
        TriggerBlockLand();
    }

    private void TriggerBlockLand()
    {   
        OnBlockLanded?.Invoke();
        EventBroker.Invoke<BlockLandedEvent>();
    }
}

public class BlockFirstCollisionEvent : IEvent { }
public class BlockLandedEvent : IEvent { }