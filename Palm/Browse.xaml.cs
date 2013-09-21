using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media.Imaging;
using Windows.Phone.Speech.Synthesis;
using System.Windows.Resources;
using System.IO;
using System.Windows.Threading;
using System.Globalization;

namespace Palm
{
    public partial class Browse : PhoneApplicationPage
    {
        private List<ViewModel> itemsSource = new List<ViewModel>(7);
        private DispatcherTimer playTimer;
        public Browse()
        {
            InitializeComponent();
            this.playTimer = new DispatcherTimer();
            this.playTimer.Interval = TimeSpan.FromSeconds(2);
            this.playTimer.Tick += this.OnPlayTimerTick;




            this.Unloaded += this.OnUnloaded;
        }
             private void OnPlayTimerTick(object sender, EventArgs e)
        {
            this.slideView.MoveToNextItem();
        }

        private async void speakText()
        {
            SpeechSynthesizer synth = new SpeechSynthesizer();

            await synth.SpeakTextAsync("These gifts are recommended for your loved ones");
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            

            this.LoadData();
            speakText();
            this.slideView.DataContext = this.itemsSource;
        } 
        private void LoadData()
        {
            StreamResourceInfo resource = Application.GetResourceStream(new Uri("Images/all.txt", UriKind.RelativeOrAbsolute));
            using (StreamReader reader = new StreamReader(resource.Stream))
            {
                string line = string.Empty;
                int index = 1;
                while (!string.IsNullOrEmpty(line = reader.ReadLine()))
                {
                    string[] values = line.Split(';');
                    ViewModel model = new ViewModel();
                    model.Image = new Uri("Images/" + values[0], UriKind.RelativeOrAbsolute);
                    model.Title = values[1];
                    model.Date = DateTime.Parse(values[2], CultureInfo.InvariantCulture);
                    model.Index = index;
                    model.Likes = values[3];

                    this.itemsSource.Add(model);
                    index++;
                }
            }
        }

        public class ViewModel
        {
            public int Index
            {
                get;
                set;
            }

            public Uri Image
            {
                get;
                set;
            }

            public string DateText
            {
                get
                {
                    return this.Date.ToString("MMMM dd, yyyy");
                }
            }

            public DateTime Date
            {
                get;
                set;
            }

            public string Title
            {
                get;
                set;
            }

            public string Likes
            {
                get;
                set;
            }
        }

 

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            if (this.playTimer.IsEnabled)
            {
                this.StopSlideShow();
            }
        }

        private void StopSlideShow()
        {
            this.playTimer.Stop();
            this.slide_button.Content = "Play";
        }

        private void OnPlayTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
           
        }

        private void playSlideShow(object sender, RoutedEventArgs e)
        {
            if (this.playTimer.IsEnabled)
            {
                this.StopSlideShow();
            }
            else
            {
                this.playTimer.Start();
                this.slide_button.Content = "Pause";
            }
        }
        private async void LockHelper(string filePathOfTheImage, bool isAppResource)
        {
            try
            {
                var isProvider = Windows.Phone.System.UserProfile.LockScreenManager.IsProvidedByCurrentApplication;
                if (!isProvider)
                {
                    // If you're not the provider, this call will prompt the user for permission.
                    // Calling RequestAccessAsync from a background agent is not allowed.
                    var op = await Windows.Phone.System.UserProfile.LockScreenManager.RequestAccessAsync();

                    // Only do further work if the access was granted.
                    isProvider = op == Windows.Phone.System.UserProfile.LockScreenRequestResult.Granted;
                }

                if (isProvider)
                {
                    // At this stage, the app is the active lock screen background provider.

                    // The following code example shows the new URI schema.
                    // ms-appdata points to the root of the local app data folder.
                    // ms-appx points to the Local app install folder, to reference resources bundled in the XAP package.
                    var schema = isAppResource ? "ms-appx:///" : "ms-appdata:///Local/";
                    var uri = new Uri(schema + filePathOfTheImage, UriKind.Absolute);

                    // Set the lock screen background image.
                    Windows.Phone.System.UserProfile.LockScreen.SetImageUri(uri);

                    // Get the URI of the lock screen background image.
                    var currentImage = Windows.Phone.System.UserProfile.LockScreen.GetImageUri();
                    MessageBox.Show("The new Wallpaper is set!");
                }
                else
                {
                    MessageBox.Show("You said no, so I can't update your background.");
                }
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }

        private void Change_Wallpaper(object sender, System.Windows.Input.GestureEventArgs e)
        {
            BitmapImage bitmap_image = (BitmapImage)(sender as Image).Source;
            string file_name = bitmap_image.UriSource.ToString();
            LockHelper(file_name, true);
        }

    }
}