using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace уч_практика_мачнева
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void DrawGraph(Canvas canvas, int[] data, bool isDuplicateGraph = false)
        {
            if (data.Length == 0) return;
            canvas.Children.Clear();
            double canvasWidth = canvas.ActualWidth;
            double canvasHeight = canvas.ActualHeight;
            if (canvasWidth == 0 || canvasHeight == 0)
            {
                canvasWidth = 300;
                canvasHeight = 200;
            }
            double margin = 30;
            double graphWidth = canvasWidth - margin * 2;
            double graphHeight = canvasHeight - margin * 2;
            int max = data.Max();
            int min = data.Min();
            double range = max - min == 0 ? 1 : max - min;
            // Оси
            Line xAxis = new Line
            {
                X1 = margin,
                Y1 = canvasHeight - margin,
                X2 = canvasWidth - margin,
                Y2 = canvasHeight - margin,
                Stroke = Brushes.Black,
                StrokeThickness = 2
            };
            Line yAxis = new Line
            {
                X1 = margin,
                Y1 = margin,
                X2 = margin,
                Y2 = canvasHeight - margin,
                Stroke = Brushes.Black,
                StrokeThickness = 2
            };
            canvas.Children.Add(xAxis);
            canvas.Children.Add(yAxis);
            if (isDuplicateGraph && data.Length == 1)
            {
                double y = margin + (1 - (data[0] - min) / range) * graphHeight;
                Line flatLine = new Line
                {
                    X1 = margin,
                    Y1 = y,
                    X2 = canvasWidth - margin,
                    Y2 = y,
                    Stroke = Brushes.Red,
                    StrokeThickness = 2,
                    StrokeDashArray = new DoubleCollection { 4, 2 } 
                };
                canvas.Children.Add(flatLine);
            }
            else
            {
                Polyline polyline = new Polyline
                {
                    Stroke = Brushes.DarkBlue,
                    StrokeThickness = 2
                };
                for (int i = 0; i < data.Length; i++)
                {
                    double x = margin + i * (graphWidth / (data.Length - 1));
                    double y = margin + (1 - (data[i] - min) / range) * graphHeight;
                    polyline.Points.Add(new Point(x, y));
                }
                canvas.Children.Add(polyline);
            }
            for (int i = 0; i < data.Length; i++)
            {
                double x = margin + i * (graphWidth / (data.Length - 1));

                TextBlock label = new TextBlock
                {
                    Text = i.ToString(),
                    FontSize = 10
                };
                Canvas.SetLeft(label, x - 5);
                Canvas.SetTop(label, canvasHeight - margin + 2);
                canvas.Children.Add(label);
            }
            for (int i = 0; i <= 5; i++)
            {
                double value = min + i * (range / 5);
                double y = margin + (1 - (value - min) / range) * graphHeight;
                TextBlock label = new TextBlock
                {
                    Text = Math.Round(value, 1).ToString(),
                    FontSize = 10
                };
                Canvas.SetLeft(label, 0);
                Canvas.SetTop(label, y - 7);
                canvas.Children.Add(label);
            }
        }
        private void GenerateButton_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(ArraySizeTextBox.Text, out int size) || size <= 1 ||
                !int.TryParse(MinValueTextBox.Text, out int min) ||
                !int.TryParse(MaxValueTextBox.Text, out int max) || min > max)
            {
                MessageBox.Show("Пожалуйста, введите корректные значения.");
                return;
            }
            Random rand = new Random();
            int[] array = new int[size];
            for (int i = 0; i < size; i++)
            {
                array[i] = rand.Next(min , max + 1);
            }
            var analyzer = new ArrayAnalyzer(array);
            int duplicateCount = analyzer.CountDuplicates();
            var duplicateIndexes = analyzer.DuplicateIndexes;
            int[] duplicateValues = duplicateIndexes.Select(i => array[i]).ToArray();
            OriginalGraphCanvas.Children.Clear();
            DuplicateGraphCanvas.Children.Clear();
            DuplicatesListBox.Items.Clear();
            ResultTextBlock.Text = "";
            ResultTextBlock.Text = $"Повторяющихся подряд элементов: {duplicateCount}";

            if (duplicateValues.Length > 0)
            {
                foreach (int i in duplicateIndexes)
                {
                    DuplicatesListBox.Items.Add($"Индекс: {i}, Значение: {array[i]}");
                }
            }
            else
            {
                DuplicatesListBox.Items.Add("Нет повторяющихся подряд элементов.");
            }
            DrawGraph(OriginalGraphCanvas, array);
            bool isStraightLine = duplicateValues.Length == 1;
            DrawGraph(DuplicateGraphCanvas, duplicateValues, isDuplicateGraph: !isStraightLine);
        }
    }
}
