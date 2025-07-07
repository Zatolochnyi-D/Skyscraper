using ThreeDent.DevelopmentTools;
using ThreeDent.EventBroker;
using UnityEngine;

public class ActiveBlockManager : Singleton<ActiveBlockManager>
{
    private GameObject activeBlock;
    private BlockMover activeBlockMover;
    private Rigidbody2D activeBlockRigidbody;

    public Transform ActiveBlockTransform => activeBlock.transform;

    public void SetActiveBlock(GameObject newActiveBlock)
    {
        activeBlock = newActiveBlock;

        activeBlockMover = activeBlock.GetComponent<BlockMover>();
        activeBlockMover.Activate();

        activeBlockRigidbody = activeBlock.GetComponent<Rigidbody2D>();
        activeBlockRigidbody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        EventBroker.Invoke<ActiveBlockSet>();
    }

    public void RemoveActiveBlock()
    {
        activeBlock = null;

        activeBlockMover.Deactivate();
        activeBlockMover = null;

        activeBlockRigidbody.collisionDetectionMode = CollisionDetectionMode2D.Discrete;
        activeBlockRigidbody = null;

        EventBroker.Invoke<ActiveBlockUnset>();
    }
}

public class ActiveBlockSet : IEvent { }
public class ActiveBlockUnset : IEvent { }