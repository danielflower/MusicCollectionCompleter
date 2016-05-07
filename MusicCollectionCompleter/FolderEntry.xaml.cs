using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using MusicCollectionCompleter.Desktop.Properties;

namespace MusicCollectionCompleter.Desktop
{
    /// <summary>
    /// Interaction logic for FolderEntry.xaml
    /// </summary>
    public partial class FolderEntry : UserControl
    {
        private static readonly string MyMusicPath = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic,
            Environment.SpecialFolderOption.DoNotVerify);

        public static DependencyProperty SelectedDirectoryProperty = DependencyProperty.Register("SelectedDirectory",
            typeof(string), typeof(FolderEntry),
            new FrameworkPropertyMetadata(MyMusicPath, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static DependencyProperty DescriptionProperty = DependencyProperty.Register("Description", typeof(string),
            typeof(FolderEntry), new PropertyMetadata(null));

        public event DependencyPropertyChangedEventHandler SelectedDirectoryChanged;

        public string SelectedDirectory
        {
            get { return GetValue(SelectedDirectoryProperty) as string; }
            set
            {
                SetValue(SelectedDirectoryProperty, value);
                BindingExpression thing = GetBindingExpression(SelectedDirectoryProperty);
                thing?.UpdateSource();
            }
        }

        public string Description
        {
            get { return GetValue(DescriptionProperty) as string; }
            set { SetValue(DescriptionProperty, value); }
        }


        public FolderEntry()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            if (string.IsNullOrEmpty(SelectedDirectory))
            {
                SelectedDirectory = MyMusicPath;
            }
        }


        private void BrowseFolder(object sender, RoutedEventArgs e)
        {
            // Initializing Open Dialog
            Gat.Controls.OpenDialogView openDialog = new Gat.Controls.OpenDialogView();
            Gat.Controls.OpenDialogViewModel vm = (Gat.Controls.OpenDialogViewModel) openDialog.DataContext;
            vm.SelectedFilePath = SelectedDirectory;
            vm.IsDirectoryChooser = true;
            bool? result = vm.Show();
            if (result == true && vm.SelectedFolder != null)
            {
                SelectedDirectory = vm.SelectedFolder.Path;
                Settings.Default.Save();
            }
        }

        private void OnStartClicked(object sender, RoutedEventArgs e)
        {
            SelectedDirectoryChanged?.Invoke(this,
                new DependencyPropertyChangedEventArgs(SelectedDirectoryProperty, null, SelectedDirectory));
        }
    }
}