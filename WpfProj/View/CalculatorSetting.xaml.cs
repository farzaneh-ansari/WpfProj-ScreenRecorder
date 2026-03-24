using System.Windows.Controls;
using WpfProj.ViewModel;


namespace WpfProj.View
{
    /// <summary>
    /// Interaction logic for CalculatorSetting.xaml
    /// </summary>
    public partial class CalculatorSetting : UserControl
    {
        CalculatorSettingVM calculatorSettingVM = new CalculatorSettingVM();
        public CalculatorSetting()
        {
            InitializeComponent();
            this.DataContext = calculatorSettingVM;

            calculatorSettingVM.GetCalculationResult(5, 3);
        }
    }
}
