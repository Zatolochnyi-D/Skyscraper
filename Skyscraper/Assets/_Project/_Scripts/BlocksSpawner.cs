using Skyscraper.WorldBounds;
using ThreeDent.EventBroker;
using ThreeDent.Helpers.Extensions;
using UnityEngine;

public class BlocksSpawner : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Collider2D heightProbe;
    [SerializeField] private GameObject blockToSpawn;
    [SerializeField] private float additionalSpawnInterval = 1f;
    [SerializeField] private float timeToReachUpperBound = 4f;

    void Awake()
    {
        EventBroker.Subscribe<BlockLandedEvent>(SpawnAfterInterval);
    }

    void Start()
    {
        Spawn();
    }

    void OnDestroy()
    {
        EventBroker.Unsubscribe<BlockLandedEvent>(SpawnAfterInterval);
    }

    private void Spawn()
    {
        var newBlock = Instantiate(blockToSpawn, transform);
        newBlock.GetComponent<BlockSilhouetteController>().Show();
        var distanceInSeconds = 0.5f * -Physics2D.gravity.y * Mathf.Pow(timeToReachUpperBound, 2f);
        newBlock.transform.position = spawnPoint.position.With(y: WorldBoundsController.RightBound + distanceInSeconds);
        BlockMovementController.SetCurrentBlock(newBlock.GetComponent<BlockMover>());
    }

    private void SpawnAfterInterval()
    {
        this.InvokeOnce(Spawn, additionalSpawnInterval);
    }
}