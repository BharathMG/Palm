﻿using System;
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
        }

        private void loadUI()
        {
            details = Profile.loadProfile();
            if (details != null)
            {
                
            }
        }
    }
}