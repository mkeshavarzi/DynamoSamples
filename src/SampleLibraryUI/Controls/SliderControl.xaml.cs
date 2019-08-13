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

        private static UserControl userControlStatic;
        private static ScrollViewer scrollStatic;
        private static Slider sliderDebugStatic;
        private static StackPanel stackPanelStatic;
        private static StackPanel stackPanel_AllSlidersStatic;
        private static TextBox textBoxDebugStatic;


        public static List<SliderINotifyModel> sliderINotifyModelList = new List<SliderINotifyModel>();


        public SliderControl()
        {
            InitializeComponent();

 //           sliderDebugStatic = StaticSlider(sliderDebugStatic, sliderDebug);
 //           textBoxDebugStatic = StaticTextBox(textBoxDebugStatic, textBoxDebug);


 //           stackPanelStatic = StaticStackPanel(stackPanelStatic, SliderStackPanel_Copy);
 //           stackPanel_AllSlidersStatic = StaticStackPanel(stackPanel_AllSlidersStatic, SliderStackPanel_AllSliders);

 //           scrollStatic = StaticScroll(scrollStatic, SliderScroll);
 //           userControlStatic = StaticUserControl(userControlStatic, MultiSliderUserControl);          

 //           SliderStackPanel_Copy.Visibility = Visibility.Hidden;
 //           stackPanelStatic.Visibility = Visibility.Visible;

        }


        public static List<double> valueList = new List<double>();


        private void Slider_ValueChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<double> e)

        {
//            debug.Text = e.NewValue.ToString();

            if (valueList.Count > 0)
            {
//                valueList[0] = e.NewValue;
 //               SliderCustomNodeModel.sliderValueList[0] = e.NewValue;
            }

            else
            {
 //               valueList.Add(e.NewValue);
//                SliderCustomNodeModel.sliderValueList.Add(e.NewValue);

 //               sliderValueCollection.Add(e.NewValue);
            }

 //           var element = sender as Slider;
 //           SliderCustomNodeModel.sliderValue = e.NewValue;





            //           debug.Text = SliderStackPanel.Children.IndexOf(element).ToString();

        }
        private void Slider_ValueCustom(object sender, System.Windows.RoutedPropertyChangedEventArgs<double> e) { }

        private void MovedSliderProp_Changed(object sender, System.Windows.RoutedPropertyChangedEventArgs<double> e)
        {

        }


        public static Slider StaticSlider(Slider staticSlider, Slider instanceSlider)
        {
            staticSlider = instanceSlider;
            return staticSlider;
        }

        public static TextBox StaticTextBox(TextBox staticTextBox, TextBox instanceTextBox)
        {
            staticTextBox = instanceTextBox;
            return staticTextBox;
        }

        public static StackPanel StaticStackPanel(StackPanel staticStackPanel, StackPanel instanceStackPanel)
        {
            staticStackPanel = instanceStackPanel;
            return staticStackPanel;
        }

        public static ScrollViewer StaticScroll(ScrollViewer staticScroll, ScrollViewer instanceScroll)
        {
            staticScroll = instanceScroll;
            return staticScroll;
        }

        public static UserControl StaticUserControl(UserControl staticUserControl, UserControl instanceUserControl)
        {
            staticUserControl = instanceUserControl;
            return staticUserControl;
        }

        public void AdditionalSliders(NodeModel datModel, int oldCount, int newCount)
        {

            SliderStackPanel_Copy.Visibility = Visibility.Visible;

            for (int i = oldCount; i<newCount; i++)
            {

                

                StackPanel newDeepCopySP = StackPanelDeepCopy(SliderStackPanel_Copy);
                newDeepCopySP.Children.Clear();


                

//              AddTextBox(datModel, i, newDeepCopySP);
                AddSlider(datModel, i, newDeepCopySP, this);

                stackPanel_AllSlidersStatic.Children.Add(newDeepCopySP);


            }

        }


        public void DeleteSliders(NodeModel datModel, int oldCount, int newCount, SliderControl sliderControl)
        {
            for (int i = (oldCount-2); i > (newCount-2); i--)
            {
                DeleteSlider(datModel, i);
                stackPanel_AllSlidersStatic.Height -= SliderStackPanel_Copy.Height;
                SliderScroll.Height -= SliderStackPanel_Copy.Height;
                MultiSliderUserControl.Height -= SliderStackPanel_Copy.Height;  


            }

        }

        public  void AddSlider(NodeModel datModel, int index, StackPanel sliderSP, SliderControl sliderControl)
        {


            SliderCustomNodeModel.sliderValueList.Add(0);

            TextBox newTextBoxDeepCopy = TextBoxDeepCopy(sliderControl.textBoxDebug);
            sliderSP.Children.Add(newTextBoxDeepCopy);

            Slider newDeepCopy = SliderDeepCopy(sliderDebug);
            sliderSP.Children.Add(newDeepCopy);

            SliderINotifyModel newDataObject = new SliderINotifyModel();
            newDataObject.sliderCusModel = datModel as SliderCustomNodeModel;
            newDataObject.index = index;
            Binding newBinding = new Binding("MovedSliderProp");
            newBinding.Source = newDataObject;
            newBinding.Mode = BindingMode.TwoWay;
//           newBinding.UpdateSourceTrigger = UpdateSourceTrigger.LostFocus;
            // Bind the new data source to the myText TextBlock control's Text dependency property.
            newDeepCopy.SetBinding(Slider.ValueProperty, newBinding);
            newTextBoxDeepCopy.SetBinding(TextBox.TextProperty, newBinding);

            newDataObject.sliderAssigned = newDeepCopy;
            newDataObject.textBoxAssigned = newTextBoxDeepCopy;
            sliderINotifyModelList.Add(newDataObject);

            stackPanel_AllSlidersStatic.Height += SliderStackPanel_Copy.Height;
            SliderScroll.Height += SliderStackPanel_Copy.Height;
            MultiSliderUserControl.Height += SliderStackPanel_Copy.Height;

        }

        public static void DeleteSlider (NodeModel datModel, int index)
        {
            stackPanel_AllSlidersStatic.Children.RemoveAt(index);
            SliderCustomNodeModel.sliderValueList.RemoveAt(index);
        }

        public static void AddTextBox(NodeModel datModel, int index, StackPanel sliderSP)// can be commented out
        {
            TextBox newTextBoxDeepCopy = TextBoxDeepCopy(textBoxDebugStatic);
            sliderSP.Children.Add(newTextBoxDeepCopy);

            //double movedSlider = new double();
            SliderINotifyModel newDataObject = new SliderINotifyModel();
            newDataObject.sliderCusModel = datModel as SliderCustomNodeModel;
            newDataObject.index = index;
            Binding newBinding = new Binding("MovedSliderProp");
            newBinding.Source = newDataObject;
            newBinding.Mode = BindingMode.TwoWay;
 
            newBinding.UpdateSourceTrigger =  UpdateSourceTrigger.LostFocus;
            // Bind the new data source to the myText TextBlock control's Text dependency property.
            newTextBoxDeepCopy.SetBinding(TextBox.TextProperty, newBinding);
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

 //                  newSlider.ValueChanged += Slider_ValueChanged;



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

        public static TextBox TextBoxDeepCopy(TextBox element)
        {
            string shapestring = XamlWriter.Save(element);
            StringReader stringReader = new StringReader(shapestring);
            XmlTextReader xmlTextReader = new XmlTextReader(stringReader);
            TextBox DeepCopyobject = (TextBox)XamlReader.Load(xmlTextReader);
            return DeepCopyobject;

        }

        public static StackPanel StackPanelDeepCopy(StackPanel element)
        {
            string shapestring = XamlWriter.Save(element);
            StringReader stringReader = new StringReader(shapestring);
            XmlTextReader xmlTextReader = new XmlTextReader(stringReader);
            StackPanel DeepCopyobject = (StackPanel)XamlReader.Load(xmlTextReader);
            return DeepCopyobject;

        }

    }





}
