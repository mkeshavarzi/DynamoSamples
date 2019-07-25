using System.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Windows;
using Autodesk.DesignScript.Runtime;
using Dynamo.Controls;
using Dynamo.Graph.Nodes;
using Dynamo.UI.Commands;
using Dynamo.Wpf;
using ProtoCore.AST.AssociativeAST;
using SampleLibraryUI.Controls;
using SampleLibraryUI.Properties;
using SampleLibraryZeroTouch;
using Newtonsoft.Json;
using System.Windows.Data;
using SampleLibraryUI.Examples;
using System.IO;
using System.Xml;
using System.Windows.Markup;


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


        public static List<double> valueList = new List<double>();


        private void Slider_ValueChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<double> e)

        {
            debug.Text = e.NewValue.ToString();

            if (valueList.Count > 0)
            {
                valueList[0] = e.NewValue;
                SliderCustomNodeModel.sliderValueList[0] = e.NewValue;
            }

            else
            {
                valueList.Add(e.NewValue);
                SliderCustomNodeModel.sliderValueList.Add(e.NewValue);
            }

            var element = sender as Slider;
            SliderCustomNodeModel.sliderValue = e.NewValue;
            SliderCustomNodeModel.sliderMoved = e.NewValue;


 //           debug.Text = SliderStackPanel.Children.IndexOf(element).ToString();

        }
        private void Slider_ValueCustom(object sender, System.Windows.RoutedPropertyChangedEventArgs<double> e) { }



/*
        private void CountValueChanged(object sender, TextChangedEventArgs e)
        {
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
                 newSlider.TickFrequency = Int32.Parse(stepTextBox.Text);
                }



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

 //                   Binding myBinding = new Binding();
 //                   myBinding.Source = SliderCustomNodeModel.sliderMoved;
 //                   myBinding.Path = new PropertyPath("SliderMoved");
 //                   myBinding.Mode = BindingMode.TwoWay;
 //                   myBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
 //                   BindingOperations.SetBinding(newSlider, Slider.ValueProperty, myBinding);

                    newSlider.ValueChanged += Slider_ValueChanged; 

                    

                    newStackPanel.Children.Add(newTextBlock);
                    newStackPanel.Children.Add(newSlider);

                    SliderStackPanel.Children.Add(newStackPanel);
                    
                }




            }

            if (sliderDebug != null)
            {
                //               Slider newControl = new Slider { DataContext = sliderDebug.DataContext };
                //               SliderStackPanel_Copy.Children.Add(newControl);
                //               newControl.ValueChanged += Slider_ValueChanged;
                //              newControl.BindingGroup = sliderDebug.BindingGroup;

                Slider newDeepCopy = SliderDeepCopy(sliderDebug);
                SliderStackPanel_Copy.Children.Add(newDeepCopy);
                newDeepCopy.ValueChanged += Slider_ValueChanged;
                newDeepCopy.BindingGroup = sliderDebug.BindingGroup;

                Binding myBinding = new Binding();
                myBinding.Source = SliderCustomNodeModel.sliderMoved;
                myBinding.Path = new PropertyPath("SliderMoved");
                myBinding.Mode = BindingMode.TwoWay;
                myBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;

                BindingOperations.SetBinding(newDeepCopy, Slider.ValueProperty, myBinding);
            }
        }


*/
  
        public Slider SliderDeepCopy(Slider element)
        {
            string shapestring = XamlWriter.Save(element);
            StringReader stringReader = new StringReader(shapestring);
            XmlTextReader xmlTextReader = new XmlTextReader(stringReader);
            Slider DeepCopyobject = (Slider)XamlReader.Load(xmlTextReader);
            return DeepCopyobject;


        }
    }





}
