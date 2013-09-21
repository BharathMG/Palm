using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.IO.IsolatedStorage;
using System.Diagnostics;

namespace Palm
{

    public partial class Profile : PhoneApplicationPage
    {

        // Personality can have many attributes. As of now, Name is created.
        public class Personality
        {
            public string Name
            {
                get;
                set;
            }
        } 

        public class Details
        {
            public string Name
            {
                get;
                set;
            }
            public string Age
            {
                get;
                set;
            }
            public string Gender
            {
                get;
                set;
            }
            public Personality personality
            {
                get;
                set;
            }

        }

        public Profile()
        {
            InitializeComponent();
            
            Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        List<Personality> source = new List<Personality>();
                        source.Add(new Personality() { Name = "Adult" });
                        source.Add(new Personality() { Name = "Geek" });
                        source.Add(new Personality() { Name = "Teen" });
                        source.Add(new Personality() { Name = "Children" });
                        personality_list.ItemsSource = source;
                    });
            
        }


        private void choose_clicked(object sender, RoutedEventArgs e)
        {
            String age_text = age_box.Text;
            String name_text = name_box.Text;
            String gender_text = (bool)male_button.IsChecked ? "Male" : "Female";
            String personality_text = (personality_list.SelectedItem as Personality).Name;
            Personality personality = new Personality() { Name = personality_text };
            Details profile = new Details() { Age = age_text, Name = name_text, Gender = gender_text, personality = personality };
            
            //System.Diagnostics.Debug.WriteLine(age_text + " , " + name_text + " , " + gender_text + " , " + personality_text);
            if (age_text.Length < 1 || name_text.Length < 1)
            {
                Dispatcher.BeginInvoke(() =>
                {
                        MessageBox.Show("Please enter Age AND Name ");
                });
                return;
            }

            //Stores profile if everything is in place.
            storeProfile(profile);
            NavigationService.Navigate(new Uri("/Gifts.xaml?personality="+personality_text, UriKind.Relative));



        }

        private static void storeProfile(Details profile)
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains("profile"))
            {
                IsolatedStorageSettings.ApplicationSettings["profile"] = profile;
                Debug.WriteLine("Updated");
            }
            else
            {
                IsolatedStorageSettings.ApplicationSettings.Add("profile", profile);
                Debug.WriteLine("Created");
            }
            IsolatedStorageSettings.ApplicationSettings.Save();
        }

        public static Details loadProfile()
        {
            Details profile;
            if (IsolatedStorageSettings.ApplicationSettings.TryGetValue<Details>("profile",out profile))
            {
                return profile;
            }
            return null;
        }
    }
}