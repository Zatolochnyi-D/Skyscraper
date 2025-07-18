using ThreeDent.DevelopmentTools;
using ThreeDent.EventBroker;
using UnityEngine;

public class ActiveBlockManager : Singleton<ActiveBlockManager>
{
    [SerializeField] private float fallSpeed;

    public float FallSpeed => fallSpeed;
    public Rigidbody2D ActiveBodyRigidbody => activeBlockMover.PhysicalBody;

    private GameObject activeBlock;
    private BlockMover activeBlockMover;

    public GameObject ActiveBlock => activeBlock;
    public Transform ActiveBlockTransform => activeBlock.transform;

    public void SetActiveBlock(GameObject newActiveBlock)
    {
        activeBlock = newActiveBlock;

        activeBlockMover = activeBlock.GetComponent<BlockMover>();
        activeBlockMover.Activate(fallSpeed);

        EventBroker.Invoke<ActiveBlockSet>();
    }

    public void RemoveActiveBlock()
    {
        activeBlock = null;

        activeBlockMover.Deactivate();
        activeBlockMover = null;

        EventBroker.Invoke<ActiveBlockUnset>();
    }
}

public class ActiveBlockSet : IEvent { }
public class ActiveBlockUnset : IEvent { }