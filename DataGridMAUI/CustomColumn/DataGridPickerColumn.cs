using Syncfusion.Maui.DataGrid;
using System.Collections;

namespace DataGridMAUI
{
    public class DataGridPickerColumn : DataGridColumn
    {
        public SfDataGrid DataGrid { get; set; }
        public DataGridPickerColumn()
        {
            this.CellType = "Picker";
        }
        #region BindableProperty

        /// <summary>
        /// Identifies the ItemsSource <see cref="BindableProperty"/>.
        /// </summary>
        /// <remarks>This <see cref="BindableProperty"/> is read-only.</remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "Reviewed. Suppression is OK here. This field is immutable.")]
        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create("ItemsSource", typeof(IList), typeof(DataGridPickerColumn), null, BindingMode.TwoWay);

        /// <summary>
        /// Identifies the Title <see cref="BindableProperty"/>.
        /// </summary>
        /// <remarks>This <see cref="BindableProperty"/> is read-only.</remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "Reviewed. Suppression is OK here. This field is immutable.")]
        public static readonly BindableProperty TitleProperty =
            BindableProperty.Create("Title", typeof(string), typeof(DataGridPickerColumn), null, BindingMode.TwoWay);

        /// <summary>
        /// Identifies the DisplayMemberPath <see cref="BindableProperty"/>.
        /// </summary>
        /// <remarks>This <see cref="BindableProperty"/> is read-only.</remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "Reviewed. Suppression is OK here. This field is immutable.")]
        public static readonly BindableProperty DisplayMemberPathProperty =
            BindableProperty.Create("DisplayMemberPath", typeof(string), typeof(DataGridPickerColumn), string.Empty, BindingMode.TwoWay);

        /// <summary>
        /// Identifies the ValueMemberPath <see cref="BindableProperty"/>.
        /// </summary>
        /// <remarks>This <see cref="BindableProperty"/> is read-only.</remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "Reviewed. Suppression is OK here. This field is immutable.")]
        public static readonly BindableProperty ValueMemberPathProperty =
            BindableProperty.Create("ValueMemberPath", typeof(string), typeof(DataGridPickerColumn), string.Empty, BindingMode.TwoWay);

        /// <summary>
        /// Identifies the  <see cref="Syncfusion.SfDataGrid.XForms.GridPickerColumn.ItemsSourceSelector"/> dependency property.
        /// </summary>
        /// <remarks>
        /// The identifier for the <see cref="Syncfusion.SfDataGrid.XForms.GridPickerColumn.ItemsSourceSelector"/> dependency property.
        /// </remarks>
        public static readonly BindableProperty ItemsSourceSelectorProperty =
           BindableProperty.Create("ItemsSourceSelector", typeof(IItemsSourceSelector), typeof(DataGridPickerColumn));
        #endregion

        public IList ItemsSource
        {
            get { return (IList)GetValue(ItemsSourceProperty); }
            set { this.SetValue(ItemsSourceProperty, value); }
        }

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { this.SetValue(TitleProperty, value); }
        }

        public string DisplayMemberPath
        {
            get { return (string)GetValue(DisplayMemberPathProperty); }
            set { this.SetValue(DisplayMemberPathProperty, value); }
        }
        public string ValueMemberPath
        {
            get { return (string)GetValue(ValueMemberPathProperty); }
            set { this.SetValue(ValueMemberPathProperty, value); }
        }

        public IItemsSourceSelector ItemsSourceSelector
        {
            get { return (IItemsSourceSelector)GetValue(ItemsSourceSelectorProperty); }
            set { SetValue(ItemsSourceSelectorProperty, value); }
        }

        protected override void SetConverterForDisplayBinding()
        {            
            var cachedDisplayBinding = (Binding)(this.GetType().GetProperty("CacheDisplayBinding", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).GetValue(this));            
            cachedDisplayBinding.Converter = new DisplayMemberConverter(this);
        }
    }
}
