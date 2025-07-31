using ThreeDent.SceneManagement;
using UnityEngine;

public class DefaultSceneLoader : SceneTransitioner
{
    protected override void OnProgress(float progress)
    {
        Debug.Log($"Progress: {progress}");
    }

    protected override void OnStep(int stepIndex)
    {
        Debug.Log($"Step: {stepIndex}");
    }
}
