public class Size : ISize
{
    public int Height { get; set; }
    public int Width { get; set; }
}

public class LevelCoordinate : ISize
{
    public int Height { get; set; }
    public int Width { get; set; }

    public LevelCoordinate Clone()
    {
        return new LevelCoordinate
        {
            Height = this.Height,
            Width = this.Width
        };
    }
}