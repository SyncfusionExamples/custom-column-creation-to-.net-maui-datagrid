using Syncfusion.Maui.Data;
using Syncfusion.Maui.DataGrid;
using System.Collections;
using System.ComponentModel;
using System.Globalization;

namespace DataGridMAUI
{
    internal class DisplayMemberConverter : IValueConverter
    {
        private DataGridColumn cachedColumn;
        public DisplayMemberConverter(DataGridColumn column)
        {
            this.cachedColumn = column;
        }

        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return Convert(value);
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value;
        }

        private object Convert(object selectedValue)
        {
            IEnumerable list = null;
            var displayMemberPath = string.Empty;
            var valueMemberPath = string.Empty;
            if (this.cachedColumn is DataGridPickerColumn)
            {
                var column = this.cachedColumn as DataGridPickerColumn;
                this.cachedColumn = column;
                displayMemberPath = column.DisplayMemberPath;
                valueMemberPath = column.ValueMemberPath;
                if (column.ItemsSourceSelector != null && selectedValue != null)
                {
                    var dataContext = column.BindingContext;
                    list = column.ItemsSourceSelector.GetItemsSource(selectedValue, dataContext);
                    var view = column.DataGrid.View;
                    selectedValue = view == null ? null : view.GetPropertyAccessProvider().GetValue(selectedValue, column.MappingName);
                }
                else
                {
                    list = column.ItemsSource as IEnumerable;
                }
            }
            if (selectedValue == null)
            {
                return null;
            }

            PropertyDescriptorCollection pdc = null;

            if (string.IsNullOrEmpty(valueMemberPath))
            {
                if (!string.IsNullOrEmpty(displayMemberPath))
                {
                    var type = selectedValue.GetType();
                    pdc = TypeDescriptor.GetProperties(type);                    
                    return pdc.GetValue(selectedValue, displayMemberPath);
                }

                return selectedValue;
            }
            else
            {
                if (list == null)
                {
                    return null;
                }

                var enumerator = list.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    var type = enumerator.Current.GetType();

                    pdc = pdc ?? TypeDescriptor.GetProperties(type);

                    if (selectedValue.Equals(pdc.GetValue(enumerator.Current, valueMemberPath)))
                    {
                        if (!string.IsNullOrEmpty(displayMemberPath))
                        {
                            return pdc.GetValue(enumerator.Current, displayMemberPath);
                        }

                        return enumerator.Current;
                    }
                }
            }
            return null;
        }
    }
}
