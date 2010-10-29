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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using Microsoft.Practices.Prism.Events;

using Commanding.Modules.Order.Services;
using Commanding.Modules.Order.Views;

namespace Commanding.Modules.Order.PresentationModels
{
    /// <summary>
    /// Presentation model to support the OrdersEditorView.
    /// </summary>
    public class OrdersEditorPresentationModel : INotifyPropertyChanged
    {
        private readonly IOrdersRepository  ordersRepository;
        private readonly OrdersCommandProxy commandProxy;

        private ObservableCollection<OrderPresentationModel> _orders { get; set; }

        public OrdersEditorPresentationModel( IOrdersRepository ordersRepository, OrdersCommandProxy commandProxy )
        {
            this.ordersRepository = ordersRepository;
            this.commandProxy     = commandProxy;

            // Create dummy order data.
            this.PopulateOrders();

            // Initialize a CollectionView for the underlying Orders collection.
#if SILVERLIGHT
            this.Orders = new PagedCollectionView( _orders );
#else
            this.Orders = new ListCollectionView( _orders );
#endif
            // Track the current selection.
            this.Orders.CurrentChanged += SelectedOrderChanged;
        }

        public ICollectionView Orders { get; private set; }

        private void SelectedOrderChanged( object sender, EventArgs e )
        {
            SelectedOrder = Orders.CurrentItem as OrderPresentationModel;
            NotifyPropertyChanged( "SelectedOrder" );
        }

        public OrderPresentationModel SelectedOrder { get; private set; }

        private void PopulateOrders()
        {
            _orders = new ObservableCollection<OrderPresentationModel>();

            foreach ( Services.Order order in this.ordersRepository.GetOrdersToEdit() )
            {
                // Wrap the Order object in a presentation model object.
                var orderPresentationModel = new OrderPresentationModel( order );
                _orders.Add( orderPresentationModel );

                // Subscribe to the Save event on the individual orders.
                orderPresentationModel.Saved += this.OrderSaved;

                //TODO: 04 - Each Order Save command is registered with the application's SaveAll command.
                commandProxy.SaveAllOrdersCommand.RegisterCommand( orderPresentationModel.SaveOrderCommand );
            }
        }

        private void OrderSaved( object sender, DataEventArgs<OrderPresentationModel> e )
        {
            if (e != null && e.Value != null)
            {
                OrderPresentationModel order = e.Value;
                if ( this.Orders.Contains( order ) )
                {
                    order.Saved -= this.OrderSaved;
                    //TODO: 05 - As each order is saved, it is unregistered from the application's SaveAll command.
                    this.commandProxy.SaveAllOrdersCommand.UnregisterCommand( order.SaveOrderCommand );
                    // Remove saved orders from the collection.
                    this._orders.Remove( order );
                }
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged( string propertyName )
        {
            if ( this.PropertyChanged != null )
            {
                this.PropertyChanged( this, new PropertyChangedEventArgs( propertyName ) );
            }
        }

        #endregion
    }
}
