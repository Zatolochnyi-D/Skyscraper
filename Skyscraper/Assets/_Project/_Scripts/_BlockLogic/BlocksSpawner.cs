using Skyscraper.WorldBounds;
using ThreeDent.EventBroker;
using ThreeDent.Helpers.Extensions;
using UnityEngine;

public class BlocksSpawner : MonoBehaviour
{
    [SerializeField] private float additionalSpawnInterval = 1f;
    [SerializeField] private float timeToReachUpperBound = 4f;

    private void Awake()
    {
        EventBroker.Subscribe<BlockLandedEvent>(SpawnAfterInterval);
        EventBroker.Subscribe<InventoryEmptyEvent>(DeactivateSpawner);
    }

    private void Start()
    {
        this.InvokeOnce(Spawn, 1);
    }

    private void OnDestroy()
    {
        EventBroker.Unsubscribe<BlockLandedEvent>(SpawnAfterInterval);
        EventBroker.Unsubscribe<InventoryEmptyEvent>(DeactivateSpawner);
    }

    private void Spawn()
    {
        var blockId = InventorySelectionController.Instance.GetCurrentSelectedBlockId();
        var prefab = PlayerInventory.Instance.GetPrefab(blockId);
        var newBlock = Instantiate(prefab, transform);
        newBlock.GetComponent<BlockSilhouetteController>().Show();
        var distanceInSeconds = ActiveBlockManager.Instance.FallSpeed * timeToReachUpperBound;
        newBlock.transform.position = new(Camera.main.transform.position.x, WorldBoundsController.UpperBound + distanceInSeconds, 0f);

        ActiveBlockManager.Instance.SetActiveBlock(newBlock);
    }

    private void SpawnAfterInterval()
    {
        this.InvokeOnce(Spawn, additionalSpawnInterval);
    }

    private void DeactivateSpawner()
    {
        Debug.Log("Inventory depleted. Waiting for game over confirmation.");
        EventBroker.Unsubscribe<BlockLandedEvent>(SpawnAfterInterval);
        StopAllCoroutines();
    }
}