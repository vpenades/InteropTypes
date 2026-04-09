using System;
using System.Collections.Generic;
using System.IO;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Media.TextFormatting;
using Avalonia.Metadata;

using Microsoft.Extensions.FileProviders;

using DIRECTORYINFO = InteropTypes.IO.PhysicalDirectoryInfo;

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
            get => _CurrentDir;
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

    // example of new TreeDataTemplate: https://github.com/AvaloniaUI/Avalonia/blob/master/src/Markup/Avalonia.Markup.Xaml/Templates/TreeDataTemplate.cs

    class FileOrFoderTemplateSelector : Avalonia.Controls.Templates.ITreeDataTemplate
    {
        public bool SupportsRecycling => false;

        [Content]
        public Dictionary<string, ITreeDataTemplate> Templates { get; } = new Dictionary<string, ITreeDataTemplate>();

        [AssignBinding]
        public BindingBase ItemsSource { get; set; }

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

        public IDisposable BindChildren(AvaloniaObject target, AvaloniaProperty targetProperty, object item)
        {
            return ItemsSource is not null
                ? target.Bind(targetProperty, ItemsSource)
                : new _Empty();
        }

        private readonly struct _Empty : IDisposable
        {
            public void Dispose() { }
        }        
    }
}
