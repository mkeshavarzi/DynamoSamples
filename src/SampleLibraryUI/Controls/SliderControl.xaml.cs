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
        public SliderControl()
        {
            InitializeComponent(); 
        }

        private void Slider_ValueChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<double> e)

        {

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

        public void  AdditionalSliders(NodeModel datModel, int oldCount, int newCount)
        {            
            SliderStackPanel_Copy.Visibility = Visibility.Visible;
            for (int i = oldCount; i<newCount; i++)
            {
                StackPanel newDeepCopySP = StackPanelDeepCopy(SliderStackPanel_Copy);
                newDeepCopySP.Children.Clear();
                AddSlider(datModel, i, newDeepCopySP, this);
                SliderStackPanel_AllSliders.Children.Add(newDeepCopySP);
            }

        }

        public void DeleteSliders(NodeModel datModel, int oldCount, int newCount, SliderControl sliderControl)
        {
            for (int i = (oldCount-2); i > (newCount-2); i--)
            {
                DeleteSlider(datModel, i);
                SliderStackPanel_AllSliders.Height -= SliderStackPanel_Copy.Height;
                SliderPanel.Height -= SliderStackPanel_Copy.Height;

                if (i < 3)
                {
                    MultiSliderUserControl.Height -= SliderStackPanel_Copy.Height;
                    SliderScroll.Height -= SliderStackPanel_Copy.Height;
                }
            }

        }

        public  void AddSlider(NodeModel datModel, int index, StackPanel sliderSP, SliderControl sliderControl)
        {
            Double newSliderValue = 1;
            TextBox newTextBoxDeepCopy = TextBoxDeepCopy(sliderControl.textBoxDebug);
            sliderSP.Children.Add(newTextBoxDeepCopy);

            Slider newDeepCopy = SliderDeepCopy(sliderDebug);
            sliderSP.Children.Add(newDeepCopy);

            SliderINotifyModel newDataObject = new SliderINotifyModel();
            newDataObject.sliderCusModel = datModel as SliderCustomNodeModel;

            if(newDataObject.sliderCusModel.SiderValueCollection.Count < index+1)
            {
                newDataObject.sliderCusModel.SiderValueCollection.Add(newSliderValue);
            }

            newSliderValue = newDataObject.sliderCusModel.SiderValueCollection[index];
            newDataObject.index = index;
            Binding newBinding = new Binding("MovedSliderProp");
            newBinding.Source = newDataObject;
            newBinding.Mode = BindingMode.TwoWay;
            newDeepCopy.SetBinding(Slider.ValueProperty, newBinding);
            newTextBoxDeepCopy.SetBinding(TextBox.TextProperty, newBinding);

            newDataObject.sliderAssigned = newDeepCopy;
            newDataObject.textBoxAssigned = newTextBoxDeepCopy;

            SliderCustomNodeModel.sliderValueList.Add(newSliderValue);
            newTextBoxDeepCopy.Text = newSliderValue.ToString();
            newDeepCopy.Value = newSliderValue;

            SliderCustomNodeModel.INotifySliderModels.Add(newDataObject);

            SliderStackPanel_AllSliders.Height += SliderStackPanel_Copy.Height;
            SliderPanel.Height += SliderStackPanel_Copy.Height;

            if (index < 4)
            {
                MultiSliderUserControl.Height += SliderStackPanel_Copy.Height;
                SliderScroll.Height += SliderStackPanel_Copy.Height;
            }
        }

        public void DeleteSlider (NodeModel datModel, int index)
        {
            SliderStackPanel_AllSliders.Children.RemoveAt(index);
            SliderCustomNodeModel.sliderValueList.RemoveAt(index+1);
            SliderCustomNodeModel.INotifySliderModels[index].sliderCusModel.SiderValueCollection.RemoveAt(index+1);

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
