using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace ObjectTK.Tools.Shapes
{
    public class TexturedQuad
        : TexturedShape
    {
        public TexturedQuad()
        {
            DefaultMode = PrimitiveType.TriangleStrip;
            
            const int z = 0;
            Vertices = new[]
            {
                new Vector3(-1, -1, z),
                new Vector3( 1, -1, z),
                new Vector3(-1,  1, z),
                new Vector3( 1,  1, z)
            };
            
            TexCoords = new[]
            {
                new Vector2(0,0),
                new Vector2(1,0),
                new Vector2(0,1),
                new Vector2(1,1)
            };
        }
    }
}