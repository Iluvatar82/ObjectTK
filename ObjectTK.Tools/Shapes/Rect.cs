using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace ObjectTK.Tools.Shapes
{
    public class Rect
        : Shape
    {
        public Rect(float right, float top)
            : this(-1, -1, right, top)
        {
        }

        public Rect(float left, float bottom, float right, float top)
        {
            DefaultMode = PrimitiveType.LineLoop;
            const int z = 0;
            Vertices = new[]
            {
                new Vector3(left, bottom, z),
                new Vector3(right, bottom, z),
                new Vector3(right, top, z),
                new Vector3(left, top, z)
            };
        }
    }
}