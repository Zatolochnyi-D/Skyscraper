using ThreeDent.Helpers.Extensions;
using UnityEngine;

public class BlocksSpawner : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject blockToSpawn;
    [SerializeField] private float spawnInterval;

    void Awake()
    {
        this.InvokeRepeatedly(Spawn, spawnInterval);
    }

    private void Spawn()
    {
        var newBlock = Instantiate(blockToSpawn, transform);
        newBlock.transform.position = spawnPoint.position;
    }
}