using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "LevelSize", menuName = "Level Gen/LevelSize", order = 51)]
public class LevelSize : ScriptableObject
{
    public int heightInGrids = 4;
    public int widthInGrids = 4;
}
