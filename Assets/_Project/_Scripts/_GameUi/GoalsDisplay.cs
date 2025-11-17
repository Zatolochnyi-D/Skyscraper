using TMPro;
using UnityEngine;

public class GoalsDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI heighestScoreText;
    [SerializeField] private TextMeshProUGUI nextGoalText;

    private void Start()
    {
        GoalController.Instance.OnHeighestPointChange += DisplayHeighestPoint;
        nextGoalText.text = $"{GoalController.Instance.GoalPoint}m";
    }

    private void DisplayHeighestPoint()
    {
        heighestScoreText.text = $"{GoalController.Instance.HeighestPoint}m";
        nextGoalText.text = $"{GoalController.Instance.GoalPoint}m";
    }
}
