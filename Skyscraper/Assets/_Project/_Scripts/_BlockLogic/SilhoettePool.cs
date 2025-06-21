using ThreeDent.Helpers.Tools;
using UnityEngine;

public class SilhoettePool : Singleton<SilhoettePool>
{
    [SerializeField] private GameObject[] blockVisuals;

    private GameObjectPool[] blockVisualPools;

    protected override void Awake()
    {
        base.Awake();
        blockVisualPools = new GameObjectPool[blockVisuals.Length];
        for (int i = 0; i < blockVisuals.Length; i++)
            blockVisualPools[i] = new(blockVisuals[i], parent: transform);
    }

    public static GameObject GetSilhoette(int index)
    {
        return Instance.blockVisualPools[index].GetPooledObject();
    }
}
