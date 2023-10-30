using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;

using SharpSvn;

namespace InteropTypes.IO
{

    [System.Diagnostics.DebuggerDisplay("🐢 {_Target.FileName}")]
    public class SVNFileProvider : IFileProvider
    {
        #region lifecycle

        public SVNFileProvider(SvnClient client, SvnTarget target)
        {
            ArgumentNullException.ThrowIfNull(client);
            ArgumentNullException.ThrowIfNull(target);

            Client = client;
            _Target = target;

            if (!Client.GetInfo(_Target, out _Info)) throw new ArgumentException($"can't access {target}", nameof(target));
        }

        #endregion

        #region data

        public SvnClient Client { get; protected set; }

        private SvnTarget _Target;

        private SvnInfoEventArgs _Info;

        #endregion

        #region properties

        public long LastChangeRevision => _Info?.LastChangeRevision ?? -1;

        public DateTime LastChangeTime => _Info?.LastChangeTime ?? DateTime.MinValue;

        #endregion

        #region API

        public IDirectoryContents GetDirectoryContents(string subpath)
        {
            return new _DirectoryContents(Client, _Target.Concat(subpath));
        }

        public IFileInfo GetFileInfo(string subpath)
        {
            var target = _Target.Concat(subpath);

            return SVNEntryInfo.Create(Client, target) as IFileInfo
                ?? new Microsoft.Extensions.FileProviders.NotFoundFileInfo(subpath);
        }

        public IChangeToken Watch(string filter)
        {
            throw new NotSupportedException();
        }

        #endregion

        #region helpers

        public static SvnRevision GetRevisionFrom(IFileInfo finfo)
        {
            var srv = finfo as IServiceProvider;
            if (srv == null) return null;

            return srv.GetService(typeof(SvnRevision)) as SvnRevision;
        }

        #endregion
    }

    public class SVNDisposableFileProvider : SVNFileProvider, IDisposable
    {
        #region lifecycle

        public SVNDisposableFileProvider(SvnTarget target)
            : base(new SvnClient(), target) { }

        public SVNDisposableFileProvider(SvnTarget target, NetworkCredential credentials)
            : base(new SvnClient(), target)
        {
            this.Client.Authentication.DefaultCredentials = credentials;
        }

        public SVNDisposableFileProvider(SvnTarget target, Action<SvnClient> configure)
            : base(new SvnClient(), target)
        {
            configure(this.Client);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~SVNDisposableFileProvider() { Dispose(false); }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Client?.Dispose();
                Client = null;
            }
        }

        #endregion
    }


}
