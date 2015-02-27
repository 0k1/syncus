using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace SyncUs
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
        }

        private void SyncButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void Load_Click(object sender, RoutedEventArgs e)
        {
            await ListSDCardFileContents();
        }
        private async Task ListSDCardFileContents()
        {
            // List the first /default SD Card whih is on the device. Since we know Windows Phone devices only support one SD card, this should get us the SD card on the phone.
            StorageFolder sdCard = (await KnownFolders.RemovableDevices.GetFoldersAsync()).FirstOrDefault();
            if (sdCard != null)
            {
                StorageFolder sdrootFolder=  sdCard;
                // Get the root folder on the SD card.
                if (sdrootFolder != null)
                {
                    // List all the files on the root folder.
                    var files = await sdrootFolder.GetFilesAsync();
                    if (files != null)
                    {
                        foreach (StorageFile file in files)
                        {
                            Stream s = await file.OpenStreamForReadAsync();
                            if (s != null || s.Length == 0)
                            {
                                long streamLength = s.Length;
                                StreamReader sr = new StreamReader(s);
                                MessageBox.Text=sr.ReadToEnd();
                            }
                            else
                            {
                                MessageBox.Text+="\nThere were no files in the root folder";
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Text+="\nFailed to get root folder on SD card";
                }
            }
            else
            {
                MessageBox.Text += "\nSD Card not found on device";
            }


        }
    }
}
