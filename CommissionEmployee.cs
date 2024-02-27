namespace MTSahilKumar
{
    public class CommissionEmployee : Employee
    {
        public double GrossSales { get; set; }
        public double CommissionRate { get; set; }
        public override double GrossEarnings => GrossSales * CommissionRate;
    }
}
