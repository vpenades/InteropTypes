using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.FileProviders;

using SharpSvn;
using SharpSvn.Remote;

namespace InteropTypes.IO.VersionControl
{
    
    abstract class SVNFileInfo : SVNEntryInfo, IFileInfo
    {
        #region properties
        string IFileInfo.PhysicalPath => null;
        bool IFileInfo.IsDirectory => false;
        public long Length { get; private set; }        

        #endregion

        #region API

        public abstract Stream CreateReadStream();

        protected override void Update(SvnTarget target, ISvnRepositoryListItem args)
        {
            base.Update(target, args);
            
            if (args.Entry.NodeKind != SvnNodeKind.File) throw new ArgumentException("not a file", nameof(args));
            
            Length = args.Entry.FileSize;
            Exists = true;
        }

        #endregion
    }

    [System.Diagnostics.DebuggerDisplay("🗎 {_Target.FileName} {Length}")]
    sealed class _SVNClientFileInfo : SVNFileInfo
    {
        #region lifecycle

        public _SVNClientFileInfo(SvnClient client, SvnTarget target, ISvnRepositoryListItem args)
        {
            _Client = client;
            _Target = target;
            Update(target, args);
        }

        public _SVNClientFileInfo(SvnClient client, SvnTarget target, SvnInfoEventArgs args)
        {
            _Client = client;
            _Target = target;
            Update(target, args);
        }

        #endregion

        #region data

        private SvnClient _Client;
        private SvnTarget _Target;

        private WeakReference<Byte[]> _CachedContent;

        #endregion

        #region API

        public override Stream CreateReadStream()
        {
            return _InternalUtils.GetMemoryStream(ref _CachedContent, () => _Client.OpenReadStream(_Target));
        }

        public override object GetService(Type serviceType)
        {
            if (serviceType == typeof(SvnClient)) return _Client;
            if (serviceType == typeof(SvnTarget)) return _Target;            

            return base.GetService(serviceType);
        }

        #endregion
    }
}
