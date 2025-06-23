using ThreeDent.EventBroker;
using ThreeDent.Helpers.Extensions;
using UnityEngine;

public class BlocksSpawner : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject blockToSpawn;
    [SerializeField] private float additionalSpawnInterval = 1f;

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
        newBlock.transform.position = spawnPoint.position;
        BlockMovementController.SetCurrentBlock(newBlock.GetComponent<BlockMover>());
    }

    private void SpawnAfterInterval()
    {
        this.InvokeOnce(Spawn, additionalSpawnInterval);
    }
}