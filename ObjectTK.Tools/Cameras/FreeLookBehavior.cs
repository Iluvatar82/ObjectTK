using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace ObjectTK.Tools.Cameras
{
    public class FreeLookBehavior : CameraBehavior
    {
        public override void UpdateFrame(CameraState state, float step, KeyboardState? keyboardState = null)
        {
            if (keyboardState == default)
                return;

            var dir = Vector3.Zero;
            var leftRight = Vector3.Cross(state.Up, state.LookAt).Normalized();
            if (keyboardState.IsKeyDown(Keys.W)) dir += state.LookAt;
            if (keyboardState.IsKeyDown(Keys.S)) dir -= state.LookAt;
            if (keyboardState.IsKeyDown(Keys.A)) dir += leftRight;
            if (keyboardState.IsKeyDown(Keys.D)) dir -= leftRight;
            if (keyboardState.IsKeyDown(Keys.Space)) dir += state.Up;
            if (keyboardState.IsKeyDown(Keys.LeftControl)) dir -= state.Up;
            // normalize dir to enforce consistent movement speed, independent of the number of keys pressed
            if (dir.LengthSquared > 0) state.Position += dir.Normalized() * step;
        }

        public override void MouseMove(CameraState state, Vector2 delta, MouseState? mouseState = null)
        {
            if (mouseState == default)
                return;

            if (mouseState.IsButtonDown(MouseButton.Left))
                HandleFreeLook(state, delta);
        }
    }
}