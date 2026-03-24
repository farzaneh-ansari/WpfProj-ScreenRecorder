namespace WpfProj.Strategy
{
    public class AdditionStrategy : ICalculationStrategy
    {
        public double Calculate(double a, double b)
        {
            return a + b;
        }
    }
}
