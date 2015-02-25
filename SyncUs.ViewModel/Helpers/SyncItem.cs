using System;
using System.Collections.Generic;
using System.Text;

namespace SyncUs.Helpers
{
    public class SyncItem
    {
        public string Path { get; set; }
        public string Name { get; set; }
        public bool IsFolder { get; set; }
        public bool SyncStatus { get; set; }
        public SyncItem()
        {
            Path = "";
            Name = "";
            IsFolder = false;
            SyncStatus = false;
        }
    }
}
