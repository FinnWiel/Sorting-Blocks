using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Cryptography;
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
using System.Xml.Linq;

namespace Sorting_Blocks
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<string> blockList = new List<string>();
        public List<string> colors = new List<string>();


        public MainWindow()
        {
            InitializeComponent();
            colors.Add("Red");              //1 #FFFF0000
            colors.Add("Orange");           //2 #FFFFA500
            colors.Add("Yellow");           //3 #FFFFFF00
            colors.Add("Yellowgreen");      //4 #FF9ACD32
            colors.Add("Green");            //5 #FF008000
        }

        private void btnGetBlocks_Click(object sender, RoutedEventArgs e)
        {
            int number;
            bool success = int.TryParse(tbxBlockAmount.Text, out number);
            if (success)
            {
                EmptyBlocks();
                int blockAmount = Int32.Parse(tbxBlockAmount.Text);
               
                Random random = new Random();

                for (int i = 0; i < blockAmount; i++)
                {
                    int randomClr = random.Next(0, colors.Count);
                    string rndColor = colors[randomClr];
                    blockList.Add(rndColor);
                }
                for (int i = 0; i < blockList.Count; i++)
                {
                    if (blockList[i] != null)
                    {
                        var converter = new System.Windows.Media.BrushConverter();
                        var brushColor = (Brush)converter.ConvertFromString(blockList[i]);
                        var name = "box" + i.ToString();

                        // Create the rectangle
                        Rectangle rec = new Rectangle();
                        rec.Width = 20;
                        rec.Height = 20;
                        rec.Fill = brushColor; //Make random color
                        rec.Stroke = Brushes.Black;
                        rec.StrokeThickness = .5;
                        rec.Name = name;
                        Thickness margin = rec.Margin;
                        rec.Margin = margin;
                        blockContainer.Children.Add(rec);

                        //(Re)name boxes
                        object isRegistered = blockContainer.FindName("box" + i.ToString());
                        if (isRegistered is Rectangle)
                        {
                            UnregisterName(rec.Name);
                        }
                        RegisterName(rec.Name, rec);
                    }
                }

                //Show list in msgBox 
                //var message = string.Join(Environment.NewLine, blockList) ;
                //MessageBox.Show(message);
            }
            else
            {
                MessageBox.Show("Please enter a valid number.");
            }
        }
        private void FillBlocks(int[] array)
        {
            EmptyBlocks();
            List<string> newList = new List<string>();

            for (int i = 0; i < array.Length; i++)
            {
                switch (array[i])
                {
                    case 1:
                        newList.Add("Red");
                        break;
                    case 2:
                        newList.Add("Orange");
                        break;
                    case 3:
                        newList.Add("Yellow");
                        break;
                    case 4:
                        newList.Add("Yellowgreen");
                        break;
                    case 5:
                        newList.Add("Green");
                        break;
                }


                var converter = new System.Windows.Media.BrushConverter();
                var brushColor = (Brush)converter.ConvertFromString(newList[i]);

                // Create the rectangle
                var name = "box" + i.ToString();
                Rectangle rec = new Rectangle();
                rec.Width = 20;
                rec.Height = 20;
                rec.Fill = brushColor; //Make random color
                rec.Stroke = Brushes.Black;
                rec.StrokeThickness = .5;
                rec.Name = name;
                Thickness margin = rec.Margin;
                rec.Margin = margin;
                blockContainer.Children.Add(rec);
            }
        }
        private void EmptyBlocks()
        {
            blockList.Clear();
            blockContainer.Children.Clear();
        }

        private async void btnSort_Click(object sender, RoutedEventArgs e)
        {
            //Sort 1: Red, 2: Orange, 3: Yellow, 4: Yellowgreen, 5: Green.
            //MessageBox.Show(((SolidColorBrush)box1Green.Fill).Color);

            //Red:          #FFFF0000 -> 1
            //Organe:       #FFFFA500 -> 2
            //Yellow:       #FFFFFF00 -> 3
            //Yellowgreen:  #FF9ACD32 -> 4
            //Green:        #FF008000 -> 5

            List<int> startingList = new List<int>();
            for (int i = 0; i < blockList.Count; i++)
            {
                var a = FindName("box" + i) as Rectangle;
                var b = a.Fill;
                switch (b.ToString())
                {
                    case "#FFFF0000":
                        startingList.Add(1);
                        break;
                    case "#FFFFA500":
                        startingList.Add(2);
                        break;
                    case "#FFFFFF00":
                        startingList.Add(3);
                        break;
                    case "#FF9ACD32":
                        startingList.Add(4);
                        break;
                    case "#FF008000":
                        startingList.Add(5);
                        break;
                }
            }
            int[] unsortedArray = startingList.ToArray();
            //var message = string.Join(Environment.NewLine, arr) ;
            //MessageBox.Show(message);
 
            for (int i = 0; i < unsortedArray.Length; i++)
            {
                int currentBlock = unsortedArray[i];
                int prevBlock = i - 1;

                while (prevBlock >= 0 && unsortedArray[prevBlock] > currentBlock) 
                {
                    unsortedArray[prevBlock + 1] = unsortedArray[prevBlock];
                    prevBlock--;
                }
                unsortedArray[prevBlock + 1] = currentBlock;

                //Zet het gesorte blocks in de wrappanel
                FillBlocks(unsortedArray);
                await Task.Delay(50);
            }
            MessageBox.Show("The blocks are done sorting!");
        }
    }
}
