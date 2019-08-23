using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Capstone.Pages.Nodes
{
    /// <summary>
    /// Interaction logic for ReviewList.xaml
    /// </summary>
    public partial class ReviewList : Page
    {
        FirestoreDb db;
        string collecName = "";
        public ReviewList()
        {
            db = FirestoreDb.Create("nodereview-73781");
            InitializeComponent();
        }
        public ReviewList(string collection)
        {
            db = FirestoreDb.Create("nodereview-73781");
            InitializeComponent();
            collecName = collection;
            LoadSnap(collection);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(
                (new SpecificReviewPage(
                    collecName, 
                    ((Button)sender).Content.ToString())));
        }

        public async void LoadSnap(string collection)
        {
            Query allMediaQuery = db.Collection(collection);
            QuerySnapshot allMediaQuerySnap = await allMediaQuery.GetSnapshotAsync();
            foreach (DocumentSnapshot documentSnapshot in allMediaQuerySnap.Documents)
            {
                if (documentSnapshot.Exists)
                {
                    Border border = new Border();
                    border.MinWidth = 30;
                    border.MinHeight = 20;
                    border.Margin = new Thickness(10);
                    border.BorderThickness = new Thickness(5);
                    border.CornerRadius = new CornerRadius(5);
                    border.BorderBrush = new SolidColorBrush(Colors.Black);

                    Button b = new Button();
                    b.Content = documentSnapshot.Id.ToString();
                    b.Click += Button_Click;
                    b.Background = new SolidColorBrush(Colors.Black);
                    b.Foreground = new SolidColorBrush(Color.FromRgb(76, 181, 174));
                    b.FontSize = 30;
                    b.BorderThickness = new Thickness(0);

                    

                    border.Child = b;
                    reviewList.Children.Add(border);
                }
                else
                {
                    Console.WriteLine("Document {0} does not exist!", documentSnapshot.Id);
                }
            }
        }
    }
}
