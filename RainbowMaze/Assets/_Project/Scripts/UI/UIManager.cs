using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text textSteps;
    [SerializeField] private Text textFewestSteps;
    [SerializeField] private Text textMaze;
    [SerializeField] private Text textMazesPassed;

    [SerializeField] private RulesMenuManager canvasRules;

    private void Start()
    {
        textFewestSteps.text = $"Record:{PlayerData.FewestSteps}";
        textMaze.text = $"Maze {PlayerData.CurrentMaze}";
        textMazesPassed.text = $"Passed:{PlayerData.MazesPassed}";
    }

    public void OpenRulesMenu()
    {
        canvasRules.gameObject.SetActive(true);

        canvasRules.Open();
    }

    public void UpdateSteps(int steps)
    {
        textSteps.text = $"Steps:{steps}";
    }
}
