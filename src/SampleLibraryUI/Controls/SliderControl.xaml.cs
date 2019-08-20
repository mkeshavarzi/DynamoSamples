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

        public void  AdditionalSliders(NodeModel datModel, int oldCount, int newCount, SliderControl sliderControl, SliderCustomNodeModel sliderCustomModel)
        {            
            SliderStackPanel_Copy.Visibility = Visibility.Visible;
            for (int i = oldCount; i<newCount; i++)
            {
                StackPanel newDeepCopySP = StackPanelDeepCopy(SliderStackPanel_Copy);
                newDeepCopySP.Children.Clear();
                AddSlider(datModel, i, newDeepCopySP, sliderControl, sliderCustomModel);
                SliderStackPanel_AllSliders.Children.Add(newDeepCopySP);
            }

        }

        public void DeleteSliders(NodeModel datModel, int oldCount, int newCount, SliderControl sliderControl, SliderCustomNodeModel sliderCusModel)
        {
            for (int i = (oldCount-2); i > (newCount-2); i--)
            {
                DeleteSlider(sliderCusModel, i);
                SliderStackPanel_AllSliders.Height -= SliderStackPanel_Copy.Height;
                SliderPanel.Height -= SliderStackPanel_Copy.Height;

                if (i < 3)
                {
                    MultiSliderUserControl.Height -= SliderStackPanel_Copy.Height;
                    SliderScroll.Height -= SliderStackPanel_Copy.Height;
                }
            }

        }

        public  void AddSlider(NodeModel datModel, int index, StackPanel sliderSP, SliderControl sliderControl, SliderCustomNodeModel sliderCusNodeModel)
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
            newDataObject.sliderCusModel = sliderCusNodeModel;
            newDataObject.newCount = sliderCusNodeModel.newCount;
            Binding newBinding = new Binding("MovedSliderProp");
            newBinding.Source = newDataObject;
            newBinding.Mode = BindingMode.TwoWay;
            newDeepCopy.SetBinding(Slider.ValueProperty, newBinding);
            newTextBoxDeepCopy.SetBinding(TextBox.TextProperty, newBinding);

            newDataObject.sliderAssigned = newDeepCopy;
            newDataObject.textBoxAssigned = newTextBoxDeepCopy;

            sliderCusNodeModel.sliderValueList.Add(newSliderValue);
            newTextBoxDeepCopy.Text = newSliderValue.ToString();
            newDeepCopy.Value = newSliderValue;

            sliderCusNodeModel.iNotSlidersModel.Add(newDataObject);

            SliderStackPanel_AllSliders.Height += SliderStackPanel_Copy.Height;
            SliderPanel.Height += SliderStackPanel_Copy.Height;

            if (index < 4)
            {
                MultiSliderUserControl.Height += SliderStackPanel_Copy.Height;
                SliderScroll.Height += SliderStackPanel_Copy.Height;
            }
        }

        public void DeleteSlider (SliderCustomNodeModel sliderCusModel, int index)
        {
            SliderStackPanel_AllSliders.Children.RemoveAt(index);
            sliderCusModel.sliderValueList.RemoveAt(index+1);
            sliderCusModel.iNotSlidersModel[index].sliderCusModel.SiderValueCollection.RemoveAt(index+1);

        }

        public Slider SliderDeepCopy(Slider element)
        {
            string shapestring = XamlWriter.Save(element);
            StringReader stringReader = new StringReader(shapestring);
            XmlTextReader xmlTextReader = new XmlTextReader(stringReader);
            Slider DeepCopyobject = (Slider)XamlReader.Load(xmlTextReader);
            return DeepCopyobject;

        }

        public TextBox TextBoxDeepCopy(TextBox element)
        {
            string shapestring = XamlWriter.Save(element);
            StringReader stringReader = new StringReader(shapestring);
            XmlTextReader xmlTextReader = new XmlTextReader(stringReader);
            TextBox DeepCopyobject = (TextBox)XamlReader.Load(xmlTextReader);
            return DeepCopyobject;

        }

        public StackPanel StackPanelDeepCopy(StackPanel element)
        {
            string shapestring = XamlWriter.Save(element);
            StringReader stringReader = new StringReader(shapestring);
            XmlTextReader xmlTextReader = new XmlTextReader(stringReader);
            StackPanel DeepCopyobject = (StackPanel)XamlReader.Load(xmlTextReader);
            return DeepCopyobject;

        }

        public void UpdateMin (double min)
        {
            sliderDebug.Minimum = min;
        }

        public void UpdateMax(double max)
        {
            sliderDebug.Maximum = max;
        }

        public void UpdateStep(double step)
        {
            sliderDebug.TickFrequency = step;
        }
    }





}
