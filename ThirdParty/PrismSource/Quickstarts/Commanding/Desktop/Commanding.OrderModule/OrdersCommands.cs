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
using Microsoft.Practices.Prism.Commands;

namespace Commanding.Modules.Order
{
    /// <summary>
    /// Defines the SaveAll command. This command is defined as a static so
    /// that it can be easily accessed throughout the application.
    /// </summary>
    public static class OrdersCommands
    {
        //TODO: 03 - The application defines a global SaveAll command which invokes the Save command on all registered Orders. It is enabled only when all orders can be saved.
        public static CompositeCommand SaveAllOrdersCommand = new CompositeCommand();
    }

    /// <summary>
    /// Provides a class wrapper around the static SaveAll command.
    /// </summary>
    public class OrdersCommandProxy
    {
        public virtual CompositeCommand SaveAllOrdersCommand
        {
            get { return OrdersCommands.SaveAllOrdersCommand; }
        }
    }
}
