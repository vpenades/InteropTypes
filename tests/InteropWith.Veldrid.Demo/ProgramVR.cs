using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

using InteropBitmaps;
using InteropDrawing;

using InteropTypes.Graphics.Drawing;

using Veldrid;
using Veldrid.Sdl2;
using Veldrid.StartupUtilities;
using Veldrid.VirtualReality;

namespace InteropWith
{
    class ProgramVR
    {
        public static void Run(string[] args)
        {
            // Create the window and the graphics device
            if (!VeldridInit(out var window, out var graphicsDevice, out var vrContext))
            {
                Console.WriteLine("This sample requires an Oculus or OpenVR-capable headset.");
                return;
            }

            var factory = new VeldridDrawingFactory(graphicsDevice);

            // We run the game loop here and do our drawing inside of it.
            VeldridRunLoop(window, graphicsDevice, vrContext, () => Draw(factory, vrContext));

            factory.Dispose();

            graphicsDevice.Dispose();
        }

        private static bool VeldridInit(out Sdl2Window window, out GraphicsDevice graphicsDevice, out VRContext vr)
        {
            bool debug = false;
            #if DEBUG
            debug = true;
            #endif

            // create vrContext

            var useOculus = true;
            if (!VRContext.IsOculusSupported())
            {
                useOculus = false;
                if (!VRContext.IsOpenVRSupported())
                {                    
                    window = null;
                    graphicsDevice = null;
                    vr = null;
                    return false;
                }
            }            

            var vrOptions = new VRContextOptions
            {
                EyeFramebufferSampleCount = TextureSampleCount.Count4
            };

            vr = useOculus
                ? VRContext.CreateOculus(vrOptions)
                : VRContext.CreateOpenVR(vrOptions);

            // create window

            var wnd = VeldridStartup.CreateWindow(
                new WindowCreateInfo(
                    Sdl2Native.SDL_WINDOWPOS_CENTERED, Sdl2Native.SDL_WINDOWPOS_CENTERED,
                    1280, 720,
                    WindowState.Normal,
                    "Veldrid.VirtualReality Sample"));

            window = wnd;

            // create graphics device

            GraphicsBackend backend = GraphicsBackend.Direct3D11;
            
            var gdOptions = new GraphicsDeviceOptions(debug, null, false, ResourceBindingModel.Improved, true, true, true);

            // Oculus runtime causes validation errors.
            if (backend == GraphicsBackend.Vulkan) gdOptions.Debug = false;

            (GraphicsDevice gd, Swapchain sc) = CreateDeviceAndSwapchain(wnd, vr, backend, gdOptions);
            wnd.Resized += () => sc.Resize((uint)wnd.Width, (uint)wnd.Height);

            graphicsDevice = gd;

            vr.Initialize(gd);

            _MirrorTexMgr = new MirrorTextureManager(gd, vr, sc);

            return true;
        }

        private static (GraphicsDevice gd, Swapchain sc) CreateDeviceAndSwapchain(
            Sdl2Window window,
            VRContext vrc,
            GraphicsBackend backend,
            GraphicsDeviceOptions gdo)
        {
            if (backend == GraphicsBackend.Vulkan)
            {
                (string[] instance, string[] device) = vrc.GetRequiredVulkanExtensions();

                var vdo = new VulkanDeviceOptions(instance, device);
                var gd = GraphicsDevice.CreateVulkan(gdo, vdo);

                var swd = new SwapchainDescription(
                    VeldridStartup.GetSwapchainSource(window),
                    (uint)window.Width, (uint)window.Height,
                    gdo.SwapchainDepthFormat, gdo.SyncToVerticalBlank, true);
                var sc = gd.ResourceFactory.CreateSwapchain(swd);

                return (gd, sc);
            }
            else
            {
                var gd = VeldridStartup.CreateGraphicsDevice(window, gdo, backend);
                var sc = gd.MainSwapchain;
                return (gd, sc);
            }
        }


        private static MirrorTextureManager _MirrorTexMgr;

        static Vector3 _userPosition = Vector3.Zero;

        private static void VeldridRunLoop(Sdl2Window window, GraphicsDevice gd, VRContext vr, Action action)
        {
            while (window.Exists)
            {
                var input = window.PumpEvents();

                if (!window.Exists) continue;

                action();
                vr.SubmitFrame();                

                _MirrorTexMgr.Run(MirrorTextureEyeSource.BothEyes);
                gd.SwapBuffers();
            }
        }        

        private static void Draw(VeldridDrawingFactory factory, VRContext vrContext)
        {
            var poses = vrContext.WaitForPoses();

            var lv = poses.CreateView(VREye.Left, _userPosition, -Vector3.UnitZ, Vector3.UnitY);
            var lp = poses.LeftEyeProjection;

            var rv = poses.CreateView(VREye.Right, _userPosition, -Vector3.UnitZ, Vector3.UnitY);
            var rp = poses.RightEyeProjection;

            // why it works reversing the eyes !???

            DrawEye(factory, vrContext.LeftEyeFramebuffer, rv, rp);
            DrawEye(factory, vrContext.RightEyeFramebuffer, lv, lp);            
        }

        private static void DrawEye(VeldridDrawingFactory factory, Framebuffer target, Matrix4x4 view, Matrix4x4 proj)
        {
            using (var dc3 = factory.CreateDrawing3DContext(target, view, proj))
            {
                dc3.FillFrame(System.Drawing.Color.CornflowerBlue);

                dc3.DrawSphere((0, 0, -3), 1, System.Drawing.Color.Yellow);

                dc3.DrawSphere((1, 0, -3), 1, System.Drawing.Color.Yellow);
                dc3.DrawSphere((-1, 0, -3), 1, System.Drawing.Color.Yellow);

                dc3.DrawSphere((0, 1.5f, -3), 1, System.Drawing.Color.Yellow);
                dc3.DrawSphere((0, -1.5f, -3), 1, System.Drawing.Color.Yellow);

                dc3.DrawSegment((1.5f, 1.5f, -3), (1, 0.2f, -1), 0.1f, System.Drawing.Color.Red);

                dc3.DrawSphere((0, -1.5f, -10), 1, System.Drawing.Color.Yellow);

                dc3.DrawSphere((0, 0f, -0.20f), 0.1f, System.Drawing.Color.Yellow);
            }
        }
    }


    class MirrorTextureManager : IDisposable
    {
        #region lifecycle
        public MirrorTextureManager(GraphicsDevice gd, VRContext vr, Swapchain sc)
        {
            _Ext_GD = gd;
            _Ext_VR = vr;
            _Ext_SC = sc;
            _Commands = gd.ResourceFactory.CreateCommandList();
        }

        public void Dispose()
        {
            if (_Ext_GD == null) return;

            if (_Commands != null) _Ext_GD.DisposeWhenIdle(_Commands);            
            _Commands = null;

            _Ext_VR = null;
            _Ext_GD = null;
            _Ext_SC = null;
        }

        #endregion

        #region data

        private GraphicsDevice _Ext_GD;
        private VRContext _Ext_VR;
        private Swapchain _Ext_SC;

        private CommandList _Commands;

        #endregion

        #region API

        public void Run(MirrorTextureEyeSource eyeSource)
        {
            _Commands.Begin();
            _Commands.SetFramebuffer(_Ext_SC.Framebuffer);
            _Commands.ClearColorTarget(0, new RgbaFloat(0f, 0f, 0.2f, 1f));
            _Ext_VR.RenderMirrorTexture(_Commands, _Ext_SC.Framebuffer, eyeSource);
            // igr.Render(gd, windowCL);
            _Commands.End();
            _Ext_GD.SubmitCommands(_Commands);
            // _Ext_GD.SwapBuffers(_Ext_SC);
        }

        #endregion
    }
}
