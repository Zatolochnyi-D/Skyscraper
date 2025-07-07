using ThreeDent.DevelopmentTools;
using ThreeDent.EventBroker;
using UnityEngine;

public class ActiveBlockManager : Singleton<ActiveBlockManager>
{
    private GameObject activeBlock;
    private BlockMover activeBlockMover;

    public Transform ActiveBlockTransform => activeBlock.transform;

    public void SetActiveBlock(GameObject newActiveBlock)
    {
        activeBlock = newActiveBlock;

        activeBlockMover = activeBlock.GetComponent<BlockMover>();
        activeBlockMover.Activate();

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