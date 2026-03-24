namespace WpfProj.Strategy
{
    public class SubtractionStrategy : ICalculationStrategy
    {
        public double Calculate(double a, double b)
        {
            return a - b;
        }    
    }
}
