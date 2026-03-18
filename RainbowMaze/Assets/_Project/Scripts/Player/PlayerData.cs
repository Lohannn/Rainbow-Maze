using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static int FewestSteps { get; set; }
    public static int CurrentMaze { get; set; } = 1;
    public static int MazesPassed { get; set; }

    public static void UpdateData(int steps)
    {
        CurrentMaze++;
        MazesPassed++;

        if (steps < FewestSteps || FewestSteps == 0)
        {
            FewestSteps = steps;
        }
    }
}
