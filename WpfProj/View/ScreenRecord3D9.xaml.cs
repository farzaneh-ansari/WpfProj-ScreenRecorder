using System.Windows;
using System.Windows.Controls;
using WpfProj.ViewModel;


namespace WpfProj.View
{
    /// <summary>
    /// Interaction logic for ScreenRecord3D9.xaml
    /// </summary>
    public partial class ScreenRecord3D9 : UserControl
    {
        ScreenRecord3D9VM screenRecord3D9 = new ScreenRecord3D9VM();
        public ScreenRecord3D9()
        {
            InitializeComponent();            
            this.DataContext = screenRecord3D9;
            
        }

        
        private void Start_Click(object sender, RoutedEventArgs e)
        {

            Window mainWindow = Application.Current.MainWindow;

            screenRecord3D9.StartRecording(mainWindow);


        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {

            screenRecord3D9.StopRecording();
            


        }
        private void VideoRender_Click(object sender, RoutedEventArgs e)
        {            

            screenRecord3D9.VideoRender();
            


        }





    }
}
