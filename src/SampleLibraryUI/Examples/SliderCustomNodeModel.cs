﻿using System;
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
using System.ComponentModel;
using Dynamo.Graph;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Xml;
using System.Windows.Controls;


namespace SampleLibraryUI.Examples
{
    /*
     * This exmple shows how to create a UI node for Dynamo
     * which loads custom data-bound UI into the node's view
     * at run time. 

     * Nodes with custom UI follow a different loading path
     * than zero touch nodes. The assembly which contains
     * this node needs to be located in the 'nodes' folder in
     * Dynamo in order to be loaded at startup.

     * Dynamo uses the MVVM model of programming, 
     * in which the UI is data-bound to the view model, which
     * exposes data from the underlying model. Custom UI nodes 
     * are a hybrid because NodeModel objects already have an
     * associated NodeViewModel which you should never need to
     * edit. So here we will create a data binding between 
     * properties on our class and our custom UI.
    */

    // The NodeName attribute is what will display on 
    // top of the node in Dynamo
    [NodeName("Slider Pool")]
    // The NodeCategory attribute determines how your
    // node will be organized in the library. You can
    // specify your own category or by default the class
    // structure will be used.  You can no longer 
    // add packages or custom nodes to one of the 
    // built-in OOTB library categories.
    [NodeCategory("SliderPool")]
    // The description will display in the tooltip
    // and in the help window for the node.
    [NodeDescription("Returns a list of sliders with custum count, minimum, maximum and step value")]
    // Specifying InPort and OutPort types simply
    // adds these types to the help window for the
    // node when hovering the name in the library.
    //[InPortTypes("double")]
    [OutPortTypes("List<double>")]
    // Add the IsDesignScriptCompatible attribute to ensure
    // that it gets loaded in Dynamo.
    [IsDesignScriptCompatible]



    public class SliderCustomNodeModel : NodeModel
    {
        #region private members
        [JsonIgnore]
        public double sliderValue = 1;

        [JsonIgnore]
        public List<double> sliderValueList = new List<double>();

        [JsonIgnore]
        private int countValue = 1;

      private double stepValue;
        private double minValue;
        private double maxValue;

        [JsonIgnore]
        public int newCount;

        [JsonIgnore]
        public int slidersCountChange;

        [JsonIgnore]
        public SliderControl multiSliderControl = new SliderControl();

   
        public ObservableCollection<double> sliderValueCollection = new ObservableCollection<double>();

        [JsonIgnore]
        public List<SliderINotifyModel> iNotSlidersModel = new List<SliderINotifyModel>();

        #endregion

        #region properties

        [JsonIgnore]
        public ObservableCollection<double> SiderValueCollection
        {
            get { return sliderValueCollection; }
            set
            {
                sliderValueCollection = value;
            }
        }




        public double MinValue
        {
            get { return minValue; }
            set
            {

                minValue = value;
                RaisePropertyChanged("MinValue");
                foreach (SliderINotifyModel slider in iNotSlidersModel)
                {
                    slider.sliderAssigned.Minimum = minValue;
                    if (slider.sliderGenValue < minValue)
                    {
                        slider.sliderAssigned.Value = minValue;
                    }

                }
                multiSliderControl.UpdateMin(minValue);
                if (SliderValue < minValue) SliderValue = minValue;
                OnNodeModified();
            }
        }


        public double MaxValue
        {
            get { return maxValue; }
            set
            {

                maxValue = value;
                RaisePropertyChanged("MaxValue");
                foreach (SliderINotifyModel slider in iNotSlidersModel)
                {
                    slider.sliderAssigned.Maximum = maxValue;
                    if (slider.sliderGenValue > maxValue)
                    {
                        slider.sliderAssigned.Value = maxValue;
                    }
                }
                multiSliderControl.UpdateMax(maxValue);
                if (SliderValue > maxValue) SliderValue = maxValue;
                OnNodeModified();
            }
        }

        public double StepValue
        {
            get { return stepValue; }
            set
            {

                stepValue = value;  
                RaisePropertyChanged("StepValue");
                foreach (SliderINotifyModel slider in iNotSlidersModel)
                {
                    slider.sliderAssigned.TickFrequency = stepValue;
                    if ((slider.sliderAssigned.Value % stepValue) > (stepValue / 2)) slider.sliderAssigned.Value = Math.Ceiling(slider.sliderAssigned.Value / stepValue) * stepValue;
                    else slider.sliderAssigned.Value = Math.Floor(slider.sliderAssigned.Value / stepValue) * stepValue;
                }

                multiSliderControl.UpdateStep(stepValue);
                if ((SliderValue % stepValue) > (stepValue / 2)) SliderValue = Math.Ceiling(SliderValue / stepValue) * stepValue;
                else SliderValue = Math.Floor(SliderValue / stepValue) * stepValue;

                OnNodeModified();
            }
        }

        public double SliderValue
        {
            get { return sliderValue; }
            set
            {
                sliderValue = value;

                if (sliderValue > maxValue) sliderValue = maxValue;
                if (sliderValue < minValue) sliderValue = minValue;


                RaisePropertyChanged("SliderValue");

                if ((sliderValue % stepValue) > (stepValue / 2)) sliderValue = Math.Ceiling(sliderValue / stepValue) * stepValue;
                else sliderValue = Math.Floor(sliderValue / stepValue) * stepValue;

                if (sliderValueList.Count == 0)
                {
                    sliderValueList.Add(0);
                }
                if (sliderValueList.Count > 0)
                {
                    sliderValueList[0] = sliderValue;
                    sliderValueCollection[0] = sliderValue;
                }
                if (sliderValueCollection.Count == 0)
                {
                    sliderValueCollection.Add(sliderValue);
                }



                OnNodeModified();
            }
        }


        public int CountValue
        {
            get { return countValue; }
            set
            {
                int oldCount = countValue;
                slidersCountChange = value - countValue;
                countValue = value;
                newCount = value;
                RaisePropertyChanged("CountValue");
                if ((slidersCountChange > 0)&& (oldCount > 0))
                {
                    multiSliderControl.AdditionalSliders(this, oldCount, CountValue, multiSliderControl, this);
                }
                if ((slidersCountChange < 0)&& (oldCount > 1))
                {
                    multiSliderControl.DeleteSliders(this, oldCount, CountValue, multiSliderControl, this);
                }
                OnNodeModified();
            }
        }

        #endregion

        #region constructor

        /// <summary>
        /// The constructor for a NodeModel is used to create
        /// the input and output ports and specify the argument
        /// lacing. It gets invoked when the node is added to 
        /// the graph from the library or through copy/paste.
        /// </summary>
        public SliderCustomNodeModel()
        {
            // When you create a UI node, you need to do the
            // work of setting up the ports yourself. To do this,
            // you can populate the InPorts and the OutPorts
            // collections with PortData objects describing your ports.

            // Nodes can have an arbitrary number of inputs and outputs.
            // If you want more ports, just create more PortData objects.
            OutPorts.Add(new PortModel(PortType.Output, this, new PortData(">", "List<Double>")));
            //            OutPorts.Add(new PortModel(PortType.Output, this, new PortData("max value", "returns a 0-100 double value")));
            //            OutPorts.Add(new PortModel(PortType.Output, this, new PortData("min value", "returns a 0-100 double value")));


            // This call is required to ensure that your ports are
            // properly created.
            RegisterAllPorts();

            // The arugment lacing is the way in which Dynamo handles
            // inputs of lists. If you don't want your node to
            // support argument lacing, you can set this to LacingStrategy.Disabled.
            ArgumentLacing = LacingStrategy.Disabled;

            sliderValue = 1;
            countValue = 1;
            stepValue = 1;
            minValue = 0;
            maxValue = 10;


            if (sliderValueCollection.Count == 0) sliderValueCollection.Add(sliderValue);
            if (sliderValueList.Count == 0) sliderValueList.Add(sliderValue);

        }

        // Starting with Dynamo v2.0 you must add Json constructors for all nodeModel
        // dervived nodes to support the move from an Xml to Json file format.  Failing to
        // do so will result in incorrect ports being generated upon serialization/deserialization.
        // This constructor is called when opening a Json graph.
        [JsonConstructor]
        SliderCustomNodeModel(IEnumerable<PortModel> inPorts, IEnumerable<PortModel> outPorts) : base(inPorts, outPorts)
        {
            sliderValueCollection.CollectionChanged += SliderItems_CollectionChanged;
        }

        private void SliderItems_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            RaisePropertyChanged("MovedSliderProp");
            OnNodeModified();
        }

        #endregion

        #region public methods

        /// <summary>
        /// BuildOutputAst is where the outputs of this node are calculated.
        /// This method is used to do the work that a compiler usually does 
        /// by parsing the inputs List inputAstNodes into an abstract syntax tree.
        /// </summary>
        /// <param name="inputAstNodes"></param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(false)]
        public override IEnumerable<AssociativeNode> BuildOutputAst(List<AssociativeNode> inputAstNodes)
        {
            // When you create your own UI node you are responsible
            // for generating the abstract syntax tree (AST) nodes which
            // specify what methods are called, or how your data is passed
            // when execution occurs.

            // WARNING!!!
            // Do not throw an exception during AST creation. If you
            // need to convey a failure of this node, then use
            // AstFactory.BuildNullNode to pass out null.

            // We create a DoubleNode to wrap the value 'sliderValue' that
            // we've stored in a private member.





            List<AssociativeNode> sliderValueAssoList = new List<AssociativeNode>();
            foreach (double value in sliderValueList)
            {
                var member = AstFactory.BuildDoubleNode(value);
                sliderValueAssoList.Add(member);

            }

            var sliderValueListAST = AstFactory.BuildExprList(sliderValueAssoList);

            var doubleNode = AstFactory.BuildDoubleNode(sliderValue);



            // A FunctionCallNode can be used to represent the calling of a 
            // function in the AST. The method specified here must live in 
            // a separate assembly and have been loaded by Dynamo at the time 
            // that this AST is built. If the method can't be found, you'll get 
            // a "De-referencing a non-pointer warning."

            var funcNode = AstFactory.BuildFunctionCall(
                new Func<double, double>(SampleUtilities.MultiplyInputByNumber),
                new List<AssociativeNode>() { doubleNode });

            // Using the AstFactory class, we can build AstNode objects
            // that assign doubles, assign function calls, build expression lists, etc.
            return new[]
            {
                // In these assignments, GetAstIdentifierForOutputIndex finds 
                // the unique identifier which represents an output on this node
                // and 'assigns' that variable the expression that you create.

                // For the first node, we'll just pass through the 
                //input provided to this node.
                AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(1), sliderValueListAST),


//                AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(1), AstFactory.BuildDoubleNode(sliderValue)),

                // For the second node, we'll build a double node that 
                // passes along our value for multipled value.


 //               AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(1), funcNode)
            };
        }

        #endregion
    }

   
    public class SliderINotifyModel : INotifyPropertyChanged
    {

        public double sliderGenValue;
        public SliderCustomNodeModel sliderCusModel;
        public int index;
        public Slider sliderAssigned;
        public TextBox textBoxAssigned;
        public SliderINotifyModel() { }
        public int newCount;

        public double MovedSliderProp
        {
            get { return sliderGenValue; }
            set
            {
                sliderGenValue = value;
                OnPropertyChanged("MovedSliderProp");

                if ((sliderGenValue % sliderCusModel.StepValue) > (sliderCusModel.StepValue / 2)) sliderGenValue =  Math.Ceiling(sliderGenValue / sliderCusModel.StepValue) * sliderCusModel.StepValue;
                else sliderGenValue = Math.Floor(sliderGenValue / sliderCusModel.StepValue) * sliderCusModel.StepValue;

                if (sliderGenValue > sliderCusModel.MaxValue) sliderGenValue = sliderCusModel.MaxValue;
                if (sliderGenValue < sliderCusModel.MinValue) sliderGenValue = sliderCusModel.MinValue;




                if (sliderCusModel.sliderValueList.Count < (index+1))
                {
                    sliderCusModel.sliderValueList.Add(sliderGenValue);
                    sliderCusModel.SiderValueCollection.Add(sliderGenValue);
                }

                if (sliderCusModel.sliderValueList.Count >= 2)
                {
                    sliderCusModel.sliderValueList[index] = sliderGenValue;
                    sliderCusModel.SiderValueCollection[index] = sliderGenValue;
                }


                sliderCusModel.OnNodeModified(true);                
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string info)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(info));
            }
        }

    }


    /// <summary>
    ///     View customizer for CustomNodeModel Node Model.
    /// </summary>
    public class SliderCustomNodeModelNodeViewCustomization : INodeViewCustomization<SliderCustomNodeModel>
    {
        /// <summary>
        /// At run-time, this method is called during the node 
        /// creation. Here you can create custom UI elements and
        /// add them to the node view, but we recommend designing
        /// your UI declaratively using xaml, and binding it to
        /// properties on this node as the DataContext.
        /// </summary>
        /// <param name="model">The NodeModel representing the node's core logic.</param>
        /// <param name="nodeView">The NodeView representing the node in the graph.</param> 

        public void CustomizeView(SliderCustomNodeModel model, NodeView nodeView)
        {
            // The view variable is a reference to the node's view.
            // In the middle of the node is a grid called the InputGrid.
            // We reccommend putting your custom UI in this grid, as it has
            // been designed for this purpose.

            // Create an instance of our custom UI class (defined in xaml),
            // and put it into the input grid.
            //                       SliderCustomNodeModel sliderModel = new SliderCustomNodeModel();
            //                       SliderControl sliderControl = sliderModel.multiSliderControl;
            //           var sliderControl = new SliderControl();
            var sliderControl = model.multiSliderControl;
            nodeView.inputGrid.Children.Add(sliderControl);

            // Set the data context for our control to be the node model.
            // Properties in this class which are data bound will raise 
            // property change notifications which will update the UI.
            sliderControl.DataContext = model;

        }

        /// <summary>
        /// Here you can do any cleanup you require if you've assigned callbacks for particular 
        /// UI events on your node.
        /// </summary>
        public void Dispose() { }
    }

}
