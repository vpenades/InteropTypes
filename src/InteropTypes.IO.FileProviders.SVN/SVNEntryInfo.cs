using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpSvn;
using SharpSvn.Remote;

namespace InteropTypes.IO
{
    abstract class SVNEntryInfo : IServiceProvider
    {
        #region API

        public static SVNEntryInfo Create(SvnClient client, SvnTarget target)
        {
            if (client.GetInfo(target, out var info))
            {
                if (info.NodeKind == SvnNodeKind.Directory) return new _SVNClientDirectoryInfo(client, target, info);
                if (info.NodeKind == SvnNodeKind.File) return new _SVNClientFileInfo(client, target, info);
                throw new NotImplementedException(info.NodeKind.ToString());
            }

            throw new ArgumentException("failed", nameof(client));
        }

        #endregion

        #region properties        
        public string Name { get; protected set; }
        public bool Exists { get; protected set; }
        public DateTimeOffset LastModified { get; protected set; }

        public long Revision { get; private set; }

        #endregion

        #region API

        protected virtual void Update(SvnTarget target, ISvnRepositoryListItem args)
        {            
            if (args == null) throw new ArgumentNullException(nameof(args));
            System.Diagnostics.Debug.Assert(args is SvnListEventArgs largs && target.FileName == largs.Name);            

            Revision = args.Entry.Revision;

            Update(target, args.Entry.Time);
        }

        protected virtual void Update(SvnTarget target, SvnInfoEventArgs args)
        {            
            if (args == null) throw new ArgumentNullException(nameof(args));
            System.Diagnostics.Debug.Assert(target.FileName == args.Path);

            Revision = args.Revision;

            Update(target, args.LastChangeTime);
        }

        protected void Update(SvnTarget target, DateTime lastModified)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));
            Name = target.FileName;
            LastModified = lastModified;
            Exists = true;
        }

        public virtual object GetService(Type serviceType)
        {
            if (serviceType == typeof(long)) return this.Revision;
            if (serviceType == typeof(SvnRevision)) return new SvnRevision(this.Revision);

            return null;
        }

        #endregion
    }
}
