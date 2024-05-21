using Syncfusion.Maui.Data;
using Syncfusion.Maui.DataGrid;
using System.Collections;

namespace DataGridMAUI
{
    public class DataGridPickerRenderer : DataGridCellRenderer<SfDataGridLabel, GridPicker>
    {
        public DataGridPickerRenderer()
        {
            this.IsEditable = true;
            this.IsFocusable = true;
        }
        protected override SfDataGridLabel? OnCreateDisplayUIView()
        {
            return new SfDataGridLabel();            
        }

        protected override GridPicker? OnCreateEditUIView()
        {
            return new GridPicker();
        }

        public override void OnInitializeEditView(DataColumnBase dataColumn, GridPicker view)
        {
            base.OnInitializeEditView(dataColumn, view);
            this.InitializeEditBinding(dataColumn, view);
        }

        private void InitializeEditBinding(DataColumnBase dataColumn, GridPicker view)
        {
            var pickerColumn = (DataGridPickerColumn)dataColumn.DataGridColumn;
            if (pickerColumn != null && pickerColumn.ItemsSourceSelector != null)
            {
                // Support for ItemsSourceSelector for GridPickerColumn
                var dataGridDataContext = DataGrid.BindingContext;
                object value = pickerColumn.ItemsSourceSelector.GetItemsSource(dataColumn.RowData, dataGridDataContext);
                pickerColumn.ItemsSource = value as IList;
            }
            view.TextAlignment = pickerColumn.CellTextAlignment;            
            view.Title = pickerColumn.Title;
            view.DisplayMemberPath = pickerColumn.DisplayMemberPath;
            view.ValueMemberPath = pickerColumn.ValueMemberPath;
            view.ItemsSource = pickerColumn.ItemsSource;
            var valueBinding = pickerColumn.ValueBinding as Binding;
            var bind = new Binding()
            {
                Converter = valueBinding.Converter,
                ConverterParameter = valueBinding.ConverterParameter,
                Mode = BindingMode.TwoWay,
                Path = valueBinding.Path,
                Source = valueBinding.Source,
            };
            view.SetBinding(GridPicker.SelectedItemProperty, bind);
        }

        /// <summary>
        /// Returns the current cell value, only if the renderer's <see cref="DataGridCellRendererBase.HasCurrentCellState"/> is <b>true</b>.
        /// </summary>
        /// <returns>The current cell value.</returns>
        public override object? GetControlValue()
        {
            if (!this.HasCurrentCellState)
            {
                return base.GetControlValue();
            }

            if (CurrentCellRendererElement!.GetValue(GridPicker.SelectedItemProperty) is string)
            {
                return CurrentCellRendererElement.GetValue(IsInEditing ? GridPicker.SelectedItemProperty : Label.TextProperty);
            }

            return base.GetControlValue();
            
        }

        public override void SetControlValue(object value)
        {
            if (!this.HasCurrentCellState)
            {
                return;
            }

            if (this.IsInEditing)
            {
                ((GridPicker)CurrentCellRendererElement!).SelectedItem = value;
            }
        }

        protected override void WireEditUIElement(GridPicker editElement)
        {            
            if (editElement != null)
            {
                base.WireEditUIElement(editElement);
                editElement.SelectedIndexChanged += OnSelectedIndexChanged;
            }
        }

        private void OnSelectedIndexChanged(object sender, EventArgs eventArgs)
        {
            var picker = (GridPicker)sender;
            if (picker.SelectedIndex < 0 || picker.SelectedIndex > picker.Items.Count - 1)
            {
                picker.SelectedItem = null;
            }
            else
            {
                if (picker.IsComplexType)
                {
                    var pickedData = picker.ItemsSource[picker.SelectedIndex];
                    if (!string.IsNullOrEmpty(picker.ValueMemberPath) && !string.IsNullOrWhiteSpace(picker.ValueMemberPath))
                    {
                        picker.SelectedItem = PropertyDescriptorExtensions.GetValue(picker.PropertyInfoCollection, pickedData, picker.ValueMemberPath);
                    }
                    else
                    {
                        picker.SelectedItem = pickedData;
                    }
                }
                else
                {
                    picker.SelectedItem = picker.Items[picker.SelectedIndex];
                }
            }
            
            picker = null;
        }

        protected override void UnwireEditUIElement(GridPicker editElement)
        {
            if (editElement != null)
            {
                base.UnwireEditUIElement(editElement);
                editElement.SelectedIndexChanged -= OnSelectedIndexChanged;
            }
        }
    }
}
