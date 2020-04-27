using System;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
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
using Path = System.IO.Path;


namespace GepteremProjekt
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Grid VoteGrid = new Grid();
        PetrikLajosSzg Petrik;
        private List<String> images;
        private List<String> voteImages = new List<string> { 
            @"Kepek/Pont0.jpg",
            @"Kepek/Pont1.jpg",
            @"Kepek/Pont2.jpg",
            @"Kepek/Pont3.jpg"
        };
        public string PersonImage { get; set; }
        public int ActualRoom { get; set; }
        public MainWindow()
        {
            ActualRoom = 0;
            Petrik = new PetrikLajosSzg("petrikgepek.txt");
            images = new List<String>();
            InitializeComponent();
            for (int i = 0; i < Petrik.Geptermek.Count; i++)
            {
                //string file = $"{Petrik.Geptermek[i].Nev.Split(' ')[0]}.jpg";
                //string path = $"Kepek\{Petrik.Geptermek[i].Nev.Split(' ')[0]}.jpg";
                //string path = Path.Combine("Kepek", file).Replace("\\\\", "\\");

                images.Add($"Kepek/{Petrik.Geptermek[i].Nev.Split(' ')[0]}.jpg");
            }

        }

        private void CreateViewImageDynamically(int i)
        {
            // Create Image and set its width and height  
            //System.Windows.Controls.Image dynamicImage = new System.Windows.Controls.Image();

            // Create a BitmapSource  
            BitmapImage bitmap = CreateBitmap(i, images);

            // Add Image.Source  
            Pic.Source = bitmap;
            ChangeTitle();
            BuildGrid(ActualRoom);
        }

        private BitmapImage CreateBitmap(int i, List<String> images)
        {
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(images[i], UriKind.Relative);
            bitmap.EndInit();
            return bitmap;
        }

        private void BuildGrid(int actualRoom)
        {
            Vote.ColumnDefinitions.Clear();
            Vote.RowDefinitions.Clear();
            for (int i = 0; i < Petrik.Geptermek[ActualRoom].HelyDb; i++)
            {
                ColumnDefinition oszlop = new ColumnDefinition();
                Vote.ColumnDefinitions.Add(oszlop);
            }
            for (int i = 0; i < Petrik.Geptermek[ActualRoom].SorDb; i++)
            {
                RowDefinition sor = new RowDefinition();
                Vote.RowDefinitions.Add(sor);
            }
            for (int i = 0; i < Petrik.Geptermek[ActualRoom].SorDb; i++)
            {
                for (int j = 0; j < Petrik.Geptermek[ActualRoom].HelyDb; j++)
                {
                    BitmapImage bm = CreateBitmap(Petrik.Geptermek[ActualRoom].Ertekeles[i, j], voteImages);
                    System.Windows.Controls.Image img = new System.Windows.Controls.Image();
                    Grid.SetRow(img, i);
                    Grid.SetColumn(img, j);
                    img.Name = $"i_{i}_{j}";
                    img.Margin = new Thickness(5);
                    img.MouseRightButtonDown += Img_MouseRightButtonDown;
                    img.Source = bm;
                    
                    Vote.Children.Add(img);
                }
            }

        }

        private void Img_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Controls.Image pic = sender as System.Windows.Controls.Image;
            string[] coordinates = pic.Name.Split('_');
            int row = int.Parse(coordinates[1]);
            int column = int.Parse(coordinates[2]);
            var actualElement = Petrik.Geptermek[ActualRoom].Ertekeles[row, column];
            if (actualElement == 0)
            {
                Message("This cannot be changed");
                return;
            }
            else if (actualElement < 3)
            {
                Petrik.Geptermek[ActualRoom].Ertekeles[row, column]++;

            }
            else
            {
                Petrik.Geptermek[ActualRoom].Ertekeles[row, column] = 1;
            }

            BuildGrid(ActualRoom);
            ChangeTitle();
        }

        private static void Message(string m)
        {
            MessageBox.Show(m);
        }

        private void ChangeTitle()
        {

            Win.Title = $"{Petrik.Geptermek[ActualRoom].Nev} | Average: " + String.Format("{0:0.##}", Petrik.Geptermek[ActualRoom].Avg());
        }

        private void win_Loaded(object sender, RoutedEventArgs e)
        {
            CreateViewImageDynamically(ActualRoom);
        }

        private void LeftArrow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (ActualRoom > 0)
            {
                ActualRoom--;
            }
            else
            {
                ActualRoom = images.Count - 1;
            }
            CreateViewImageDynamically(ActualRoom);
        }

        private void RightArrow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (ActualRoom < images.Count - 1)
            {
                ActualRoom++;
            }
            else
            {
                ActualRoom = 0;
            }
            CreateViewImageDynamically(ActualRoom);
        }

        private void Save_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                File.Delete("petrikgepek.bak");
                File.Copy("petrikgepek.txt", "petrikgepek.bak");
                StreamWriter writer = new StreamWriter("petrikgepek.txt");
                foreach (var item in Petrik.Geptermek)
                {
                    writer.WriteLine(item.Nev);
                    writer.WriteLine($"{item.SorDb};{item.HelyDb}");
                    for (int i = 0; i < item.Ertekeles.GetLength(0); i++)
                    {
                        for (int j = 0; j < item.Ertekeles.GetLength(1); j++)
                        {
                            writer.Write($"{item.Ertekeles[i, j]};");
                        }
                        writer.WriteLine();
                    }
                    writer.WriteLine();
                }
                writer.Close();
                Message("Sikeres mentés");
            }
            catch (IOException ex)
            {

                Message(ex.Message);
            }
        }
    }
}
