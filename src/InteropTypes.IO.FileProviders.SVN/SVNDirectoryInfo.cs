using System;
using System.Collections;
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
    
    abstract class SVNDirectoryInfo : SVNEntryInfo, IFileInfo, IDirectoryContents
    {
        #region data        

        private IFileInfo[] _Contents;

        #endregion

        #region properties        
        string IFileInfo.PhysicalPath => null;
        bool IFileInfo.IsDirectory => true;
        long IFileInfo.Length => -1;        

        #endregion

        #region API

        Stream IFileInfo.CreateReadStream()
        {
            throw new NotSupportedException();
        }

        protected override void Update(SvnTarget target, ISvnRepositoryListItem args)
        {
            base.Update(target, args);
            
            if (args.Entry.NodeKind != SvnNodeKind.Directory) throw new ArgumentException("not a directory", nameof(args));            

            _Contents = null; // force refresh
        }

        protected override void Update(SvnTarget target, SvnInfoEventArgs args)
        {
            base.Update(target,args);

            if (args.NodeKind != SvnNodeKind.Directory) throw new ArgumentException("not a directory", nameof(args));

            _Contents = null; // force refresh
        }

        public IEnumerator<IFileInfo> GetEnumerator()
        {
            _Contents ??= LoadDirectoryContents().ToArray();
            return _Contents.AsEnumerable().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            _Contents ??= LoadDirectoryContents().ToArray();
            return _Contents.GetEnumerator();
        }

        internal void SetDirectoryContents(IEnumerable<IFileInfo> source)
        {
            _Contents = source.ToArray();
        }

        protected abstract IEnumerable<IFileInfo> LoadDirectoryContents();

        public abstract void CopyTo(System.IO.DirectoryInfo dstDir, SearchOption searchOption, bool overwrite = true);

        #endregion
    }

    [System.Diagnostics.DebuggerDisplay("📁 {_Target.FileName}")]
    sealed class _SVNClientDirectoryInfo : SVNDirectoryInfo
    {
        #region lifecycle

        public _SVNClientDirectoryInfo(SvnClient client, SvnTarget target, ISvnRepositoryListItem args)
        {
            _Client = client;
            _Target = target;
            Update(target, args);
        }

        public _SVNClientDirectoryInfo(SvnClient client, SvnTarget target, SvnInfoEventArgs args)
        {
            _Client = client;
            _Target = target;
            Update(target, args);
        }

        #endregion

        #region data

        private SvnClient _Client;
        private SvnTarget _Target;

        #endregion

        #region API

        protected override IEnumerable<IFileInfo> LoadDirectoryContents()
        {
            return new _DirectoryContents(_Client,_Target);
        }

        public override void CopyTo(System.IO.DirectoryInfo dstDir, SearchOption searchOption, bool overwrite = true)
        {
            dstDir.Create();

            var args = new SvnExportArgs();
            args.Depth = searchOption.ToSvnDepth();
            args.Overwrite = overwrite;

            if (!_Client.Export(_Target, dstDir.FullName, args)) throw new InvalidOperationException();
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