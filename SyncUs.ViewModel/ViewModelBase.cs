using System.ComponentModel;
using System.Runtime.Serialization;

namespace SyncUs.ViewModel
{
    [DataContract]
    [Windows.Foundation.Metadata.WebHostHidden]    
    public abstract class ViewModelBase : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                if (MainViewModel.Instance.Dispatcher.HasThreadAccess)
                    handler(this, new PropertyChangedEventArgs(propertyName));
                else
                    MainViewModel.Instance.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        handler(this, new PropertyChangedEventArgs(propertyName));
                    });  
            }
        }
    }
}
