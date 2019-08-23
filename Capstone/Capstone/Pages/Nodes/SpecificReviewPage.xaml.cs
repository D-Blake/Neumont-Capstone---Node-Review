using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Capstone.Pages.Nodes
{
    /// <summary>
    /// Interaction logic for SpecificReviewPage.xaml
    /// </summary>
    public partial class SpecificReviewPage : Page
    {
        string collecName = "";
        string docName = "";
        FirestoreDb db;
        public SpecificReviewPage(string collection, string doc)
        {
            db = FirestoreDb.Create("nodereview-73781");
            InitializeComponent();
            collecName = collection;
            docName = doc;
            getSnapshot();
        }
        public async void getSnapshot()
        {
            Google.Cloud.Firestore.DocumentReference docRef = db.Collection(collecName).Document(docName);
            DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
            if (snapshot.Exists)
            {
                Console.WriteLine("Document data for {0} document:", snapshot.Id);
                mediaTitle.Text = "Title: " + snapshot.Id;

                Dictionary<string, object> media = snapshot.ToDictionary();
                foreach (KeyValuePair<string, object> pair in media)
                {
                    Console.WriteLine("{0}: {1}", pair.Key, pair.Value);
                    switch (pair.Key)
                    {
                        case "Desc":
                            mediaDesc.Text = "Description: " + pair.Value.ToString();
                            break;
                        case "Rating":
                            mediaRating.Text = "Rating: " + pair.Value.ToString();
                            break;
                    }
                }
            }
            else
            {
                Console.WriteLine("Document {0} does not exist!", snapshot.Id);
            }
            //Make Review blocks
            Query query = db.Collection(collecName).Document(docName).Collection("reviews");
            QuerySnapshot allReviewsQuerySnapshot = await query.GetSnapshotAsync();
            string rating = "", desc = "";
            
            foreach (DocumentSnapshot documentSnapshot in allReviewsQuerySnapshot.Documents)
            {
                if (documentSnapshot.Exists)
                {
                    Dictionary<string, object> media = documentSnapshot.ToDictionary();
                    foreach (KeyValuePair<string, object> pair in media)
                    {
                        Console.WriteLine("{0}: {1}", pair.Key, pair.Value);
                        switch (pair.Key)
                        {
                            case "desc":
                                desc = "Description: " + pair.Value.ToString();
                                break;
                            case "rating":
                                rating = "Rating: " + pair.Value.ToString() + "/10";
                                break;
                        }
                    }
                    Border border = new Border();
                    border.MinWidth = 30;
                    border.MinHeight = 20;
                    border.Margin = new Thickness(10);
                    border.BorderThickness = new Thickness(5);
                    border.CornerRadius = new CornerRadius(5);
                    border.BorderBrush = new SolidColorBrush(Colors.Black);

                    StackPanel panel = new StackPanel();
                    panel.Background = new SolidColorBrush(Colors.Black);
                    TextBlock nameBlock = new TextBlock();
                    nameBlock.Background = new SolidColorBrush(Colors.Black);
                    nameBlock.Foreground = new SolidColorBrush(Colors.White);
                    nameBlock.Text = "Name: " + documentSnapshot.Id;
                    nameBlock.FontSize = 15;

                    TextBlock ratingBlock = new TextBlock();
                    ratingBlock.Background = new SolidColorBrush(Colors.Black);
                    ratingBlock.Foreground = new SolidColorBrush(Colors.White);
                    ratingBlock.Text = rating;
                    ratingBlock.FontSize = 15;

                    TextBlock descBlock = new TextBlock();
                    descBlock.Background = new SolidColorBrush(Colors.Black);
                    descBlock.Foreground = new SolidColorBrush(Colors.White);
                    descBlock.Text = desc;
                    descBlock.FontSize = 15;

                    panel.Children.Add(nameBlock);
                    panel.Children.Add(ratingBlock);
                    panel.Children.Add(descBlock);

                    border.Child = panel;
                    reviewList.Children.Add(border);
                }
                else
                {
                    Console.WriteLine("Document {0} does not exist!", documentSnapshot.Id);
                }
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            //Add Review
            DocumentReference docRef = db.Collection(collecName).Document(docName).Collection("reviews").Document(usernameBox.Text);

            Dictionary<string, object> docData = new Dictionary<string, object>
                {
                    { "desc",  reviewBox.Text},
                    { "rating", (Int32.Parse(ratingBox.Text)) }
                };
            await docRef.SetAsync(docData);
            //Update media's rating
            Query query = db.Collection(collecName).Document(docName).Collection("reviews");
            QuerySnapshot allReviewsQuerySnapshot = await query.GetSnapshotAsync();
            int newRating = 0;
            int numRatings = 0;

            foreach (DocumentSnapshot documentSnapshot in allReviewsQuerySnapshot.Documents)
            {
                if (documentSnapshot.Exists)
                {
                    Dictionary<string, object> media = documentSnapshot.ToDictionary();
                    foreach (KeyValuePair<string, object> pair in media)
                    {
                        Console.WriteLine("{0}: {1}", pair.Key, pair.Value);
                        if (pair.Key.Equals("rating"))
                        {
                            newRating += Int32.Parse(pair.Value.ToString());
                            numRatings++;
                        }
                    }
                }
            }
            docRef = db.Collection(collecName).Document(docName);
            if (numRatings>0)
            {
                newRating = newRating / numRatings;
            }
            await docRef.UpdateAsync("Rating", newRating);
            NavigationService.Navigate((new SpecificReviewPage(collecName, docName)));
        }
    }
}
