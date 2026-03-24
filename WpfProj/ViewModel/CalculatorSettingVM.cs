using WpfProj.MVVM;
using WpfProj.Strategy;

namespace WpfProj.ViewModel
{
    public class CalculatorSettingVM : ViewModelBase
    {
        public void GetCalculationResult(double num1, double num2)
        {
            var calculator = new Calculator();

            calculator.SetStrategy(new AdditionStrategy());
            var addResult = calculator.Calculate(num1, num2);

            calculator.SetStrategy(new SubtractionStrategy());
            var substractResult = calculator.Calculate(num1, num2);
        }
    }
}
