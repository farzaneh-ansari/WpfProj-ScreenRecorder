using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfProj.Model;
using WpfProj.ViewModel;
using static System.Net.Mime.MediaTypeNames;

namespace WpfProj.View
{
    /// <summary>
    /// Interaction logic for WidowSetting.xaml
    /// </summary>
    public partial class WidowSetting : UserControl
    {
        public WidowSetting()
        {
            InitializeComponent();
            WindowSettingVM windowSettingVM = new WindowSettingVM();
            this.DataContext = windowSettingVM;
            
            var widthExprs = ExpressionHelper.CreateMemberInitExpression<WindowSettingVM, int>(x => x.Width, 200);

            ExpressionHelper.CreateMemberInitExpression<WindowSettingVM, int>(x => x.Height, 400);

            var windowSetting = ExpressionHelper.CreateObjectFromExpression<WindowSettingVM>(ExpressionHelper.CreateMemberInitExpression<WindowSettingVM, int>(x => x.Width, 200));

            Console.WriteLine(windowSetting.Width);
            Console.WriteLine(widthExprs.ToString());
        }
    }
}
