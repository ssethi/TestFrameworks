//===================================================================================
// Microsoft patterns & practices
// Composite Application Guidance for Windows Presentation Foundation and Silverlight
//===================================================================================
// Copyright (c) Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===================================================================================
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
//===================================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AcceptanceTestLibrary.Common;
using System.Windows.Automation;
using AcceptanceTestLibrary.TestEntityBase;


namespace ModularityWithMef.Tests.AcceptanceTest.TestEntities.Page
{
    public static class ModularityPage<TApp>
       where TApp : AppLauncherBase, new()
    {

        #region Desktop
        public static AutomationElement Window
        {
            get { return PageBase<TApp>.Window; }
            set { PageBase<TApp>.Window = value; }
        }

        public static AutomationElement ModuleA
        {
            get { return PageBase<TApp>.FindControlByAutomationId("ModuleA"); }
        }

        public static AutomationElement ModuleB
        {
            get { return PageBase<TApp>.FindControlByAutomationId("ModuleB"); }
        }

        public static AutomationElement ModuleC
        {
            get { return PageBase<TApp>.FindControlByAutomationId("ModuleC"); }
        }
        public static AutomationElement ModuleE
        {
            get { return PageBase<TApp>.FindControlByAutomationId("ModuleE"); }
        }
        public static AutomationElement ModuleD
        {
            get { return PageBase<TApp>.FindControlByAutomationId("ModuleD"); }
        }
        public static AutomationElement ModuleF
        {
            get { return PageBase<TApp>.FindControlByAutomationId("ModuleF"); }
        }
        public static AutomationElementCollection Modules
        {
            get { return PageBase<TApp>.FindAllControlsByAutomationId("Modules"); }
        }

        public static AutomationElement ModuleAContent
        {
            get { return PageBase<TApp>.FindControlByContent("ModuleACon"); }
        }

        public static AutomationElement ModuleDContent
        {
            get { return PageBase<TApp>.FindControlByContent("ModuleDCon"); }
        }

        public static AutomationElement ModuleCContent
        {
            get { return PageBase<TApp>.FindControlByContent("ModuleCCon"); }
        }

        public static AutomationElement ModuleFContent
        {
            get { return PageBase<TApp>.FindControlByContent("ModuleFCon"); }
        }
        public static AutomationElement ModuleEContent
        {
            get { return PageBase<TApp>.FindControlByContent("ModuleECon"); }
        }
        public static AutomationElement IntializedModuletooltip
        {
            get { return PageBase<TApp>.FindControlByContent("ModAtooltip"); }
        }
        public static AutomationElement DownloadedModuletooltip
        {
            get { return PageBase<TApp>.FindControlByContent("Downloadedtooltip"); }
        }
        public static AutomationElement ModuleBContent
        {
            get { return PageBase<TApp>.FindControlByContent("ModuleBCon"); }
        }
        #endregion

    }
}
