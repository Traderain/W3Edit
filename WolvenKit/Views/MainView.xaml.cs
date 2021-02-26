using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using AvalonDock.Layout.Serialization;
using Catel.Data;
using Catel.IoC;
using Catel.MVVM;
using WolvenKit.Commands;
using WolvenKit.ViewModels;
using WolvenKit.Layout.MLib;

namespace WolvenKit.Views
{
    public partial class MainView
    {
        #region fields

        private const string AvalonDockConfigPath = @".\Resources\AvalonDock.Layout.config";


		ICommand _loadLayoutCommand = null;
        ICommand _saveLayoutCommand = null;
        #endregion fields

        public MainView()
        {
            InitializeComponent();

            var path = Path.GetFullPath(AvalonDockConfigPath);
            LayoutLoader = new LayoutLoader(AvalonDockConfigPath);
		}

        /// <summary>
        /// Gets an object that loads the AvalonDock Xml layout string
        /// in an aysnchronous background task.
        /// </summary>
        private LayoutLoader LayoutLoader { get; set; }

		protected override void OnViewModelPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnViewModelPropertyChanged(e);

            if (e is not AdvancedPropertyChangedEventArgs property)
            {
                return;
            }

            switch (property.PropertyName)
            {
                case "SaveLayout":
                    OnSaveLayout();
                    break;
                default:
                    break;
            }
        }

        protected override async void OnLoaded(EventArgs e)
        {
            base.OnLoaded(e);

            await LayoutLoader.LoadLayoutAsync();

            // Load and layout AvalonDock elements when MainWindow has loaded
            OnLoadLayoutAsync();
        }

		#region methods
		#region LoadLayoutCommand
		public ICommand LoadLayoutCommand
		{
			get
			{
				if (_loadLayoutCommand == null)
				{
					_loadLayoutCommand = new DelegateCommand<object>(
						(p) => OnLoadLayoutAsync(p),
						(p) => CanLoadLayout(p));
				}

				return _loadLayoutCommand;
			}
		}

		private bool CanLoadLayout(object parameter) => LayoutLoader.CanLoadLayout();

        private void OnLayoutLoaded_Event(object sender, LayoutLoadedEventArgs layoutLoadedEvent) =>
            Application.Current.Dispatcher.Invoke(() =>
            {
                try
                {
                    // Process the finally block since we have nothing to do here
                    var result = layoutLoadedEvent?.Result;
                    if (result == null)
                    {
                        return;
                    }

                    if (result.LoadwasSuccesful == true)
                    {
                        // Make sure AvalonDock control is visible at the end of restoring layout
                        var stringLayoutSerializer = new XmlLayoutSerializer(dockManager);

                        using var reader = new StringReader(result.XmlContent);
                        stringLayoutSerializer.Deserialize(reader);
                    }
                }
                catch (Exception exception)
                {
                    Debug.WriteLine(exception);
                }
                finally
                {
                    // Make sure AvalonDock control is visible at the end of restoring layout
                    dockManager.SetCurrentValue(VisibilityProperty, Visibility.Visible);

                    // show default tools
                    //ServiceLocator.Default.ResolveType<ICommandManager>()
                    //    .GetCommand(AppCommands.Application.ShowLog)
                    //    .SafeExecute(true);
                    //ServiceLocator.Default.ResolveType<ICommandManager>()
                    //    .GetCommand(AppCommands.Application.ShowProjectExplorer)
                    //    .SafeExecute(true);
                }
            }, System.Windows.Threading.DispatcherPriority.Background);

        private async void OnLoadLayoutAsync(object parameter = null)
		{
			if (this.DataContext is WorkSpaceViewModel wspace)
				wspace.CloseAllDocuments();

			var myApp = (App)Application.Current;

            var LoaderResult = await this.LayoutLoader.GetLayoutString(OnLayoutLoaded_Event);

			// Call this even with null to ensure standard initialization takes place
			this.OnLayoutLoaded_Event(null, (LoaderResult == null ? null : new LayoutLoadedEventArgs(LoaderResult)));
		}

		#endregion

		#region SaveLayoutCommand
		public ICommand SaveLayoutCommand =>
            _saveLayoutCommand ?? (_saveLayoutCommand =
                new DelegateCommand<object>((p) => OnSaveLayout(p), (p) => CanSaveLayout(p)));

        private bool CanSaveLayout(object parameter) => true;

        private void OnSaveLayout(object parameter = null)
		{
            var layoutSerializer = new XmlLayoutSerializer(dockManager);
            layoutSerializer.Serialize(AvalonDockConfigPath);
        }

        #endregion

        #endregion methods

    

        private void UserControl_IsVisibleChanged_1(object sender, DependencyPropertyChangedEventArgs e)
        {
			if (this.IsVisible && this.IsLoaded)
			{
				DiscordRPCHelper.WhatAmIDoing("Main View");
			}
		}
    }
}
