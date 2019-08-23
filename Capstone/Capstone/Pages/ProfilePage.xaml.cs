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

namespace Capstone
{
    /// <summary>
    /// Interaction logic for ProfilePage.xaml
    /// </summary>
    public partial class ProfilePage : Page
    {
        FirestoreDb db;
        public ProfilePage()
        {
            db = FirestoreDb.Create("nodereview-73781");
            InitializeComponent();
            getSnapshot();
        }
        public async void getSnapshot()
        {
            DocumentReference docRef = db.Collection("users").Document("userID");
            DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
            if (snapshot.Exists)
            {
                Console.WriteLine("Document data for {0} document:", snapshot.Id);

                Dictionary<string, object> movie = snapshot.ToDictionary();
                foreach (KeyValuePair<string, object> pair in movie)
                {
                    Console.WriteLine("{0}: {1}", pair.Key, pair.Value);
                    switch (pair.Key)
                    {
                        case "profilePic":
                            BitmapImage profileImg = new BitmapImage();
                            profileImg.BeginInit();
                            profileImg.UriSource = new Uri(pair.Value.ToString());
                            profileImg.EndInit();
                            profileImage.Source = profileImg;
                            break;
                        case "bio":
                            profileBio.Text = "Bio: " + pair.Value.ToString();
                            break;
                    }
                }
            }
            else
            {
                Console.WriteLine("Document {0} does not exist!", snapshot.Id);
            }
        }
    }
}
