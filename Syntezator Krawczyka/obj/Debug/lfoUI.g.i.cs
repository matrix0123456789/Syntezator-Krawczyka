﻿#pragma checksum "..\..\lfoUI.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "AF38533E5CF9DAB01135AD8F1582F2D0"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34011
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

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
    /// lfoUI
    /// </summary>
    public partial class lfoUI : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 9 "..\..\lfoUI.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Slider sliderA;
        
        #line default
        #line hidden
        
        
        #line 10 "..\..\lfoUI.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Slider sliderB;
        
        #line default
        #line hidden
        
        
        #line 11 "..\..\lfoUI.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Slider sliderC;
        
        #line default
        #line hidden
        
        
        #line 12 "..\..\lfoUI.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label label1;
        
        #line default
        #line hidden
        
        
        #line 13 "..\..\lfoUI.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton sinusoidalny;
        
        #line default
        #line hidden
        
        
        #line 14 "..\..\lfoUI.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton trójkątny;
        
        #line default
        #line hidden
        
        
        #line 15 "..\..\lfoUI.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton prostokątny;
        
        #line default
        #line hidden
        
        
        #line 16 "..\..\lfoUI.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton piłokształtny;
        
        #line default
        #line hidden
        
        
        #line 17 "..\..\lfoUI.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid DoOscylatora;
        
        #line default
        #line hidden
        
        
        #line 20 "..\..\lfoUI.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Slider sliderD;
        
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
            System.Uri resourceLocater = new System.Uri("/Syntezator Krawczyka;component/lfoui.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\lfoUI.xaml"
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
            
            #line 7 "..\..\lfoUI.xaml"
            ((Syntezator_Krawczyka.Synteza.lfoUI)(target)).MouseMove += new System.Windows.Input.MouseEventHandler(this.UserControl_MouseMove);
            
            #line default
            #line hidden
            return;
            case 2:
            this.sliderA = ((System.Windows.Controls.Slider)(target));
            
            #line 9 "..\..\lfoUI.xaml"
            this.sliderA.ValueChanged += new System.Windows.RoutedPropertyChangedEventHandler<double>(this.slider1_ValueChanged);
            
            #line default
            #line hidden
            return;
            case 3:
            this.sliderB = ((System.Windows.Controls.Slider)(target));
            
            #line 10 "..\..\lfoUI.xaml"
            this.sliderB.ValueChanged += new System.Windows.RoutedPropertyChangedEventHandler<double>(this.slider2_ValueChanged);
            
            #line default
            #line hidden
            return;
            case 4:
            this.sliderC = ((System.Windows.Controls.Slider)(target));
            
            #line 11 "..\..\lfoUI.xaml"
            this.sliderC.ValueChanged += new System.Windows.RoutedPropertyChangedEventHandler<double>(this.slider3_ValueChanged);
            
            #line default
            #line hidden
            return;
            case 5:
            this.label1 = ((System.Windows.Controls.Label)(target));
            return;
            case 6:
            this.sinusoidalny = ((System.Windows.Controls.RadioButton)(target));
            
            #line 13 "..\..\lfoUI.xaml"
            this.sinusoidalny.Checked += new System.Windows.RoutedEventHandler(this.sinusoidalny_Checked);
            
            #line default
            #line hidden
            return;
            case 7:
            this.trójkątny = ((System.Windows.Controls.RadioButton)(target));
            
            #line 14 "..\..\lfoUI.xaml"
            this.trójkątny.Checked += new System.Windows.RoutedEventHandler(this.trójkątny_Checked);
            
            #line default
            #line hidden
            return;
            case 8:
            this.prostokątny = ((System.Windows.Controls.RadioButton)(target));
            
            #line 15 "..\..\lfoUI.xaml"
            this.prostokątny.Checked += new System.Windows.RoutedEventHandler(this.prostokątny_Checked);
            
            #line default
            #line hidden
            return;
            case 9:
            this.piłokształtny = ((System.Windows.Controls.RadioButton)(target));
            
            #line 16 "..\..\lfoUI.xaml"
            this.piłokształtny.Checked += new System.Windows.RoutedEventHandler(this.piłokształtny_Checked);
            
            #line default
            #line hidden
            return;
            case 10:
            this.DoOscylatora = ((System.Windows.Controls.Grid)(target));
            return;
            case 11:
            this.sliderD = ((System.Windows.Controls.Slider)(target));
            
            #line 20 "..\..\lfoUI.xaml"
            this.sliderD.ValueChanged += new System.Windows.RoutedPropertyChangedEventHandler<double>(this.slider4_ValueChanged);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

