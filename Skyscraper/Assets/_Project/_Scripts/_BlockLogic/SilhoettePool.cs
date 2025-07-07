using ThreeDent.DevelopmentTools;
using UnityEngine;

public class SilhoettePool : Singleton<SilhoettePool>
{
    [SerializeField] private GameObject[] blockVisualPrefabs;

    private GameObject[] blockVisuals;

    protected override void Awake()
    {
        base.Awake();
        blockVisuals = new GameObject[blockVisualPrefabs.Length];
        for (int i = 0; i < blockVisualPrefabs.Length; i++)
        {
            blockVisuals[i] = Instantiate(blockVisualPrefabs[i], transform);
            blockVisuals[i].SetActive(false);
        }
    }

    public static GameObject GetSilhoette(int index)
    {
        return Instance.blockVisuals[index];
    }
}
