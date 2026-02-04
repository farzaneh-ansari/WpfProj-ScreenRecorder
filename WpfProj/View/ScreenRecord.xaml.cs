using System.Windows;
using System.Windows.Controls;
using WpfProj.ViewModel;


namespace WpfProj.View
{
    /// <summary>
    /// Interaction logic for ScreenRecord.xaml
    /// </summary>
    public partial class ScreenRecord : UserControl
    {
        ScreenRecordVM screenRecordVM = new ScreenRecordVM();
        public ScreenRecord()
        {
            InitializeComponent();            
            this.DataContext = screenRecordVM;
            screenRecordVM.InitializeRecording();
        }

        
        private void Start_Click(object sender, RoutedEventArgs e)
        {

            Window mainWindow = Application.Current.MainWindow;           

            screenRecordVM.StartRecording(mainWindow);


        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
           
            screenRecordVM.StopRecording();

        }

       

        

    }
}
