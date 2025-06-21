using UnityEngine;
using ThreeDent.EventBroker;
using System.Collections;
using System;

public class BlockLandingDetector : MonoBehaviour
{
    private static readonly BlockLandedEvent eventArgs = new();

    public event Action OnBlockLanded;

    [OnThis, SerializeField] private Rigidbody2D physicalBody;
    [SerializeField] private float speedEpsilon = 0.0002f;
    [SerializeField] private float timeToStandNotMoved = 4f;

    private bool firstCollisionHappened;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!firstCollisionHappened)
        {
            // Object hit other object for the first time.
            // Should initiate object movement check to ensure it actually landed and not just hit other object.
            firstCollisionHappened = true;
            StartCoroutine(MovementCheckCycle());
        }
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
        eventArgs.sender = gameObject;
        OnBlockLanded?.Invoke();
        EventBroker.Invoke(eventArgs);
    }
}

public class BlockLandedEvent : IEvent
{
    public GameObject sender;
}