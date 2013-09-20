using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Palm
{
    public partial class Store : PhoneApplicationPage
    {
        Palm.Profile.Details details;
        public Store()
        {
            InitializeComponent();
            loadUI();
        }

        private void loadUI()
        {
            String name, age, gender, personality;
            details = Profile.loadProfile();
            if (details != null)
            {
                name = details.Name;
                age = details.Age;
                gender = details.Gender;
                personality = details.personality.Name;
                Dispatcher.BeginInvoke(() =>
                {
                    name_block.Text = name;
                    age_block.Text = age;
                    gender_block.Text = gender;
                    personality_block.Text = personality;
                });
            }
            else
            {
                Dispatcher.BeginInvoke(() =>
                {
                    MessageBox.Show("No Profile added. Please add in new profile page");

                });
            }

        }
    }
}