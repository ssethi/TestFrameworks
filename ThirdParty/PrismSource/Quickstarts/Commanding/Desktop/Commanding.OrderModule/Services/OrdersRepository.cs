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
using System.Globalization;

namespace Commanding.Modules.Order.Services
{
    public class OrdersRepository : IOrdersRepository
    {
        private const int InitialOrdersCount = 3;

        public IEnumerable<Order> GetOrdersToEdit()
        {
            var orders = new List<Order>( InitialOrdersCount );
            for ( int i = 1 ; i <= InitialOrdersCount ; i++ )
            {
                string orderName = String.Format(CultureInfo.CurrentCulture, "Order {0}", i);
                orders.Add( new Order { Name = orderName, DeliveryDate = DateTime.Today.AddDays(i) } );
            }

            return orders;
        }
    }
}