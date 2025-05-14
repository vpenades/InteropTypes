using System;

namespace InteropTypes.IO
{
    public partial class PhysicalDirectoryInfo
    {
        /// <summary>
        /// Maps to <see cref="Environment.CurrentDirectory"/>
        /// </summary>
        public static PhysicalDirectoryInfo CurrentDirectory
        {
            get
            {
                var dinfo = new System.IO.DirectoryInfo(Environment.CurrentDirectory);
                return new PhysicalDirectoryInfo(dinfo);
            }            
        }

        public static PhysicalDirectoryInfo GetSpecialDirectory(Environment.SpecialFolder folder)
        {
            var dinfo = new System.IO.DirectoryInfo(Environment.GetFolderPath(folder));
            return new PhysicalDirectoryInfo(dinfo);
        }

        public static PhysicalDirectoryInfo GetSpecialDirectory(Environment.SpecialFolder folder, Environment.SpecialFolderOption option)
        {
            var dinfo = new System.IO.DirectoryInfo(Environment.GetFolderPath(folder, option));
            return new PhysicalDirectoryInfo(dinfo);
        }


        /// <summary>
        /// Maps <see cref="AppContext.BaseDirectory"/>
        /// </summary>
        /// <remarks>
        /// This is the directory that contains the assemblies and resources of the application, NOT where it's being executed.
        /// </remarks>
        public static PhysicalDirectoryInfo ApplicationDirectory => _AppBaseDir.Value;

        private static readonly Lazy<PhysicalDirectoryInfo> _AppBaseDir = new Lazy<PhysicalDirectoryInfo>(_GetApplicationDirectory);
        
        private static PhysicalDirectoryInfo _GetApplicationDirectory()
        {            
            var dinfo = new System.IO.DirectoryInfo(AppContext.BaseDirectory);
            return new PhysicalDirectoryInfo(dinfo);            
        }

        /// <summary>
        /// Maps to <see cref="System.Reflection.Assembly.GetEntryAssembly().Location"/> Directory
        /// </summary>
        /// <returns></returns>
        public static PhysicalDirectoryInfo GetEntryAssemblyDirectory()
        {
            var dinfo = new System.IO.DirectoryInfo(AppContext.BaseDirectory);
            return new PhysicalDirectoryInfo(dinfo);
        }
    }
}
