﻿#pragma checksum "..\..\EdytorNut.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "95C116B2E00DC1C9AA8E24F96664B543"
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


namespace Syntezator_Krawczyka {
    
    
    /// <summary>
    /// EdytorNut
    /// </summary>
    public partial class EdytorNut : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 7 "..\..\EdytorNut.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid panel;
        
        #line default
        #line hidden
        
        
        #line 13 "..\..\EdytorNut.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox czas;
        
        #line default
        #line hidden
        
        
        #line 14 "..\..\EdytorNut.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox dlugosc;
        
        #line default
        #line hidden
        
        
        #line 15 "..\..\EdytorNut.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox ton;
        
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
            System.Uri resourceLocater = new System.Uri("/Syntezator Krawczyka;component/edytornut.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\EdytorNut.xaml"
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
            this.panel = ((System.Windows.Controls.Grid)(target));
            return;
            case 2:
            this.czas = ((System.Windows.Controls.TextBox)(target));
            
            #line 13 "..\..\EdytorNut.xaml"
            this.czas.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.czas_TextChanged);
            
            #line default
            #line hidden
            return;
            case 3:
            this.dlugosc = ((System.Windows.Controls.TextBox)(target));
            
            #line 14 "..\..\EdytorNut.xaml"
            this.dlugosc.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.dlugosc_TextChanged);
            
            #line default
            #line hidden
            return;
            case 4:
            this.ton = ((System.Windows.Controls.TextBox)(target));
            
            #line 15 "..\..\EdytorNut.xaml"
            this.ton.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.ton_TextChanged);
            
            #line default
            #line hidden
            return;
            case 5:
            
            #line 16 "..\..\EdytorNut.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

