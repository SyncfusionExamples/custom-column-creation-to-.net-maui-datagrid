# custom-column-creation-to-.net-maui-datagrid

This article shows how to create and integrate a custom column in the .NET MAUI DataGrid (SfDataGrid). Using the included sample files, the grid displays a mix of built-in columns (numeric and text) and a custom Picker-based column for selecting the Ship Country. The approach demonstrates how to declare columns explicitly (AutoGenerateColumnsMode="None"), bind to a view model, and register a custom cell renderer so the grid can render and edit cells with a native MAUI Picker.

For official guidance on built-in column types and customization, please refer: [Column Types in .NET MAUI DataGrid ]( https://help.syncfusion.com/maui/datagrid/column-types). You can also explore the control overview and features here: [.NET MAUI DataGrid ](https://www.syncfusion.com/maui-controls/maui-datagrid).

## xaml
It sets the page BindingContext, configures the SfDataGrid, and defines both built-in and custom columns.

```
<ContentPage.BindingContext>
    <local:OrderInfoRepository  x:Name="viewModel"/>
</ContentPage.BindingContext>

<syncfusion:SfDataGrid x:Name="dataGrid"
                            Margin="20"
                            VerticalOptions="FillAndExpand"
                            ItemsSource="{Binding OrderInfoCollection}"
                            GridLinesVisibility="Both"
                            HeaderGridLinesVisibility="Both"
                            AutoGenerateColumnsMode="None"
                            AllowEditing="True"
                            SelectionMode="Single"
                            ColumnWidthMode="Auto">
    <syncfusion:SfDataGrid.Columns>
        <syncfusion:DataGridNumericColumn Format="D"
                                                HeaderText="Order ID"
                                                MappingName="OrderID">
        </syncfusion:DataGridNumericColumn>
        <syncfusion:DataGridTextColumn HeaderText="Customer ID"
                                            MappingName="CustomerID">
        </syncfusion:DataGridTextColumn>
        <syncfusion:DataGridTextColumn MappingName="Customer"
                                            HeaderText="Customer">
        </syncfusion:DataGridTextColumn>
        <syncfusion:DataGridTextColumn HeaderText="Ship City"
                                            MappingName="ShipCity">
        </syncfusion:DataGridTextColumn>
        <local:DataGridPickerColumn HeaderText="Ship Country"
                                    MappingName="ShipCountry"                                                                                
                                    DataGrid="{x:Reference dataGrid}"
                                    BindingContext="{x:Reference viewModel}"
                                    ItemsSource="{Binding CountryList}"
                                    />          
    </syncfusion:SfDataGrid.Columns>
</syncfusion:SfDataGrid>
```

Highlights
- AutoGenerateColumnsMode="None" gives full control over which columns are shown and in what order.
- MappingName must exactly match the property names in the bound items (OrderID, CustomerID, Customer, ShipCity, ShipCountry).
- The custom column (local:DataGridPickerColumn) binds to CountryList from the view model and references the DataGrid, enabling the renderer to coordinate editing behavior.

## C#
The code-behind registers a custom cell renderer so the grid can display and edit the custom Picker column. This occurs after InitializeComponent in MainPage.xaml.cs.

```
public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        this.dataGrid.CellRenderers.Add("Picker", new DataGridPickerRenderer());
    }
}
```

Notes
- The string key ("Picker") associates the custom column type with its renderer. Ensure your DataGridPickerColumn internally uses that key.
- Your renderer is responsible for creating the display and editor views, binding to the row item’s ShipCountry property, and committing value changes.
- Keep the BindingContext consistent so CountryList and OrderInfoCollection are available when the page loads.

## Why a custom column?
Built-in columns like DataGridTextColumn, DataGridNumericColumn, and DataGridDateColumn cover common scenarios. However, custom columns become valuable when you need:
- A specialized editor such as a Picker, Slider, or composite control.
- Custom binding/commit logic beyond what templates provide out-of-the-box.
- Reusable behavior encapsulated in a column/renderer pair rather than repeated DataTemplates.

## Tips and best practices
- Validate MappingName for every column to avoid silent binding failures.
- Choose an appropriate ColumnWidthMode (Auto, FitByCell, FitByHeader) for optimal layout.
- Keep editor creation lightweight; reuse visual elements when possible for performance.
- Provide sensible defaults in your renderer (e.g., placeholder text) and handle null values gracefully.
- If you switch data sources at runtime, clear and rebuild Columns to prevent duplicates.
- Consider DataGridTemplateColumn when you only need ad-hoc UI without a full custom renderer.

## Extending the sample
You can complement the custom Picker column with built-ins:
- Numeric column with formatting
```
<syncfusion:DataGridNumericColumn MappingName="Quantity"
                                  HeaderText="Qty"
                                  Format="N0"/>
```
- Date column with formatting
```
<syncfusion:DataGridDateColumn MappingName="OrderDate"
                               HeaderText="Ordered"
                               Format="MM/dd/yyyy"/>
```
- Template column for actions
```
<syncfusion:DataGridTemplateColumn HeaderText="Actions">
    <syncfusion:DataGridTemplateColumn.CellTemplate>
        <DataTemplate>
            <HorizontalStackLayout Spacing="8">
                <Button Text="Edit"/>
                <Button Text="Delete"/>
            </HorizontalStackLayout>
        </DataTemplate>
    </syncfusion:DataGridTemplateColumn.CellTemplate>
</syncfusion:DataGridTemplateColumn>
```

##### Conclusion
 
I hope you enjoyed learning about how to create custom columns in .NET MAUI DataGrid (SfDataGrid).
 
You can refer to our [.NET MAUI DataGrid’s feature tour](https://www.syncfusion.com/maui-controls/maui-datagrid) page to learn about its other groundbreaking feature representations. You can also explore our [.NET MAUI DataGrid Documentation](https://help.syncfusion.com/maui/datagrid/getting-started) to understand how to present and manipulate data. 
For current customers, you can check out our .NET MAUI components on the [License and Downloads](https://www.syncfusion.com/sales/teamlicense) page. If you are new to Syncfusion, you can try our 30-day [free trial](https://www.syncfusion.com/downloads/maui) to explore our .NET MAUI DataGrid and other .NET MAUI components.
 
If you have any queries or require clarifications, please let us know in the comments below. You can also contact us through our [support forums](https://www.syncfusion.com/forums), [Direct-Trac](https://support.syncfusion.com/create) or [feedback portal](https://www.syncfusion.com/feedback/maui?control=sfdatagrid), or the feedback portal. We are always happy to assist you!