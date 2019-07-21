using System.Windows.Controls;
using System;

namespace SampleLibraryUI.Controls
{
    /// <summary>
    /// Interaction logic for SliderControl.xaml
    /// </summary>
    public partial class SliderControl : UserControl
    {
        public SliderControl()
        {
            InitializeComponent();

            int count = 3;
            if (SliderStackPanel != null)
            {
                if (SliderStackPanel.Children != null)
                {
                    SliderStackPanel.Children.Clear();
                }

                if (countTextBox.Text == null)
                {
                    count = 1;
                }
                else
                {
                    count = Int32.Parse(countTextBox.Text);
                }


                for (int i = 0; i < count; i++)
                {
                    Slider newSlider = new Slider();
                    newSlider.Value = 0;
                    newSlider.Name = "Slider2";
  //                  SliderStackPanel.Children.Add(newSlider);

                }
            }



        }

        private void Slider_ValueChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<double> e)

        {
            debug.Text = e.RoutedEvent.Name;
          //  e.NewValue.ToString()
        }
        private void Slider_ValueCustom(object sender, System.Windows.RoutedPropertyChangedEventArgs<double> e) { }

        private void CountValueChanged(object sender, TextChangedEventArgs e)
        {
            //int count = Int32.Parse(countTextBox.Text);

            int count = 3;
            if(SliderStackPanel != null)
            {
                SliderStackPanel.Children.Clear();
                if (SliderStackPanel.Children != null)
                {
                    SliderStackPanel.Children.Clear();
                }
            }

            if (countTextBox.Text == null)
            {
                count =1;
            }
            else
            {
                count = Int32.Parse(countTextBox.Text);
            }              

            for (int i = 0; i < count; i++)
            {
                Slider newSlider = new Slider();
                newSlider.Value = 0;
                newSlider.Name = "Slider"+i.ToString();

                if(maxTextBox != null)
                {

                  newSlider.Minimum = Int32.Parse(minTextBox.Text);
                  newSlider.Maximum = Int32.Parse(maxTextBox.Text);
                }


                newSlider.TickFrequency = 10;
                newSlider.IsSnapToTickEnabled = true;


                if (SliderStackPanel != null)
                {
//                    SliderStackPanel.Children.Add(newSlider);
                    StackPanel newStackPanel = new StackPanel();
                    newStackPanel.Orientation = Orientation.Horizontal;

                    TextBlock newTextBlock = new TextBlock();
                    newTextBlock.Width = 65;
                    newTextBlock.Height = 14;
                    newTextBlock.Text = newSlider.Value.ToString();
                    newSlider.Width = 185;
                    
                    

                    newStackPanel.Children.Add(newTextBlock);
                    newStackPanel.Children.Add(newSlider);

                    SliderStackPanel.Children.Add(newStackPanel);
                }


               

            }
        }



    }
}
