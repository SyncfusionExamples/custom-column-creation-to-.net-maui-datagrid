using Syncfusion.Maui.Data;
using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DataGridMAUI
{
    public class GridPicker : Picker
    {
        #region Bindable Property

        /// <summary>
        /// Identifies the text size <see cref="BindableProperty"/>.
        /// </summary>
        /// <remarks>This <see cref="BindableProperty"/> is read-only.</remarks>        
        public static readonly BindableProperty TextSizeProperty =
            BindableProperty.Create("TextSize", typeof(double), typeof(GridPicker), 14d, BindingMode.TwoWay, null);

        /// <summary>
        /// Identifies the display member path <see cref="BindableProperty"/>.
        /// </summary>
        /// <remarks>This <see cref="BindableProperty"/> is read-only.</remarks>        
        public static readonly BindableProperty DisplayMemberPathProperty =
            BindableProperty.Create("DisplayMemberPath", typeof(string), typeof(GridPicker), string.Empty, BindingMode.TwoWay, null);

        /// <summary>
        /// Identifies the value member path <see cref="BindableProperty"/>.
        /// </summary>
        /// <remarks>This <see cref="BindableProperty"/> is read-only.</remarks>        
        public static readonly BindableProperty ValueMemberPathProperty =
            BindableProperty.Create("ValueMemberPath", typeof(string), typeof(GridPicker), string.Empty, BindingMode.TwoWay, null);

        /// <summary>
        /// Identifies the text alignment <see cref="BindableProperty"/>.
        /// </summary>
        /// <remarks>This <see cref="BindableProperty"/> is read-only.</remarks>        
        public static readonly BindableProperty TextAlignmentProperty =
            BindableProperty.Create("TextAlignment", typeof(TextAlignment), typeof(GridPicker), TextAlignment.Center, BindingMode.TwoWay, null);

        /// <summary>
        /// Identifies the TextAlignment <see cref="BindableProperty"/>.
        /// </summary>
        /// <remarks>This <see cref="BindableProperty"/> is read-only.</remarks>        
        public static new readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create("ItemsSource", typeof(IList), typeof(GridPicker), default(IList), BindingMode.TwoWay, null, propertyChanged: OnItemsSourceChanged);

        /// <summary>
        /// Identifies the TextAlignment <see cref="BindableProperty"/>.
        /// </summary>
        /// <remarks>This <see cref="BindableProperty"/> is read-only.</remarks>        
        public static new BindableProperty SelectedItemProperty =
          BindableProperty.Create("SelectedItem", typeof(object), typeof(GridPicker), default(object), BindingMode.TwoWay, null, propertyChanged: OnSelectedItemChanged);

        #endregion

        #region Bindable Property

        /// <summary>
        /// Gets or sets the items source of the picker.
        /// </summary>
        /// <value>The item source of the picker.</value>        
        public new IList ItemsSource
        {
            get { return (IList)GetValue(ItemsSourceProperty); }
            set { this.SetValue(ItemsSourceProperty, value); }
        }

        /// <summary>
        /// Gets or sets the selected item of the picker.
        /// </summary>
        /// <value>The selected item of the picker.</value>
        public new object SelectedItem
        {
            get { return (object)GetValue(SelectedItemProperty); }
            set { this.SetValue(SelectedItemProperty, value); }
        }

        /// <summary>
        /// Gets or sets the text alignment of the picker.
        /// </summary>
        /// <value>The text alignment of the picker. 
        /// The default value is <see cref="Xamarin.Forms.TextAlignment.Center"/>.</value>
        public TextAlignment TextAlignment
        {
            get { return (TextAlignment)GetValue(TextAlignmentProperty); }
            set { this.SetValue(TextAlignmentProperty, value); }
        }

        /// <summary>
        /// Gets or sets the text size of the picker.
        /// </summary>
        /// <value>The text size of the picker. The default value is 14.</value>
        public double TextSize
        {
            get { return (double)GetValue(TextSizeProperty); }
            set { this.SetValue(TextSizeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the display member path of the picker.
        /// </summary>
        /// <value>The display member path of the picker.</value>
        public string DisplayMemberPath
        {
            get { return (string)GetValue(DisplayMemberPathProperty); }
            set { this.SetValue(DisplayMemberPathProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value member path of the picker.
        /// </summary>
        /// <value>The value member path of the picker.</value>
        public string ValueMemberPath
        {
            get { return (string)GetValue(ValueMemberPathProperty); }
            set { this.SetValue(ValueMemberPathProperty, value); }
        }

        #endregion

        /// <summary>
        /// Gets or sets the individual properties in the collection.
        /// </summary>
        internal PropertyDescriptorCollection PropertyInfoCollection { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the collection is a simple or complex collection.
        /// </summary>
        internal bool IsComplexType { get; set; }

        private static void OnItemsSourceChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            var picker = bindable as GridPicker;

            // Changes were moved to seperate method, since process needed when refreshing item picker comes to view.
            picker.RefreshItems();
        }

        private static void OnSelectedItemChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            var picker = bindable as GridPicker;
            if (newvalue != null)
            {
                if (!picker.IsComplexType)
                {
                    var index = picker.Items.IndexOf(newvalue.ToString());
                    picker.SelectedIndex = Math.Max(index, -1);
                }
                else if (oldvalue == null && newvalue != null)
                {
                    int startIndex = 0;
                    foreach (var item in picker.ItemsSource)
                    {
                        string path = (string.IsNullOrEmpty(picker.ValueMemberPath) || string.IsNullOrWhiteSpace(picker.ValueMemberPath)) ? picker.DisplayMemberPath : picker.ValueMemberPath;
                        if (!string.IsNullOrWhiteSpace(path) && !string.IsNullOrEmpty(path))
                        {
                            var val = PropertyDescriptorExtensions.GetValue(picker.PropertyInfoCollection, item, path);
                            if (!NullableHelperInternal.IsComplexType(newvalue.GetType()))
                            {
                                if (val.ToString() == newvalue.ToString())
                                {
                                    break;
                                }
                            }
                            else
                            {
                                if (val.ToString() == PropertyDescriptorExtensions.GetValue(picker.PropertyInfoCollection, newvalue, path).ToString())
                                {
                                    break;
                                }
                            }
                        }
                        else
                        {
                            break;
                        }

                        startIndex++;
                    }

                    picker.SelectedIndex = Math.Max(startIndex > picker.Items.Count ? 0 : startIndex, -1);
                }
            }
        }

        private void RefreshItems()
        {
            Items.Clear();
            if (this.ItemsSource == null)
            {
                return;
            }

            var itemType = this.ItemsSource.GetItemType(true);
            if (NullableHelperInternal.IsComplexType(itemType))
            {
                this.IsComplexType = true;

                //XAMARIN-24015 - Update the source project of SfDataGrid to .NET standard project
                var queryedList = this.ItemsSource.AsQueryable();
                this.PropertyInfoCollection = TypeDescriptor.GetProperties(itemType);
                var propertyInfo = PropertyDescriptorExtensions.GetPropertyDescriptor(this.PropertyInfoCollection, this.DisplayMemberPath);
                if (propertyInfo != null)
                {
                    var displayList = queryedList.Select(this.DisplayMemberPath).ToList<object>();
                    displayList.ForEach((item) =>
                    {
                        this.Items.Add(item.ToString());
                    });
                }
                else
                {
                    var displayList = this.ItemsSource.ToList<object>();
                    displayList.ForEach((item) =>
                    {
                        this.Items.Add(item.ToString());
                    });
                }
            }
            else
            {
                this.IsComplexType = false;
                foreach (var item in this.ItemsSource)
                {
                    this.Items.Add(item.ToString());
                }
            }
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if (propertyName == "IsVisible" && this.IsVisible)
            {
                this.RefreshItems();
            }
        }
    }
}
