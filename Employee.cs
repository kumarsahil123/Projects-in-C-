namespace MTSahilKumar
{
    public enum EmployeeType
    {
        Hourly,
        Commission,
        Weekly
    }
    public class Employee
    {
        public int EmployeeID { get; set; }
        public string Name { get; set; }
        public virtual double GrossEarnings { get; set; } 
        public double Tax { get; set; }
        public double NetEarnings { get; set; }
        public EmployeeType Type { get; set; }
    }
}