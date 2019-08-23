using Capstone.misc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Google.Apis.Discovery;
using Google.Apis.Services;
using Google.Cloud.Firestore;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;

namespace Capstone
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public FirestoreDb db;

        public MainWindow()
        {
            System.Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", "C:\\Users\\black\\Downloads\\NodeReview-ae04e4b428de.json");
            InitializeComponent();
            Main.Content = new HomePage();
            db = FirestoreDb.Create("nodereview-73781");

        }
    

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new HomePage();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Main.Content = new ReviewPage();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Main.Content = new ProfilePage();
        }
    }
}
