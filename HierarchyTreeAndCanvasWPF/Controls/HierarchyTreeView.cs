﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Specialized;
using HierarchyTreeAndCanvasWPF.ViewModels;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using HierarchyTreeAndCanvasWPF.Controls.CustomEventArgs;

namespace HierarchyTreeAndCanvasWPF.Controls
{
    public class HierarchyTreeView : TreeView
    {
        private IShapeCanvasViewModel _vm;
        private List<TreeItem> _selectedItems = new();

        public event EventHandler<ShapeStateChangedEventArgs> ShapeStateChanged;

        public void HandleShapeStateChanged(object sender, ShapeStateChangedEventArgs e)
        {
            foreach (TreeItem item in _vm.TreeItems)
            {
                if (item.ShapeRef == e.Shape)
                {
                    if (e.Selected)
                    {
                        if (e.SelectionType == SelectionType.Additional)
                        {
                            SelectAdditional(item);
                        }
                        else if (e.SelectionType == SelectionType.Only)
                        {
                            SelectOnly(item);
                        }
                    }
                    else
                    {
                        DeselectItem(item);
                    }

                    break;
                }
            }
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            _vm = DataContext as IShapeCanvasViewModel;
        }

        protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            base.OnItemsSourceChanged(oldValue, newValue);

            (ItemsSource as ObservableCollection<TreeItem>).CollectionChanged += HierarchyTreeView_CollectionChanged;
        }

        private void HierarchyTreeView_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Debug.WriteLine($"TREE: New items {e.NewItems}");
        }

        protected override void OnSelectedItemChanged(RoutedPropertyChangedEventArgs<object> e)
        {
            base.OnSelectedItemChanged(e);

            Debug.WriteLine($"TREE: OnSelectedItemChanged Old> {e.OldValue} New> {e.NewValue}");

            if (e.NewValue is TreeItem newItem)
            {
                // prevent default behavior
                newItem.IsSelected = false;

                if (Keyboard.Modifiers == ModifierKeys.Control
                    || Keyboard.Modifiers == ModifierKeys.Shift)
                {
                    SelectAdditionalAndRaiseEvent(newItem);
                }
                else
                {
                    SelectOnlyAndRaiseEvent(newItem);
                }

                Debug.WriteLine($"TREE: Selected items count {_selectedItems.Count}");
            }
        }

        private void SelectOnly(TreeItem item)
        {
            DeselectAllItems();

            if (!_selectedItems.Contains(item))
            {
                _selectedItems.Add(item);
                item.MSelected = true;
            }
        }

        private void SelectAdditional(TreeItem item)
        {
            if (!_selectedItems.Contains(item))
            {
                _selectedItems.Add(item);
                item.MSelected = true;
            }
        }

        private void SelectOnlyAndRaiseEvent(TreeItem item)
        {
            SelectOnly(item);
            ShapeStateChanged(this, new ShapeStateChangedEventArgs(item.ShapeRef, true, SelectionType.Only));
        }

        private void SelectAdditionalAndRaiseEvent(TreeItem item)
        {
            SelectAdditional(item);
            ShapeStateChanged(this, new ShapeStateChangedEventArgs(item.ShapeRef, true, SelectionType.Additional));
        }

        private void DeselectAllItems()
        {
            Debug.WriteLine($"TREE: Deselecting all items");

            for (int i = _selectedItems.Count - 1; i >= 0; i--)
            {
                DeselectItemAndRaiseEvent(_selectedItems[i]);
            }
        }

        private void DeselectItem(TreeItem item)
        {
            _selectedItems.Remove(item);
            item.MSelected = false;

            Debug.WriteLine($"TREE: Deselected {item.Header}");
        }

        private void DeselectItemAndRaiseEvent(TreeItem item)
        {
            DeselectItem(item);
            ShapeStateChanged(this, new ShapeStateChangedEventArgs(item.ShapeRef, false));
        }

    }
}
