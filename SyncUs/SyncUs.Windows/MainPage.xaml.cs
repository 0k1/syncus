using Newtonsoft.Json;
using SyncUs.Helpers;
using SyncUs.ViewModel;
using System;
using System.Collections.Generic;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


namespace SyncUs
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }
        private async void Load_Click(object sender, RoutedEventArgs e)
        {
            FolderPicker folderPicker = new FolderPicker();
            folderPicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            folderPicker.FileTypeFilter.Add("*");
            List<SyncItem> tempList = new List<SyncItem>();
            StorageFolder folder = await folderPicker.PickSingleFolderAsync();
            if (folder != null)
            {
                OutputTextBlock.Text = "Picked folder: " + folder.Name;
                IReadOnlyList<IStorageItem> itemsInFolder = await folder.GetItemsAsync();
                foreach (IStorageItem item in itemsInFolder)
                {
                    SyncItem temp = new SyncItem();
                    temp.Path = item.Path;
                    temp.Name = item.Name;
                    if (item.IsOfType(StorageItemTypes.Folder))
                    {
                        temp.IsFolder = true;
                    }
                    else
                    {
                        temp.IsFolder = false;
                    }
                    tempList.Add(temp);
                }
                OutputTextBlock.Text = "Operation Success.";
                if ((string)(sender as Button).Content == "Load Local")
                {
                    LocalStatusBox.Text = folder.Path + "\t loaded";
                    MainViewModel.Instance.LocalSyncItems = tempList;
                    MainViewModel.Instance.Local = folder;

                }
                else
                {
                    RemoteStatusBox.Text = folder.Path + "\t loaded";
                    MainViewModel.Instance.RemoteSyncItems = tempList;
                    MainViewModel.Instance.Remote = folder;
                }

            }
            else
            {
                OutputTextBlock.Text = "Operation cancelled.";
            }
            await MainViewModel.WriteToFile(tempList, folder); //Will Enable Later when History and tracking is implemented
        }

        private void SyncButton_Click(object sender, RoutedEventArgs e)
        {
            SyncButton.IsEnabled = false;
            MainViewModel.Instance.SyncLocalToRemote();
            SyncButton.IsEnabled = true;
            OutputTextBlock.Text += "\n Sync Done";
        }
    }
}
