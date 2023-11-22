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

namespace InteropTypes.IO.VersionControl
{

    [System.Diagnostics.DebuggerDisplay("🐢 {_Target.FileName}")]
    public class SVNFileProvider : IFileProvider
    {
        #region lifecycle

        public SVNFileProvider(SvnClient client, SvnTarget repositoryUrl, SvnRevision revision = null)
        {
            ArgumentNullException.ThrowIfNull(client);
            ArgumentNullException.ThrowIfNull(repositoryUrl);

            Client = client;            

            if (!Client.GetInfo(repositoryUrl, out _Info)) throw new ArgumentException($"can't access {repositoryUrl}", nameof(repositoryUrl));

            // using a static reader that reads the whole repository in one shot.
            _Reader = new _StaticRepoReader(client, repositoryUrl, revision);
        }        

        #endregion

        #region data        

        public SvnClient Client { get; protected set; }

        private IFileProvider _Reader;

        private SvnInfoEventArgs _Info;

        #endregion

        #region properties

        public long LastChangeRevision => _Info?.LastChangeRevision ?? -1;

        public DateTime LastChangeTime => _Info?.LastChangeTime ?? DateTime.MinValue;

        #endregion

        #region API

        public IDirectoryContents GetDirectoryContents(string subpath)
        {
            return _Reader.GetDirectoryContents(subpath);
        }

        public IFileInfo GetFileInfo(string subpath)
        {
            return _Reader.GetFileInfo(subpath);
        }

        IChangeToken IFileProvider.Watch(string filter)
        {
            return _Reader.Watch(filter);
        }

        #endregion

        #region helpers

        public static long GetRevisionNumberFrom(IFileInfo finfo)
        {
            return finfo is SVNEntryInfo svnInfo ? svnInfo.Revision : -1;
        }

        public static SvnRevision GetRevisionFrom(IFileInfo finfo)
        {
            return finfo is IServiceProvider srv
                ? srv.GetService(typeof(SvnRevision)) as SvnRevision
                : null;
        }

        #endregion
    }    

    public class SVNDisposableFileProvider : SVNFileProvider, IDisposable
    {
        #region lifecycle

        public SVNDisposableFileProvider(SvnTarget repositoryUrl, SvnRevision revision = null)
            : base(new SvnClient(), repositoryUrl, revision) { }

        public SVNDisposableFileProvider(SvnTarget repositoryUrl, NetworkCredential credentials, SvnRevision revision = null)
            : base(new SvnClient(), repositoryUrl, revision)
        {
            this.Client.Authentication.DefaultCredentials = credentials;
        }

        public SVNDisposableFileProvider(SvnTarget repositoryUrl, Action<SvnClient> configure, SvnRevision revision = null)
            : base(new SvnClient(), repositoryUrl, revision)
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
