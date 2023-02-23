using ObjectTK.Buffers;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace ObjectTK.Tools.Shapes
{
    public abstract class TexturedShape
        : Shape
    {
        public Vector2[] TexCoords { get; protected set; }
        public Buffer<Vector2> TexCoordBuffer { get; protected set; }

        public override void UpdateBuffers()
        {
            base.UpdateBuffers();
            TexCoordBuffer = new Buffer<Vector2>();
            TexCoordBuffer.Init(BufferTarget.ArrayBuffer, TexCoords);
        }

        protected override void Dispose(bool manual)
        {
            base.Dispose(manual);
            if (TexCoordBuffer != null) TexCoordBuffer.Dispose();
        }
    }
}