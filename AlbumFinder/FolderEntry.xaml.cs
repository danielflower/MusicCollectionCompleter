using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace AlbumFinder.Desktop
{
    /// <summary>
    /// Interaction logic for FolderEntry.xaml
    /// </summary>
    public partial class FolderEntry : UserControl
    {
        public static DependencyProperty SelectedDirectoryProperty = DependencyProperty.Register("SelectedDirectory", typeof(DirectoryInfo), typeof(FolderEntry), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public static DependencyProperty DescriptionProperty = DependencyProperty.Register("Description", typeof(string), typeof(FolderEntry), new PropertyMetadata(null));

        public event DependencyPropertyChangedEventHandler SelectedDirectoryChanged;

        public DirectoryInfo SelectedDirectory { get { return GetValue(SelectedDirectoryProperty) as DirectoryInfo; } set { SetValue(SelectedDirectoryProperty, value); } }

        public string Description { get { return GetValue(DescriptionProperty) as string; } set { SetValue(DescriptionProperty, value); } }


        public FolderEntry()
        {
            InitializeComponent();
        }


        private void BrowseFolder(object sender, RoutedEventArgs e)
        {
            // Initializing Open Dialog
            Gat.Controls.OpenDialogView openDialog = new Gat.Controls.OpenDialogView();
            Gat.Controls.OpenDialogViewModel vm = (Gat.Controls.OpenDialogViewModel)openDialog.DataContext;
            vm.IsDirectoryChooser = true;
            bool? result = vm.Show();
            if (result == true)
            {
                SelectedDirectory = new DirectoryInfo(vm.SelectedFolder.Path);
                BindingExpression thing = GetBindingExpression(SelectedDirectoryProperty);
                thing?.UpdateSource();
            }
        }

        private void OnStartClicked(object sender, RoutedEventArgs e)
        {
            SelectedDirectoryChanged?.Invoke(this, new DependencyPropertyChangedEventArgs(SelectedDirectoryProperty, null, SelectedDirectory));
        }
        
    }
    

}
