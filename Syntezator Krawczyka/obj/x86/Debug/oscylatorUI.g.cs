﻿#pragma checksum "..\..\..\oscylatorUI.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "C18DAE801AF8BBAC2D070AE23CB7C4BD"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18051
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Syntezator_Krawczyka;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace Syntezator_Krawczyka.Synteza {
    
    
    /// <summary>
    /// oscylatorUI
    /// </summary>
    public partial class oscylatorUI : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 9 "..\..\..\oscylatorUI.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton sinusoidalny;
        
        #line default
        #line hidden
        
        
        #line 10 "..\..\..\oscylatorUI.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton trójkątny;
        
        #line default
        #line hidden
        
        
        #line 11 "..\..\..\oscylatorUI.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton prostokątny;
        
        #line default
        #line hidden
        
        
        #line 12 "..\..\..\oscylatorUI.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton piłokształtny;
        
        #line default
        #line hidden
        
        
        #line 13 "..\..\..\oscylatorUI.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton radioButton1;
        
        #line default
        #line hidden
        
        
        #line 14 "..\..\..\oscylatorUI.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Slider slider1;
        
        #line default
        #line hidden
        
        
        #line 15 "..\..\..\oscylatorUI.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Slider sliderA;
        
        #line default
        #line hidden
        
        
        #line 16 "..\..\..\oscylatorUI.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Slider sliderD;
        
        #line default
        #line hidden
        
        
        #line 17 "..\..\..\oscylatorUI.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Slider sliderS;
        
        #line default
        #line hidden
        
        
        #line 18 "..\..\..\oscylatorUI.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Slider sliderR;
        
        #line default
        #line hidden
        
        
        #line 23 "..\..\..\oscylatorUI.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label label1;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Syntezator Krawczyka;component/oscylatorui.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\oscylatorUI.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 7 "..\..\..\oscylatorUI.xaml"
            ((Syntezator_Krawczyka.Synteza.oscylatorUI)(target)).MouseMove += new System.Windows.Input.MouseEventHandler(this.UserControl_MouseMove);
            
            #line default
            #line hidden
            return;
            case 2:
            this.sinusoidalny = ((System.Windows.Controls.RadioButton)(target));
            
            #line 9 "..\..\..\oscylatorUI.xaml"
            this.sinusoidalny.Checked += new System.Windows.RoutedEventHandler(this.sinusoidalny_Checked);
            
            #line default
            #line hidden
            return;
            case 3:
            this.trójkątny = ((System.Windows.Controls.RadioButton)(target));
            
            #line 10 "..\..\..\oscylatorUI.xaml"
            this.trójkątny.Checked += new System.Windows.RoutedEventHandler(this.trójkątny_Checked);
            
            #line default
            #line hidden
            return;
            case 4:
            this.prostokątny = ((System.Windows.Controls.RadioButton)(target));
            
            #line 11 "..\..\..\oscylatorUI.xaml"
            this.prostokątny.Checked += new System.Windows.RoutedEventHandler(this.prostokątny_Checked);
            
            #line default
            #line hidden
            return;
            case 5:
            this.piłokształtny = ((System.Windows.Controls.RadioButton)(target));
            
            #line 12 "..\..\..\oscylatorUI.xaml"
            this.piłokształtny.Checked += new System.Windows.RoutedEventHandler(this.piłokształtny_Checked);
            
            #line default
            #line hidden
            return;
            case 6:
            this.radioButton1 = ((System.Windows.Controls.RadioButton)(target));
            
            #line 13 "..\..\..\oscylatorUI.xaml"
            this.radioButton1.Checked += new System.Windows.RoutedEventHandler(this.radioButton1_Checked);
            
            #line default
            #line hidden
            return;
            case 7:
            this.slider1 = ((System.Windows.Controls.Slider)(target));
            
            #line 14 "..\..\..\oscylatorUI.xaml"
            this.slider1.ValueChanged += new System.Windows.RoutedPropertyChangedEventHandler<double>(this.slider1_ValueChanged);
            
            #line default
            #line hidden
            return;
            case 8:
            this.sliderA = ((System.Windows.Controls.Slider)(target));
            
            #line 15 "..\..\..\oscylatorUI.xaml"
            this.sliderA.ValueChanged += new System.Windows.RoutedPropertyChangedEventHandler<double>(this.sliderA_ValueChanged);
            
            #line default
            #line hidden
            return;
            case 9:
            this.sliderD = ((System.Windows.Controls.Slider)(target));
            
            #line 16 "..\..\..\oscylatorUI.xaml"
            this.sliderD.ValueChanged += new System.Windows.RoutedPropertyChangedEventHandler<double>(this.sliderD_ValueChanged);
            
            #line default
            #line hidden
            return;
            case 10:
            this.sliderS = ((System.Windows.Controls.Slider)(target));
            
            #line 17 "..\..\..\oscylatorUI.xaml"
            this.sliderS.ValueChanged += new System.Windows.RoutedPropertyChangedEventHandler<double>(this.sliderS_ValueChanged);
            
            #line default
            #line hidden
            return;
            case 11:
            this.sliderR = ((System.Windows.Controls.Slider)(target));
            
            #line 18 "..\..\..\oscylatorUI.xaml"
            this.sliderR.ValueChanged += new System.Windows.RoutedPropertyChangedEventHandler<double>(this.sliderR_ValueChanged);
            
            #line default
            #line hidden
            return;
            case 12:
            this.label1 = ((System.Windows.Controls.Label)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}
