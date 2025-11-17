using UnityEngine;
using ThreeDent.DevelopmentTools.Option;
using ThreeDent.DevelopmentTools.Attributes;

public class ParticleSystemToCameraLinker : MonoBehaviour
{
    private const float Gap = 3f;

    [OnThis, SerializeField] private ParticleSystem particles;

    private void Update()
    {
        var size = Camera.main.orthographicSize;
        var aspect = Camera.main.aspect;
        var xSize = size * aspect;
        particles.transform.ChangePosition(x: Camera.main.transform.position.x + xSize + Gap);
        var mainModule = particles.main;
        mainModule.startLifetime = (xSize + Gap) * 2f;
    }
}