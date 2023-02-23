using ObjectTK.Buffers;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace ObjectTK.Tools.Shapes
{
    public abstract class Shape : GLResource
    {
        public PrimitiveType DefaultMode { get; set; }
        public Vector3[] Vertices { get; protected set; }
        public Buffer<Vector3> VertexBuffer { get; protected set; }

        public virtual void UpdateBuffers()
        {
            VertexBuffer = new Buffer<Vector3>();
            VertexBuffer.Init(BufferTarget.ArrayBuffer, Vertices);
        }

        protected override void Dispose(bool manual)
        {
            if (!manual) return;
            if (VertexBuffer != null) VertexBuffer.Dispose();
        }
    }
}
