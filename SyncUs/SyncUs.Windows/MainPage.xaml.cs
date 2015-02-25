using Newtonsoft.Json;
using SyncUs.Helpers;
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
            folderPicker.FileTypeFilter.Add(".");
            List<SyncItem> SyncItems = new List<SyncItem>();

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
                    SyncItems.Add(temp);
                }
                OutputTextBlock.Text = "Operation Success.";
                if ((string)(sender as Button).Content == "Load Local")
                    LocalStatusBox.Text = folder.Path + "\t loaded";
                else
                    RemoteStatusBox.Text = folder.Path + "\t loaded";

            }
            else
            {
                OutputTextBlock.Text = "Operation cancelled.";
            }
            string output = "";
            foreach (var item in SyncItems)
            {
                output = output + "\n" + JsonConvert.SerializeObject(item);
            }
            StorageFile Config = await folder.CreateFileAsync("config.json", CreationCollisionOption.OpenIfExists);
            await FileIO.WriteTextAsync(Config, output);

        }

        private void SyncButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
