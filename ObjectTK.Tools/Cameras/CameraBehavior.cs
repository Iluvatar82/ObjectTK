using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace ObjectTK.Tools.Cameras
{
    public abstract class CameraBehavior
    {
        public virtual void Initialize(CameraState state) { }
        public virtual void UpdateFrame(CameraState state, float step, KeyboardState? keyboardState = null) { }
        public virtual void MouseMove(CameraState state, Vector2 delta, MouseState? mouseState = null) { }
        public virtual void MouseWheelChanged(CameraState state, float delta) { }

        protected void HandleFreeLook(CameraState state, Vector2 delta)
        {
            var leftRight = Vector3.Cross(state.Up, state.LookAt);
            var forward = Vector3.Cross(leftRight, state.Up);
            // rotate look at direction
            var rot = Matrix3.CreateFromAxisAngle(state.Up, -delta.X) * Matrix3.CreateFromAxisAngle(leftRight, delta.Y);
            Vector3.Transform(state.LookAt, Quaternion.FromMatrix(rot), out state.LookAt);
            // renormalize to prevent summing up of floating point errors
            state.LookAt.Normalize();
            // flip up vector when pitched more than +/-90� from the forward direction
            if (Vector3.Dot(state.LookAt, forward) < 0) state.Up *= -1;
        }
    }
}