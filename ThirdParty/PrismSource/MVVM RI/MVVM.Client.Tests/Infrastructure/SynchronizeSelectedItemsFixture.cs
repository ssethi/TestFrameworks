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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Interactivity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MVVM.Client.Infrastructure.Behaviors;

namespace MVVM.Client.Tests.Infrastructure
{
    [TestClass]
    public class SynchronizeSelectedItemsFixture
    {
        [TestMethod]
        public void WhenBehaviorIsAttached_ThenItSynchronizesTheInitialSelection()
        {
            var behavior = new SynchronizeSelectedItems();
            BindingOperations.SetBinding(
                behavior,
                SynchronizeSelectedItems.SelectionsProperty,
                new Binding("ObservableSelections"));

            var listBox = new ListBox() { SelectionMode = SelectionMode.Multiple };
            BindingOperations.SetBinding(
                listBox, ListBox.ItemsSourceProperty,
                new Binding("Items"));

            var itemsHolder =
                new ItemsHolder
                {
                    Items = { "a", "b", "c", "d" },
                    ObservableSelections = { "a", "c" }
                };

            listBox.DataContext = itemsHolder;

            var behaviors = Interaction.GetBehaviors(listBox);
            behaviors.Add(behavior);

            CollectionAssert.AreEquivalent(new[] { "a", "c" }, listBox.SelectedItems);
        }

        [TestMethod]
        public void WhenDataContextChanges_ThenItupdatesSelectedItems()
        {
            var behavior = new SynchronizeSelectedItems();
            BindingOperations.SetBinding(
                behavior,
                SynchronizeSelectedItems.SelectionsProperty,
                new Binding("ObservableSelections"));

            var listBox = new ListBox() { SelectionMode = SelectionMode.Multiple };
            BindingOperations.SetBinding(
                listBox, ListBox.ItemsSourceProperty,
                new Binding("Items"));

            listBox.DataContext =
                new ItemsHolder
                {
                    Items = { "a", "b", "c", "d" },
                    ObservableSelections = { "a", "c" }
                };

            var behaviors = Interaction.GetBehaviors(listBox);
            behaviors.Add(behavior);

            listBox.DataContext =
                new ItemsHolder
                {
                    Items = { "a", "b", "c", "d" },
                    ObservableSelections = { "b", "d" }
                };

            CollectionAssert.AreEquivalent(new[] { "b", "d" }, listBox.SelectedItems);
        }

        [TestMethod]
        public void WhenSelectedItemsAreUpdatedOnTheListBox_ThenSelectionsAreUpdatedOnTheModel()
        {
            var behavior = new SynchronizeSelectedItems();
            BindingOperations.SetBinding(
                behavior,
                SynchronizeSelectedItems.SelectionsProperty,
                new Binding("ObservableSelections"));

            var listBox = new ListBox() { SelectionMode = SelectionMode.Multiple };
            BindingOperations.SetBinding(
                listBox, ListBox.ItemsSourceProperty,
                new Binding("Items"));

            var itemsHolder =
                new ItemsHolder
                {
                    Items = { "a", "b", "c", "d" },
                    ObservableSelections = { "a", "c" }
                };

            listBox.DataContext = itemsHolder;

            var behaviors = Interaction.GetBehaviors(listBox);
            behaviors.Add(behavior);

            listBox.SelectedItems.Add("d");
            listBox.SelectedItems.Remove("c");

            CollectionAssert.AreEquivalent(new[] { "a", "d" }, itemsHolder.ObservableSelections);
        }

        [TestMethod]
        public void WhenSelectionsAreUpdatedOnTheObservableModel_ThenSelectedItemsAreUpdatedOnTheListBox()
        {
            var behavior = new SynchronizeSelectedItems();
            BindingOperations.SetBinding(
                behavior,
                SynchronizeSelectedItems.SelectionsProperty,
                new Binding("ObservableSelections"));

            var listBox = new ListBox() { SelectionMode = SelectionMode.Multiple };
            BindingOperations.SetBinding(
                listBox, ListBox.ItemsSourceProperty,
                new Binding("Items"));

            var itemsHolder =
                new ItemsHolder
                {
                    Items = { "a", "b", "c", "d" },
                    ObservableSelections = { "a", "c" }
                };

            listBox.DataContext = itemsHolder;

            var behaviors = Interaction.GetBehaviors(listBox);
            behaviors.Add(behavior);

            itemsHolder.ObservableSelections.Add("d");
            itemsHolder.ObservableSelections.Remove("c");

            CollectionAssert.AreEquivalent(new[] { "a", "d" }, listBox.SelectedItems);
        }

        [TestMethod]
        public void WhenSelectionsAreUpdatedOnTheNonObservableModel_ThenSelectedItemsAreNotUpdatedOnTheListBox()
        {
            var behavior = new SynchronizeSelectedItems();
            BindingOperations.SetBinding(
                behavior,
                SynchronizeSelectedItems.SelectionsProperty,
                new Binding("Selections"));

            var listBox = new ListBox() { SelectionMode = SelectionMode.Multiple };
            BindingOperations.SetBinding(
                listBox, ListBox.ItemsSourceProperty,
                new Binding("Items"));

            var itemsHolder =
                new ItemsHolder
                {
                    Items = { "a", "b", "c", "d" },
                    Selections = { "a", "c" }
                };

            listBox.DataContext = itemsHolder;

            var behaviors = Interaction.GetBehaviors(listBox);
            behaviors.Add(behavior);

            itemsHolder.Selections.Add("d");
            itemsHolder.Selections.Remove("c");

            CollectionAssert.AreEquivalent(new[] { "a", "c" }, listBox.SelectedItems);
        }

        [TestMethod]
        public void WhenSelectedItemsAreUpdatedOnTheListBoxAfterDetachingTheBehavior_ThenSelectionsAreNotUpdatedOnTheModel()
        {
            var behavior = new SynchronizeSelectedItems();
            BindingOperations.SetBinding(
                behavior,
                SynchronizeSelectedItems.SelectionsProperty,
                new Binding("ObservableSelections"));

            var listBox = new ListBox() { SelectionMode = SelectionMode.Multiple };
            BindingOperations.SetBinding(
                listBox, ListBox.ItemsSourceProperty,
                new Binding("Items"));

            var itemsHolder =
                new ItemsHolder
                {
                    Items = { "a", "b", "c", "d" },
                    ObservableSelections = { "a", "c" }
                };

            listBox.DataContext = itemsHolder;

            var behaviors = Interaction.GetBehaviors(listBox);
            behaviors.Add(behavior);

            listBox.SelectedItems.Add("d");

            behaviors.Remove(behavior);

            listBox.SelectedItems.Remove("c");

            CollectionAssert.AreEquivalent(new[] { "a", "c", "d" }, itemsHolder.ObservableSelections);
        }

        [TestMethod]
        public void WhenSelectionsAreUpdatedOnTheObservableModelAfterDetachingTheBehavior_ThenSelectedItemsAreNotUpdatedOnTheListBox()
        {
            var behavior = new SynchronizeSelectedItems();
            BindingOperations.SetBinding(
                behavior,
                SynchronizeSelectedItems.SelectionsProperty,
                new Binding("ObservableSelections"));

            var listBox = new ListBox() { SelectionMode = SelectionMode.Multiple };
            BindingOperations.SetBinding(
                listBox, ListBox.ItemsSourceProperty,
                new Binding("Items"));

            var itemsHolder =
                new ItemsHolder
                {
                    Items = { "a", "b", "c", "d" },
                    ObservableSelections = { "a", "c" }
                };

            listBox.DataContext = itemsHolder;

            var behaviors = Interaction.GetBehaviors(listBox);
            behaviors.Add(behavior);

            itemsHolder.ObservableSelections.Add("d");

            behaviors.Remove(behavior);
            
            itemsHolder.ObservableSelections.Remove("c");

            CollectionAssert.AreEquivalent(new[] { "a", "c", "d" }, listBox.SelectedItems);
        }

        public class ItemsHolder
        {
            public ItemsHolder()
            {
                this.Items = new ObservableCollection<string>();
                this.ObservableSelections = new ObservableCollection<string>();
                this.Selections = new List<string>();
            }

            public ObservableCollection<string> Items { get; private set; }
            public ObservableCollection<string> ObservableSelections { get; private set; }
            public List<string> Selections { get; private set; }
        }
    }
}
