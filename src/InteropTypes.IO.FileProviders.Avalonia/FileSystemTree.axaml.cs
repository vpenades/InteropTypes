using System.IO;
using System.Collections.Generic;

using Avalonia;
using Avalonia.Controls.Templates;
using Avalonia.Metadata;


using Avalonia.Controls;

using DIRECTORYINFO = InteropTypes.IO.PhysicalDirectoryInfo;
using Microsoft.Extensions.FileProviders;
using System;
using Avalonia.Data;

namespace InteropTypes.IO
{
    public partial class FileSystemTree : UserControl
    {
        public FileSystemTree()
        {
            InitializeComponent();
        }

        public static readonly DirectProperty<FileSystemTree, DIRECTORYINFO> CurrentDirectoryProperty
            = AvaloniaProperty.RegisterDirect<FileSystemTree, DIRECTORYINFO>(nameof(CurrentDirectory), o => o.CurrentDirectory, (o, v) => o.CurrentDirectory = v);

        private DIRECTORYINFO _CurrentDir;

        public DIRECTORYINFO CurrentDirectory
        {
            get { return _CurrentDir; }
            set
            {
                if (SetAndRaise(CurrentDirectoryProperty, ref _CurrentDir, value))
                {
                    myTree.ItemsSource = _CurrentDir;
                }
            }
        }
    }

    // https://stackoverflow.com/questions/74003533/avalonia-treeview-template-selector

    class FileOrFoderTemplateSelector : ITreeDataTemplate
    {
        public bool SupportsRecycling => false;

        [Content]
        public Dictionary<string, ITreeDataTemplate> Templates { get; } = new Dictionary<string, ITreeDataTemplate>();        

        public bool Match(object data)
        {
            return data is IFileInfo;
        }

        public Control Build(object data)
        {
            if (data is IFileInfo xinfo)
            {
                var key = xinfo.IsDirectory ? "Folder" : "File";

                return Templates[key].Build(data);
            }

            throw new NotImplementedException();
        }

        public InstancedBinding ItemsSelector(object data)
        {
            if (data is IFileInfo xinfo)
            {
                if (!xinfo.IsDirectory) return null;

                var key = xinfo.IsDirectory ? "Folder" : "File";

                return Templates[key].ItemsSelector(data);
            }

            throw new NotImplementedException();
        }
    }
}
