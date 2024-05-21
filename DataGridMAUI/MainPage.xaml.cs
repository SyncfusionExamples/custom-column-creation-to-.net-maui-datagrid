namespace DataGridMAUI
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            this.dataGrid.CellRenderers.Add("Picker", new DataGridPickerRenderer());
        }       
    }
}
