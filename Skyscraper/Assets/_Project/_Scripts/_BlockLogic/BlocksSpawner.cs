using Skyscraper.WorldBounds;
using ThreeDent.EventBroker;
using ThreeDent.Helpers.Extensions;
using UnityEngine;

public class BlocksSpawner : MonoBehaviour
{
    [SerializeField] private GameObject blockToSpawn;
    [SerializeField] private float additionalSpawnInterval = 1f;
    [SerializeField] private float timeToReachUpperBound = 4f;

    private void Awake()
    {
        EventBroker.Subscribe<BlockLandedEvent>(SpawnAfterInterval);
    }

    private void Start()
    {
        Spawn();
    }

    private void OnDestroy()
    {
        EventBroker.Unsubscribe<BlockLandedEvent>(SpawnAfterInterval);
    }

    private void Spawn()
    {
        var newBlock = Instantiate(blockToSpawn, transform);
        newBlock.GetComponent<BlockSilhouetteController>().Show();
        var distanceInSeconds = 0.5f * -Physics2D.gravity.y * Mathf.Pow(timeToReachUpperBound, 2f);
        newBlock.transform.position = new(0f, WorldBoundsController.RightBound + distanceInSeconds, 0f);

        ActiveBlockManager.Instance.SetActiveBlock(newBlock);
        // PointerController.SetActiveBlock(newBlock.transform);
    }

    private void SpawnAfterInterval()
    {
        this.InvokeOnce(Spawn, additionalSpawnInterval);
    }
}