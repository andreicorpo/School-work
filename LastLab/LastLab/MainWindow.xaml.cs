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

namespace LastLab
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        CroppedBitmap[] objImg = new CroppedBitmap[10];
        private bool gameStarted = false;
        private bool gameEnded = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            Random rnd = new Random();
            List<ValueTuple<int, int>> ts = new List<ValueTuple<int, int>>();

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    ts.Add(new ValueTuple<int, int>(i, j));
                }
            }

            Pieces.Background = null;

            CutImage();

            int x = 0;

            foreach (UIElement child in Pieces.Children)
            {
                if (child.GetValue(ContentProperty).ToString() != "1")
                {
                    child.Opacity = 100;
                    ImageBrush imageBrush = new ImageBrush();
                    imageBrush.ImageSource = objImg[x];
                    child.SetValue(BackgroundProperty, imageBrush);
                }
                x++;
            }

            x = 0;

            foreach (UIElement child in Pieces.Children)
            {
                int r = rnd.Next(ts.Count);
                Grid.SetColumn(child, ts[r].Item1);
                Grid.SetRow(child, ts[r].Item2);
                ts.RemoveAt(r);
            }

            gameEnded = false;
            gameStarted = true;
        }

        private void BtnPiece_Click(object sender, RoutedEventArgs e)
        {
            if (gameStarted && !gameEnded)
            {
                Button btn = sender as Button;
                String str = btn.GetValue(ContentProperty).ToString();
                int row = Grid.GetRow(btn);
                int col = Grid.GetColumn(btn);

                Button spaceTile = IsSpaceTile(row, col);
                if (spaceTile != null)
                {
                    int rowST = Grid.GetRow(spaceTile);
                    int colST = Grid.GetColumn(spaceTile);

                    Grid.SetColumn(btn, colST);
                    Grid.SetColumn(spaceTile, col);
                    Grid.SetRow(btn, rowST);
                    Grid.SetRow(spaceTile, row);
                }
                CheckEndGame();
            }
            else if (gameEnded)
            {
                MessageBox.Show("You won!");
            }
            else
            {
                MessageBox.Show("Press the start button to scramble the pieces");
            }
            
        }

        private static UIElement GetChildren(Grid grid, int row, int column)
        {
            foreach (UIElement child in grid.Children)
            {
                if (Grid.GetRow(child) == row
                      &&
                   Grid.GetColumn(child) == column)
                {
                    return child;
                }
            }
            return null;
        }

        private Button IsSpaceTile(int row, int columnn)
        {
            Button btn = GetChildren(Pieces, row, columnn + 1) as Button;
            if (btn != null)
            {
                if (btn.GetValue(ContentProperty).ToString() == "1")
                {
                    return btn;
                }
            }
            btn = GetChildren(Pieces, row + 1, columnn) as Button;
            if (btn != null)
            {
                if (btn.GetValue(ContentProperty).ToString() == "1")
                {
                    return btn;
                }
            }
            btn = GetChildren(Pieces, row - 1, columnn) as Button;
            if (btn != null)
            {
                if (btn.GetValue(ContentProperty).ToString() == "1")
                {
                    return btn;
                }
            }
            btn = GetChildren(Pieces, row, columnn - 1) as Button;
            if (btn != null)
            {
                if (btn.GetValue(ContentProperty).ToString() == "1")
                {
                    return btn;
                }
            }
            return null;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e) => Close();

        private void CheckEndGame()
        {
            bool ok = true;
            int x = 1;
            Button btn;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    btn = GetChildren(Pieces, i, j) as Button;
                    if (!btn.GetValue(ContentProperty).ToString().Equals(x.ToString()))
                    {
                        ok = false;
                    }
                    x++;
                }
            }

            if (ok)
            {
                gameEnded = true;
                MessageBox.Show("You won!");
            }
        }

        private void CutImage()
        {
            int count = 0;

            BitmapImage src = new BitmapImage();
            src.BeginInit();
            src.UriSource = new Uri(@"E:\Fac Files\Map\LastLab\LastLab\LastLab\Images\pic.jpg");
            src.CacheOption = BitmapCacheOption.OnLoad;
            src.EndInit();
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    objImg[count++] = new CroppedBitmap(src, new Int32Rect(j * 150, i * 150, 150, 150));
        }
    }
}
