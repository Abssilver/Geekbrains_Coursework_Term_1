namespace FileManager.DrawService
{
    /// <summary>
    /// Класс точки, содержащий координаты для отрисовки
    /// </summary>
    public class Point
    {
        public int X { get; }
        public int Y { get; }
        public Point(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
    }
}