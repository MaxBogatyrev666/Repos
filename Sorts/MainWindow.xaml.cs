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
using System.Diagnostics;
using System.Net.NetworkInformation;

namespace Sorts
{
    public partial class MainWindow : Window
    {
        int[] numbers;
        int currentIndex = 0;
        double bubbleSortTime, mergeSortTime, quickSortTime;
        Stopwatch stopwatch1 = new Stopwatch();
        Stopwatch stopwatch2 = new Stopwatch();
        Stopwatch stopwatch3 = new Stopwatch();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox1.Text))
            {
                if (IsNum(textBox1.Text) == true)
                {
                    
                    numbers = new int[Convert.ToInt32(textBox1.Text)];
                    ArrayWithRandomNumbers(numbers, 0, 300);
                    string str_num = "";
                    for (int j = 0; j < numbers.Length; ++j)
                        str_num += numbers[j].ToString() + " ";

                    TextBox4.Text = str_num;

                    int[] bubbleArray = (int[])numbers.Clone();
                    int[] mergeArray = (int[])numbers.Clone();
                    int[] quickArray = (int[])numbers.Clone();

                    stopwatch1.Reset();
                    stopwatch1.Start();
                    BubbleSort(bubbleArray);
                    stopwatch1.Stop();
                    bubbleSortTime = stopwatch1.Elapsed.TotalMilliseconds;

                    stopwatch2.Reset();
                    stopwatch2.Start();
                    MergeSort(mergeArray, 0, mergeArray.Length - 1);
                    stopwatch2.Stop();
                    mergeSortTime = stopwatch2.Elapsed.TotalMilliseconds;

                    stopwatch3.Reset();
                    stopwatch3.Start();
                    QuickSort(quickArray, 0, numbers.Length - 1);
                    stopwatch3.Stop();
                    quickSortTime = stopwatch3.Elapsed.TotalMilliseconds;

                    if (textBox2.Text == "1")
                    {
                        string str1 = "";
                        for (int j = 0; j < bubbleArray.Length; ++j)
                            str1 += bubbleArray[j].ToString() + " ";

                        textBox3.Text = str1;
                    }

                    if (textBox2.Text == "2")
                    {
                        string str2 = "";
                        for (int j = 0; j < mergeArray.Length; ++j)
                            str2 += mergeArray[j].ToString() + " ";

                        textBox3.Text = str2;
                    }

                    if (textBox2.Text == "3")
                    {
                        string str3 = "";
                        for (int j = 0; j < quickArray.Length; ++j)
                            str3 += quickArray[j].ToString() + " ";

                        textBox3.Text = str3;
                    }

                    label1.Content = bubbleSortTime.ToString();
                    label2.Content = mergeSortTime.ToString();
                    label3.Content = quickSortTime.ToString();

                    DrawBarChart();

                }
                else MessageBox.Show("Элементы в строке не являются числами!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else MessageBox.Show("Строка последовательности пуста!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void DrawBarChart()
        {
            canvas1.Children.Clear();
            double canvasWidth = canvas1.Width;
            double canvasHeight = canvas1.Height;
            double barWidth = canvasWidth / 4;
            double maxTime = Math.Max(bubbleSortTime, Math.Max(mergeSortTime, quickSortTime));

            if (maxTime == 0)
            {
                maxTime = 1;
            }

            double scale = (canvasHeight - 50) / maxTime;
            //draw bubble sort bar
            Rectangle bubbleBar = new Rectangle
            {
                Width = barWidth - 10,
                Height = bubbleSortTime * scale,
                Fill = Brushes.Red,
                Stroke = Brushes.Black,
                StrokeThickness = 1
                
            };

            Canvas.SetLeft(bubbleBar, barWidth * 0.5);
            Canvas.SetBottom(bubbleBar, 30);
            canvas1.Children.Add(bubbleBar);

            //draw merge sort bar
            Rectangle mergeBar = new Rectangle
            {
                Width = barWidth - 10,
                Height = mergeSortTime * scale,
                Fill = Brushes.Green,
                Stroke = Brushes.Black,
                StrokeThickness = 1

            };
            Canvas.SetLeft(mergeBar, barWidth * 1.5);
            Canvas.SetBottom(mergeBar, 30);
            canvas1.Children.Add(mergeBar);

            //draw quick sort bar
            Rectangle quickBar = new Rectangle
            {
                Width = barWidth - 10,
                Height = quickSortTime * scale,
                Fill = Brushes.Blue,
                Stroke = Brushes.Black,
                StrokeThickness = 1

            };
            Canvas.SetLeft(quickBar, barWidth * 2.5);
            Canvas.SetBottom(quickBar, 30);
            canvas1.Children.Add(quickBar);

            AddLabel("Bubble", barWidth * 0.5, canvasHeight - 20);
            AddLabel("Merge", barWidth * 1.5, canvasHeight - 20);
            AddLabel("Quick", barWidth * 2.5, canvasHeight - 20);

            AddLabel($"{bubbleSortTime:F6} ms", barWidth * 0.5, canvasHeight - bubbleSortTime * scale - 30);
            AddLabel($"{mergeSortTime:F6} ms", barWidth * 1.5, canvasHeight - mergeSortTime * scale - 30);
            AddLabel($"{quickSortTime:F6} ms", barWidth * 2.5, canvasHeight - quickSortTime * scale - 30);
        }

        private void AddLabel(string text, double x, double y)
        {
            TextBlock label = new TextBlock 
            {
                Text = text,
                Foreground = Brushes.Black,
                FontSize = 12
            };
            Canvas.SetLeft(label, x);
            Canvas.SetTop(label, y);
            canvas1.Children.Add(label);
        }

        public static bool IsNum(string s)
        {
            foreach (char c in s)
            {
                if (!Char.IsDigit(c)) return false;
            }
            return true;

        }
        public static void ArrayWithRandomNumbers(int[] array, int minValue, int maxValue)
        {
            Random rand = new Random();
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = rand.Next(minValue, maxValue + 1); 
            }
        }

        public void BubbleSort(int[] array)
        {
            for (int currentIndex = 0; currentIndex < array.Length - 1; currentIndex++)
            {
                for (int i = 0; i < array.Length - 1 - currentIndex; i++)
                {
                    if (array[i] > array[i+1])
                    {
                        int temp = array[i];
                        array[i] = array[i + 1];
                        array[i + 1] = temp;
                    }
                }
            }
        }

        public void QuickSort(int[] array, int left, int right)
        {
            if (left >= right)
                return;

            int pivotIndex = left + (right - left) / 2;
            int pivot = array[pivotIndex];

            int index = Partition(array, left, right, pivot);

            QuickSort(array, left, index - 1);
            QuickSort(array, index, right);
        }

        public int Partition(int[] array, int left, int right, int pivot)
        {
            while (left <= right)
            {
                while (array[left] < pivot)
                    left++;
                while (array[right] > pivot)
                    right--;

                if (left <= right)
                {
                    int temp = array[left];
                    array[left] = array[right];
                    array[right] = temp;

                    left++;
                    right--;
                }
            }
            return left;
        }

        public void MergeSort(int[] array, int left, int right)
        {
            if (left >= right)
                return;

            int middle = left + (right - left) / 2;
            MergeSort(array, left, middle);
            MergeSort(array, middle + 1, right);
            Merge(array, left, middle, right);
        }

        public void Merge(int[] array, int left, int middle, int right)
        {
            int[] leftArray = new int[middle - left + 1];
            int[] rightArray = new int[right - middle];

            Array.Copy(array, left, leftArray, 0, leftArray.Length);
            Array.Copy(array, middle + 1, rightArray, 0, rightArray.Length);

            int i = 0, j = 0, k = left;

            while (i < leftArray.Length && j < rightArray.Length)
            {
                if (leftArray[i] <= rightArray[j])
                {
                    array[k] = leftArray[i];
                    i++;
                }
                else
                {
                    array[k] = rightArray[j];
                    j++;
                }
                k++;
            }

            // Остатки левых элементов
            while (i < leftArray.Length)
            {
                array[k] = leftArray[i];
                i++;
                k++;
            }

            // Остатки правых элементов
            while (j < rightArray.Length)
            {
                array[k] = rightArray[j];
                j++;
                k++;
            }
        }

    }
}
