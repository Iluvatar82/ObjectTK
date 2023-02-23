using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace ObjectTK.Tools.Cameras
{
    public class Camera
    {
        public CameraState State;
        public CameraState DefaultState;
        protected CameraBehavior Behavior;

        public float MouseMoveSpeed = 0.005f;
        public float MouseWheelSpeed = 0.1f;
        public float MoveSpeed = 60;

        public Camera()
        {
            State = new CameraState();
            DefaultState = new CameraState();
        }

        public void ResetToDefault()
        {
            State.Position = DefaultState.Position;
            State.LookAt = DefaultState.LookAt;
            State.Up = DefaultState.Up;
            Update();
        }

        public void SetBehavior(CameraBehavior behavior)
        {
            Behavior = behavior;
            Update();
        }

        public void Enable(GameWindow window)
        {
            if (Behavior == null) throw new InvalidOperationException("Can not enable Camera while the Behavior is not set.");
            window.UpdateFrame += UpdateFrame;
            window.MouseMove += MouseMove;
            window.MouseWheel += MouseWheelChanged;
        }

        public void Disable(GameWindow window)
        {
            window.UpdateFrame -= UpdateFrame;
            window.MouseMove -= MouseMove;
            window.MouseWheel -= MouseWheelChanged;
        }

        public void Update()
        {
            if (Behavior != null) Behavior.Initialize(State);
        }

        private void UpdateFrame(FrameEventArgs e)
        {
            Behavior.UpdateFrame(State, (float) e.Time * MoveSpeed);
        }

        private void MouseMove(MouseMoveEventArgs e)
        {
            Behavior.MouseMove(State, MouseMoveSpeed * new Vector2(e.DeltaX, e.DeltaY));
        }
        
        private void MouseWheelChanged(MouseWheelEventArgs e)
        {
            Behavior.MouseWheelChanged(State, MouseWheelSpeed * e.OffsetY);
        }

        /// <summary>
        /// TODO: add smooth transitions for the CameraState variables
        /// </summary>
        public Matrix4 GetCameraTransform()
        {
            // kind of hack: prevent look-at and up directions to be parallel
            if (Math.Abs(Vector3.Dot(State.Up, State.LookAt)) > 0.99999999999) State.LookAt += 0.001f * new Vector3(3,5,4);
            return Matrix4.LookAt(State.Position, State.Position + State.LookAt, State.Up);
        }

        public override string ToString()
        {
            return string.Format("({0},{1})", State, Behavior);
        }
    }
}