//
// https://medium.com/@mikependon/designing-a-wpf-treeview-file-explorer-565a3f13f6f2
//

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using Catel.IoC;
using Catel.MVVM;
using Orc.ProjectManagement;
using WolvenKit.Common;
using WolvenKit.Common.Extensions;
using WolvenKit.Common.FNV1A;
using ObservableObject = Catel.Data.ObservableObject;

namespace WolvenKit.Model
{
    [Model]
    public class FileSystemInfoModel : ObservableObject
    {
        private class UpdateToken : FileSystemInfoModel
        {
            public UpdateToken(FileSystemInfoModel parent)
                : base(new DirectoryInfo("DummyFileSystemObjectInfo"), parent)
            {
            }
        }

        #region fields

        private object _childrenLock = new();
        private readonly IProjectManager _projectManager;
        

        #endregion

        #region constructors

        public  FileSystemInfoModel(FileSystemInfo fileSystemInfo, FileSystemInfoModel parent)
        {
            if (this is UpdateToken)
            {
                return;
            }

            Children = new ObservableCollection<FileSystemInfoModel>();
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                BindingOperations.EnableCollectionSynchronization(Children, _childrenLock);
            }));
            
            
            FileSystemInfo = fileSystemInfo;
            Parent = parent;
            _projectManager = ServiceLocator.Default.ResolveType<IProjectManager>();

            if (FileSystemInfo is DirectoryInfo)
            {
                AddUpdateToken();
            }

            PropertyChanged += FileSystemObjectInfo_PropertyChanged;
        }

        #endregion

        #region properties

        public FileSystemInfoModel Parent { get; }

        private bool _isExpanded;
        public bool IsExpanded
        {
            get => _isExpanded;
            set
            {
                if (value != _isExpanded)
                {
                    _isExpanded = value;
                    this.RaisePropertyChanged(nameof(IsExpanded));
                }

                // Expand all the way up to the root.
                if (_isExpanded && Parent != null)
                    Parent.IsExpanded = true;
            }
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected != value)
                {
                    var oldValue = _isSelected;
                    _isSelected = value;
                    RaisePropertyChanged(() => IsSelected, oldValue, value);
                }
            }
        }

        public bool IsDirectory => FileSystemInfo is DirectoryInfo;
        public bool IsFile => FileSystemInfo is FileInfo;

        public string Extension => GetFileExtension();
        public string Name => FileSystemInfo.Name;
        public string FullName => FileSystemInfo.FullName;

        private ObservableCollection<FileSystemInfoModel> _children;
        public ObservableCollection<FileSystemInfoModel> Children 
        {
            get => _children;
            set
            {
                if (_children != value)
                {
                    var oldValue = _children;
                    _children = value;
                    RaisePropertyChanged(() => Children, oldValue, value);
                }
            }
        }

        private FileSystemInfo _fileSystemInfo;
        public FileSystemInfo FileSystemInfo
        {
            get => _fileSystemInfo;
            set
            {
                if (_fileSystemInfo != value)
                {
                    var oldValue = _fileSystemInfo;
                    _fileSystemInfo = value;
                    RaisePropertyChanged(() => FileSystemInfo, oldValue, value);
                }
            }
        }
        #endregion

        #region events

        public event EventHandler RequestRefresh;

        public event EventHandler BeforeExpand;

        public event EventHandler AfterExpand;

        public event EventHandler BeforeExplore;

        public event EventHandler AfterExplore;

        
        public void RaiseRequestRefresh() => RequestRefresh?.Invoke(this, EventArgs.Empty);

        private void RaiseBeforeExpand() => BeforeExpand?.Invoke(this, EventArgs.Empty);

        private void RaiseAfterExpand() => AfterExpand?.Invoke(this, EventArgs.Empty);

        private void RaiseBeforeExplore() => BeforeExplore?.Invoke(this, EventArgs.Empty);

        private void RaiseAfterExplore() => AfterExplore?.Invoke(this, EventArgs.Empty);

        #endregion

        #region methods

        private void FileSystemObjectInfo_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!IsDirectory)
            {
                return;
            }

            switch (e.PropertyName)
            {
                case nameof(IsExpanded):
                    RaiseBeforeExpand();
                    if (IsExpanded)
                    {
                        if (HasDummy())
                        {
                            RaiseBeforeExplore();
                            RemoveDummy();
                            AddDirectories();
                            AddFiles();
                            RaiseAfterExplore();
                        }
                    }
                    RaiseAfterExpand();

                    break;
                default:
                    break;
            }
        }

        private void AddUpdateToken()
        {
            lock (_childrenLock)
            {
                Children.Add(new UpdateToken(this));
            }
        }

        private bool HasDummy() => GetDummy() != null;

        private UpdateToken GetDummy() => Children.OfType<UpdateToken>().FirstOrDefault();

        private void RemoveDummy()
        {
            lock (_childrenLock)
            {
                Children.Remove(GetDummy());
            }
        }

        public Task ReloadSync()
        {
            if (Children.Count > 0)
            {
                lock (_childrenLock)
                {
                    Children.Clear();
                }
            }
            AddDirectories();
            AddFiles();

            return Task.CompletedTask;
        }

        private void AddDirectories()
        {
            if (FileSystemInfo is not DirectoryInfo directoryInfo)
            {
                return;
            }

            var directories = directoryInfo.GetDirectories();
            foreach (var directory in directories.OrderBy(d => d.Name))
            {
                if ((directory.Attributes & FileAttributes.System) != FileAttributes.System &&
                    (directory.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden)
                {
                    var fileSystemObject = new FileSystemInfoModel(directory, this);
                    fileSystemObject.BeforeExplore += FileSystemObject_BeforeExplore;
                    fileSystemObject.AfterExplore += FileSystemObject_AfterExplore;
                    fileSystemObject.RequestRefresh += FileSystemObject_RequestRefresh;

                    lock (_childrenLock)
                    {
                        Children.Add(fileSystemObject);
                    }
                }
            }
        }

        private void AddFiles()
        {
            if (FileSystemInfo is not DirectoryInfo directoryInfo)
            {
                return;
            }

            var files = directoryInfo.GetFiles();
            foreach (var file in files.OrderBy(d => d.Name))
            {
                if ((file.Attributes & FileAttributes.System) != FileAttributes.System &&
                    (file.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden)
                {
                    var fileSystemObject = new FileSystemInfoModel(file, this);
                    fileSystemObject.RequestRefresh += FileSystemObject_RequestRefresh;

                    lock (_childrenLock)
                    {
                        Children.Add(fileSystemObject);
                    }
                }
            }
        }

        /// <summary>
        /// Exectuted when a RequestRefresh event is raised in one of the children
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FileSystemObject_RequestRefresh(object sender, EventArgs e)
        {
            if (Parent == null || sender is not FileSystemInfoModel oldModel)
            {
                return;
            }

            // Update Model
            IsExpanded = false;
            if (Children.Count > 0)
            {
                lock (_childrenLock)
                {
                    Children.Clear();
                }
            }

            AddUpdateToken();
            IsExpanded = true;

            //expand child with sender name
            //TODO: make this better?
            var oldNode = Children.FirstOrDefault(_ => _.FullName == oldModel.FullName);
            if (oldNode != null)
            {
                oldNode.IsExpanded = true;
            }

        }

        private void FileSystemObject_AfterExplore(object sender, EventArgs e) => RaiseAfterExplore();

        private void FileSystemObject_BeforeExplore(object sender, EventArgs e) => RaiseBeforeExplore();

        

        public void ExpandChildren(bool recursive)
        {
            IsExpanded = true;
            foreach (var info in Children)
            {
                info.IsExpanded = true;
                info.ExpandChildren(recursive);
            }
        }

        public void CollapseChildren(bool recursive)
        {
            IsExpanded = false;
            foreach (var info in Children)
            {
                info.IsExpanded = false;
                info.CollapseChildren(recursive);
            }
        }

        private string GetFileExtension()
        {
            var node = FileSystemInfo;
            if (node as DirectoryInfo != null)
            {
                if (_projectManager.ActiveProject is Tw3Project tw3Project)
                {
                    // check for base dirs
                    if (node.FullName == tw3Project.ModDirectory)
                        return ECustomImageKeys.ModImageKey.ToString();
                    if (node.FullName == tw3Project.ModCookedDirectory)
                        return ECustomImageKeys.ModCookedImageKey.ToString();
                    if (node.FullName == tw3Project.ModUncookedDirectory)
                        return ECustomImageKeys.ModUncookedImageKey.ToString();

                    if (node.FullName == tw3Project.DlcDirectory)
                        return ECustomImageKeys.DlcImageKey.ToString();
                    if (node.FullName == tw3Project.DlcCookedDirectory)
                        return ECustomImageKeys.DlcCookedImageKey.ToString();
                    if (node.FullName == tw3Project.DlcUncookedDirectory)
                        return ECustomImageKeys.DlcUncookedImageKey.ToString();

                    if (node.FullName == tw3Project.RawDirectory)
                        return ECustomImageKeys.RawImageKey.ToString();
                    if (node.FullName == tw3Project.RawModDirectory)
                        return ECustomImageKeys.RawModImageKey.ToString();
                    if (node.FullName == tw3Project.RawDlcDirectory)
                        return ECustomImageKeys.RawDlcImageKey.ToString();

                    if (node.FullName == tw3Project.RadishDirectory)
                        return ECustomImageKeys.RadishImageKey.ToString();
                }
                

                return IsExpanded
                    ? ECustomImageKeys.OpenDirImageKey.ToString()
                    : ECustomImageKeys.ClosedDirImageKey.ToString();
            }
            else
                return (node as FileInfo)?.Extension;
        }

        #endregion
    }

    
}