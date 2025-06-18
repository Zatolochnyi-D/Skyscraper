using UnityEngine;

public class BlockSilhouetteController : MonoBehaviour
{
    [SerializeField] private int silhoetteIndex;

    private GameObject silhoette;

    public void Show()
    {
        silhoette = SilhoettePool.GetSilhoette(silhoetteIndex);
        silhoette.SetActive(true);
    }
}
