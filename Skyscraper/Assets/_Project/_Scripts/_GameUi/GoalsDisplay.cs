using ThreeDent.DevelopmentTools.Attributes;
using ThreeDent.Helpers.Extensions;
using TMPro;
using UnityEngine;

public class GoalsDisplay : MonoBehaviour
{
    [OnChild, SerializeField] private TextMeshProUGUI heighestScoreText;

    private void Start()
    {
        GoalController.Instance.OnHeighestPointChange += DisplayHeighestPoint;
    }

    private void DisplayHeighestPoint()
    {
        heighestScoreText.text = $"{GoalController.Instance.HeighestPoint}m";
    }
}
