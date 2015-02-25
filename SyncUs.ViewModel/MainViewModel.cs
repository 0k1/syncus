using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SyncUs.ViewModel
{
    public class MainViewModel :ViewModelBase
    {
        private static MainViewModel _instance;
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
        public MainViewModel()
        {

        }
    }
}
