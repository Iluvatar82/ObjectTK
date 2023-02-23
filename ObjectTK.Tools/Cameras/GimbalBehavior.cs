using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace ObjectTK.Tools.Cameras
{
    public class GimbalBehavior
        : ThirdPersonBehavior
    {
        public override void MouseMove(CameraState state, Vector2 delta, MouseState? mouseState = null)
        {
            if (mouseState == default)
                return;

            if (mouseState.IsButtonDown(MouseButton.Left))
            {
                base.MouseMove(state, delta);
                var leftRight = Vector3.Cross(state.Up, state.LookAt);
                Vector3.Cross(state.LookAt, leftRight, out state.Up);
                state.Up.Normalize();
            }
        }
    }
}