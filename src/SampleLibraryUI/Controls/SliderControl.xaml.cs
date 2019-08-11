using System.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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


        private static Slider sliderDebugStatic;
        private static StackPanel stackPanelStatic;

        public SliderControl()
        {
            InitializeComponent();

            sliderDebugStatic = StaticSlider(sliderDebugStatic, sliderDebug);
            stackPanelStatic = StaticStackPanel(stackPanelStatic, SliderStackPanel_Copy);

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
                    //                   count = Int32.Parse(countTextBox.Text);
                }


                //               for (int i = 0; i < count; i++)
                {
                    //                   Slider newSlider = new Slider();
                    //                   newSlider.Value = 0;
                    //                   newSlider.Name = "Slider2";


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

 //               sliderValueCollection.Add(e.NewValue);
            }

            var element = sender as Slider;
            SliderCustomNodeModel.sliderValue = e.NewValue;





            //           debug.Text = SliderStackPanel.Children.IndexOf(element).ToString();

        }
        private void Slider_ValueCustom(object sender, System.Windows.RoutedPropertyChangedEventArgs<double> e) { }




        public static Slider StaticSlider(Slider staticSlider, Slider instanceSlider)
        {
            staticSlider = instanceSlider;
            return staticSlider;
        }

        public static StackPanel StaticStackPanel(StackPanel staticStackPanel, StackPanel instanceStackPanel)
        {
            staticStackPanel = instanceStackPanel;
            return staticStackPanel;
        }

        public static void AdditionalSliders(NodeModel datModel, int oldCount, int newCount)
        {
            for(int i = oldCount; i<newCount; i++)
            {
                AddSlider(datModel, i);
            }

        }


        public static void DeleteSliders(NodeModel datModel, int oldCount, int newCount)
        {
            for (int i = (oldCount-1); i > (newCount-1); i--)
            {
                DeleteSlider(datModel, i);
            }

        }

        public static void AddSlider(NodeModel datModel, int index)
        {
            Slider newDeepCopy = SliderDeepCopy(sliderDebugStatic);
            stackPanelStatic.Children.Add(newDeepCopy);
            SliderCustomNodeModel.sliderValueList.Add(0);

            //double movedSlider = new double();
            SliderINotifyModel newDataObject = new SliderINotifyModel();
            newDataObject.sliderCusModel = datModel as SliderCustomNodeModel;
            newDataObject.index = index;
            Binding newBinding = new Binding("MovedSliderProp");
            newBinding.Source = newDataObject;
            // Bind the new data source to the myText TextBlock control's Text dependency property.
            newDeepCopy.SetBinding(Slider.ValueProperty, newBinding);
        }

        public static void DeleteSlider (NodeModel datModel, int index)
        {
            stackPanelStatic.Children.RemoveAt(index+1);
            SliderCustomNodeModel.sliderValueList.RemoveAt(index);
        }







        private void CountValueChanged(object sender, TextChangedEventArgs e)
        {
            int count = 3;
            if (SliderStackPanel != null)
            {
                SliderStackPanel.Children.Clear();
                if (SliderStackPanel.Children != null)
                {
                    SliderStackPanel.Children.Clear();
                }
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
                newSlider.Name = "Slider" + i.ToString();

                if (maxTextBox != null)
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

            }
        }




        public static Slider SliderDeepCopy(Slider element)
        {
            string shapestring = XamlWriter.Save(element);
            StringReader stringReader = new StringReader(shapestring);
            XmlTextReader xmlTextReader = new XmlTextReader(stringReader);
            Slider DeepCopyobject = (Slider)XamlReader.Load(xmlTextReader);
            return DeepCopyobject;

        }

        public static Slider TextBoxDeepCopy(Slider element)
        {
            string shapestring = XamlWriter.Save(element);
            StringReader stringReader = new StringReader(shapestring);
            XmlTextReader xmlTextReader = new XmlTextReader(stringReader);
            Slider DeepCopyobject = ( Slider)XamlReader.Load(xmlTextReader);
            return DeepCopyobject;

        }

    }





}
