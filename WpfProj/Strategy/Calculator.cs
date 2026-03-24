namespace WpfProj.Strategy
{
    //Context
    public class Calculator
    {
        private ICalculationStrategy _strategy;

        public void SetStrategy(ICalculationStrategy strategy)
        {
            _strategy = strategy;
        }

        public double Calculate(double a, double b)
        {
            if (_strategy == null)
                throw new InvalidOperationException("Strategy not set.");
            return _strategy.Calculate(a, b);
        }
    }
}
