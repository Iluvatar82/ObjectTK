using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace ObjectTK.Tools
{
    /// <summary>
    /// Provides basic functionality to an OpenTK GameWindow such as camera controls,
    /// ModelView and Projection matrices and improved timing.
    /// </summary>
    public abstract class DerpWindow : GameWindow
    {
        private static readonly Logging.IObjectTKLogger Logger = Logging.LogFactory.GetLogger(typeof(DerpWindow));

        protected readonly FrameTimer FrameTimer;

        /// <summary>
        /// Initializes a new instance of the DerpWindow class.
        /// </summary>
        protected DerpWindow(int width, int height, string title)
            : base(GameWindowSettings.Default, GetNativeWindowSettings(width, height, title))
        {
            // log some OpenGL information
            Logger?.Info("OpenGL context information:");
            Logger?.InfoFormat("{0}: {1}", StringName.Vendor, GL.GetString(StringName.Vendor));
            Logger?.InfoFormat("{0}: {1}", StringName.Renderer, GL.GetString(StringName.Renderer));
            Logger?.InfoFormat("{0}: {1}", StringName.Version, GL.GetString(StringName.Version));
            Logger?.InfoFormat("{0}: {1}", StringName.ShadingLanguageVersion, GL.GetString(StringName.ShadingLanguageVersion));
            int numExtensions;
            GL.GetInteger(GetPName.NumExtensions, out numExtensions);
            Logger?.DebugFormat("Number available extensions: {0}", numExtensions);
            for (var i = 0; i < numExtensions; i++) Logger?.DebugFormat("{0}: {1}", i, GL.GetString(StringNameIndexed.Extensions, i));
            Logger?.InfoFormat("Initializing game window: {0}", title);
            // set up GameWindow events
            Resize += OnResize;
            UpdateFrame += OnUpdateFrame;
            // set up frame timer
            FrameTimer = new FrameTimer();
        }

        private void OnResize(ResizeEventArgs e)
        {
            Logger?.InfoFormat("Window resized to: {0}x{1}", Size.X, Size.Y);
        }

        private void OnUpdateFrame(FrameEventArgs e)
        {
            FrameTimer.Time();
        }

        private static NativeWindowSettings GetNativeWindowSettings(int widt, int height, string title)
        {
            var settings = NativeWindowSettings.Default;
            settings.Size = new Vector2i(widt, height);
            settings.APIVersion = new Version(4, 6);
            settings.Title = title;

            return settings;
        }
    }
}
