using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SyncUs.Helpers;
using Windows.Storage;
using Newtonsoft.Json;

namespace SyncUs.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private static MainViewModel _instance;
        public StorageFolder Local;
        public StorageFolder Remote;
        private List<SyncItem> _syncItems;
        private List<SyncItem> _remoteItems;
        public Windows.UI.Core.CoreDispatcher Dispatcher { get; set; }
        public static MainViewModel Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new MainViewModel();
                }
                return _instance;
            }
        }

        public List<SyncItem> LocalSyncItems
        {
            get
            {
                if (_syncItems == null)
                    _syncItems = new List<SyncItem>();
                return _syncItems;

            }
            set { _syncItems = value; }
        }
        public List<SyncItem> RemoteSyncItems
        {
            get
            {
                if (_remoteItems == null)
                    _remoteItems = new List<SyncItem>();
                return _remoteItems;

            }
            set { _remoteItems = value; }
        }

        #region Constructor
        public MainViewModel()
        {

        }
        #endregion
        /// <summary>
        /// Write Sync Items to config.json
        /// </summary>
        /// <param name="SyncItems"></param>
        /// <param name="folder"></param>
        /// <returns></returns>
        public static async System.Threading.Tasks.Task WriteToFile(List<SyncItem> SyncItems, StorageFolder folder)
        {
            string output = "";
            foreach (var item in SyncItems)
            {
                output = output + "\n" + JsonConvert.SerializeObject(item);
            }
            StorageFile Config = await folder.CreateFileAsync("config.json", CreationCollisionOption.OpenIfExists);
            await FileIO.WriteTextAsync(Config, output);
        }

        public void SyncLocalToRemote()
        {
            foreach (var item in LocalSyncItems)
            {
                if (item.SyncStatus)
                    continue;
                if (RemoteSyncItems.Contains(item))
                {
                    item.SyncStatus = true;
                }
                else
                {
                    CopyItem(item);
                }

            }
        }
        /// <summary>
        /// logic to copy  item from item.Path to remote location also sets syncstatus=true;
        /// </summary>
        /// <param name="item"></param>
        private async void CopyItem(SyncItem item)
        {
            if (item.IsFolder)
            {
                await Remote.CreateFolderAsync(item.Name, CreationCollisionOption.FailIfExists);
            }
            else
            {
                try
                {

                    StorageFile file = await StorageFile.GetFileFromPathAsync(item.Path);
                    await file.CopyAsync(Remote);
                    item.SyncStatus = true;
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debugger.Break();
                }
            }
        }
    }
}
