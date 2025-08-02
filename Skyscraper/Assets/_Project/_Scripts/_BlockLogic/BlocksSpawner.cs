using System;
using System.Collections;
using Skyscraper.Inputs;
using Skyscraper.WorldBounds;
using ThreeDent.DevelopmentTools;
using ThreeDent.EventBroker;
using ThreeDent.Helpers.Extensions;
using UnityEngine;

public class BlocksSpawner : Singleton<BlocksSpawner>
{
    public event Action OnCountdownStart;
    public event Action<float, float> OnCountdown;
    public event Action OnCountdownEnd;

    [SerializeField] private float spawnInterval = 8f;
    [SerializeField] private float timeToReachUpperBound = 4f;

    private bool isSpawning = false;

    protected override void Awake()
    {
        base.Awake();
        EventBroker.Subscribe<BlockFirstCollisionEvent>(SpawnAfterInterval);
        EventBroker.Subscribe<InventoryEmptyEvent>(DeactivateSpawner);
        EventBroker.Subscribe<InventoryResuppliedEvent>(ReactivateSpawner);
        InputManager.OnSpeedupStarted += SpeedUpSpawn;
    }

    private void SpeedUpSpawn()
    {
        if (isSpawning)
        {
            isSpawning = false;
            EventBroker.Invoke<ForceBlockSpawnEvent>();
            ActiveBlockManager.Instance.RemoveActiveBlock();
            StopAllCoroutines();
            Spawn();
            OnCountdownEnd?.Invoke();
        }
    }

    private void Start()
    {
        this.InvokeOnce(SpawnAfterInterval, 1);
    }

    private void OnDestroy()
    {
        EventBroker.Unsubscribe<BlockFirstCollisionEvent>(SpawnAfterInterval);
        EventBroker.Unsubscribe<InventoryEmptyEvent>(DeactivateSpawner);
        EventBroker.Unsubscribe<InventoryResuppliedEvent>(ReactivateSpawner);
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
        StartCoroutine(SpawnCycle());
    }

    private IEnumerator SpawnCycle()
    {
        isSpawning = true;
        float elapsedTime = 0f;
        OnCountdownStart?.Invoke();
        while (true)
        {
            elapsedTime += Time.deltaTime;
            OnCountdown?.Invoke(spawnInterval - elapsedTime, (spawnInterval - elapsedTime) / spawnInterval);
            if (elapsedTime >= spawnInterval)
            {
                Spawn();
                isSpawning = false;
                OnCountdownEnd?.Invoke();
                break;
            }
            yield return null;
        }
    }

    private void DeactivateSpawner()
    {
        Debug.Log("Inventory depleted. Waiting for game over confirmation.");
        EventBroker.Unsubscribe<BlockFirstCollisionEvent>(SpawnAfterInterval);
        StopAllCoroutines();
    }

    private void ReactivateSpawner()
    {
        Debug.Log("Inventory was resupplied.");
        EventBroker.Subscribe<BlockFirstCollisionEvent>(SpawnAfterInterval);
        SpawnAfterInterval();
    }
}

public class ForceBlockSpawnEvent : IEvent { }