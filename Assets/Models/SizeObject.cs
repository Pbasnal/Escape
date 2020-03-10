using UnityEngine;

[CreateAssetMenu(fileName = "Size", menuName = "LevelGen/Size", order = 51)]
public class SizeObject : ScriptableObject, ISize
{
    public int height;
    public int width;

    public int Height
    {
        get { return height; }
        set { height = value; }
    }

    public int Width
    {
        get { return width; }
        set { width = value; }
    }
}
