using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InteropTypes.Vision.IO
{
    static class _ReflectionUtils
    {
        public static VideoCaptureDevice CreateDefaultCaptureDevice()
        {
            #if DEBUG

            var args = Environment.GetCommandLineArgs();

            const string cmd = "-DebugFFmpegSource:";

            var src = args.FirstOrDefault(item => item.StartsWith(cmd));
            var path = src == null ? null : src.Substring(cmd.Length);
            if (path != null && System.IO.File.Exists(path))
            {
                return _CreateVideoFile(path);
            }

            #endif

            return _CreateCaptureDevice();
        }

        private static VideoCaptureDevice _CreateCaptureDevice()
        {
            try
            {
                var minfo = _FindStaticMethod("TensorFlow.Emgu", "EmguCaptureDevice", "CreateFromDirectShow");
                if (minfo == null) return null;

                return minfo.Invoke(null, null) as IO.VideoCaptureDevice;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private static VideoCaptureDevice _CreateVideoFile(string videoFilePath)
        {
            try
            {
                var minfo = _FindStaticMethod("TensorFlow.Emgu", "EmguCaptureDevice", "CreateFromUrl");
                if (minfo == null) return null;

                return minfo.Invoke(null, new object[] { videoFilePath }) as IO.VideoCaptureDevice;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private static System.Reflection.MethodInfo _FindStaticMethod(string assemblyName, string className, string methodName)
        {
            var assembly = AppDomain.CurrentDomain
                    .GetAssemblies()
                    .SingleOrDefault(assembly => assembly.GetName().Name == assemblyName);
            if (assembly == null) return null;

            var tinfo = assembly
                .GetTypes()
                .SingleOrDefault(t => t.Name.EndsWith(className));
            if (tinfo == null) return null;

            return tinfo.GetMethod(methodName, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
        }
    }
}
