namespace MTSahilKumar
{
    public class SalariedEmployee : Employee
    {
        public double FixedWeeklySalary { get; set; }
        public override double GrossEarnings => FixedWeeklySalary;
    }
}
