﻿#pragma checksum "..\..\..\МануалWindow.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "E67F3FF099BFA2B95ADB97F01CF0F4CF8F018068"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
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
using System.Windows.Controls.Ribbon;
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


namespace Финансы {
    
    
    /// <summary>
    /// МануалWindow
    /// </summary>
    public partial class МануалWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 18 "..\..\..\МануалWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox tbПоискСкладов;
        
        #line default
        #line hidden
        
        
        #line 32 "..\..\..\МануалWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox tbНазваниеСклада;
        
        #line default
        #line hidden
        
        
        #line 42 "..\..\..\МануалWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox tbАдресСклада;
        
        #line default
        #line hidden
        
        
        #line 55 "..\..\..\МануалWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid dgСклады;
        
        #line default
        #line hidden
        
        
        #line 70 "..\..\..\МануалWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox tbПоискТоваров;
        
        #line default
        #line hidden
        
        
        #line 84 "..\..\..\МануалWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox tbНазваниеТовара;
        
        #line default
        #line hidden
        
        
        #line 94 "..\..\..\МануалWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox tbЕдИзм;
        
        #line default
        #line hidden
        
        
        #line 104 "..\..\..\МануалWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox tbЦена;
        
        #line default
        #line hidden
        
        
        #line 117 "..\..\..\МануалWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid dgТовары;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "9.0.2.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Финансы;component/%d0%9c%d0%b0%d0%bd%d1%83%d0%b0%d0%bbwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\МануалWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "9.0.2.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.tbПоискСкладов = ((System.Windows.Controls.TextBox)(target));
            
            #line 19 "..\..\..\МануалWindow.xaml"
            this.tbПоискСкладов.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.tbПоискСкладов_TextChanged);
            
            #line default
            #line hidden
            return;
            case 2:
            this.tbНазваниеСклада = ((System.Windows.Controls.TextBox)(target));
            return;
            case 3:
            this.tbАдресСклада = ((System.Windows.Controls.TextBox)(target));
            return;
            case 4:
            
            #line 50 "..\..\..\МануалWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.ДобавитьСклад_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            
            #line 51 "..\..\..\МануалWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.УдалитьСклад_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.dgСклады = ((System.Windows.Controls.DataGrid)(target));
            return;
            case 7:
            this.tbПоискТоваров = ((System.Windows.Controls.TextBox)(target));
            
            #line 71 "..\..\..\МануалWindow.xaml"
            this.tbПоискТоваров.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.tbПоискТоваров_TextChanged);
            
            #line default
            #line hidden
            return;
            case 8:
            this.tbНазваниеТовара = ((System.Windows.Controls.TextBox)(target));
            return;
            case 9:
            this.tbЕдИзм = ((System.Windows.Controls.TextBox)(target));
            return;
            case 10:
            this.tbЦена = ((System.Windows.Controls.TextBox)(target));
            
            #line 104 "..\..\..\МануалWindow.xaml"
            this.tbЦена.KeyDown += new System.Windows.Input.KeyEventHandler(this.tbЦена_KeyDown);
            
            #line default
            #line hidden
            return;
            case 11:
            
            #line 112 "..\..\..\МануалWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.ДобавитьТовар_Click);
            
            #line default
            #line hidden
            return;
            case 12:
            
            #line 113 "..\..\..\МануалWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.УдалитьТовар_Click);
            
            #line default
            #line hidden
            return;
            case 13:
            this.dgТовары = ((System.Windows.Controls.DataGrid)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

