using System;
using System.Collections.Generic;
using System.Globalization;
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
namespace MTSahilKumar
{
    public partial class MainWindow : Window
    {
        private int currentEmployeeID = 1;
        private List<Employee> employees_list = new List<Employee>();
        public MainWindow()
        {
            InitializeComponent();
            EmployeeListBox.ItemsSource = employees_list;
            HourlyRadioButton.IsChecked = true;
            ShowEmployeeTypeStackPanel("Hourly_Paid_Employee");
            HideEmployeeTypeStackPanels(Commission_Based_Employee, Weekly_Salary_Employee);
        }
        private void ShowEmployeeTypeStackPanel(string stackPanelName)
        {
            var stackPanel = FindName(stackPanelName) as StackPanel;
            if (stackPanel != null)
            {
                stackPanel.Visibility = Visibility.Visible;
            }
        }
        private void HideEmployeeTypeStackPanels(params StackPanel[] stackPanels)
        {
            foreach (var panel in stackPanels)
            {
                if (panel != null)
                {
                    panel.Visibility = Visibility.Collapsed;
                }
            }
        }
        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (HourlyRadioButton.IsChecked == true)
            {
                ShowEmployeeTypeStackPanel("Hourly_Paid_Employee");
                HideEmployeeTypeStackPanels(Commission_Based_Employee, Weekly_Salary_Employee);
            }
            else if (CommissionRadioButton.IsChecked == true)
            {
                ShowEmployeeTypeStackPanel("Commission_Based_Employee");
                HideEmployeeTypeStackPanels(Hourly_Paid_Employee, Weekly_Salary_Employee);
            }
            else if (SalaryRadioButton.IsChecked == true)
            {
                ShowEmployeeTypeStackPanel("Weekly_Salary_Employee");
                HideEmployeeTypeStackPanels(Hourly_Paid_Employee, Commission_Based_Employee);
            }
        }
        private void Calculate_Click(object sender, RoutedEventArgs e)
        {
            EmployeeType employeeType;
            string name = "";
            if (HourlyRadioButton.IsChecked == true)
            {
                employeeType = EmployeeType.Hourly;
            }
            else if (SalaryRadioButton.IsChecked == true)
            {
                employeeType = EmployeeType.Weekly;
            }
            else if (CommissionRadioButton.IsChecked == true)
            {
                employeeType = EmployeeType.Commission;
            }
            else
            {
                MessageBox.Show("Must select an employee type.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            double hoursWorked, hourlyWage, grossEarnings, tax, netEarnings;
            if (employeeType == EmployeeType.Hourly)
            {
                name = NameTextBox1.Text;
                string hoursWorkedText = HoursWorkedTextBox.Text;
                string hourlyWageText = HourlyWageTextBox.Text;
                if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(hoursWorkedText) || string.IsNullOrWhiteSpace(hourlyWageText))
                {
                    MessageBox.Show("Make sure all fields are filled!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (!double.TryParse(hoursWorkedText, out hoursWorked) || !double.TryParse(hourlyWageText, out hourlyWage))
                {
                    MessageBox.Show("Invalid input for hours worked or hourly wage!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (hoursWorked <= 0)
                {
                    MessageBox.Show("Hours worked must be positive numbers!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                grossEarnings = CalGrossEarnings(hoursWorked, hourlyWage);
                tax = calSalaryTax(grossEarnings);
                netEarnings = CalNetEarnings(grossEarnings, tax);
            }
            else if (employeeType == EmployeeType.Commission)
            {
                name = NameTextBox2.Text;
                string commissionSalesText = GrossSalesTextBox1.Text;
                string commissionRateText = CommissionRateTextBox1.Text;
                if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(commissionSalesText) || string.IsNullOrWhiteSpace(commissionRateText))
                {
                    MessageBox.Show("Make sure all fields are filled!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (!double.TryParse(commissionSalesText, out double commissionSales) || !double.TryParse(commissionRateText, out double commissionRate))
                {
                    MessageBox.Show("Invalid input for commission sales or commission rate!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (commissionSales <= 0 || commissionRate <= 0)
                {
                    MessageBox.Show("Commission sales and commission rate must be positive numbers!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                grossEarnings = CalCommissionEarnings(commissionSales, commissionRate);
                tax = calSalaryTax(grossEarnings);
                netEarnings = CalNetEarnings(grossEarnings, tax);
            }
            else if (employeeType == EmployeeType.Weekly)
            {
                name = NameTextBox3.Text;
                string weeklySalaryText = WeeklySalaryTextBox1.Text;
                if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(weeklySalaryText)){
                    MessageBox.Show("Make sure all fields are filled!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (!double.TryParse(weeklySalaryText, out double weeklySalary)){
                    MessageBox.Show("Invalid input for weekly salary!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (weeklySalary <= 0){
                    MessageBox.Show("Weekly salary must be a positive number!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                grossEarnings = CalNetEarnings(weeklySalary);
                tax = calSalaryTax(grossEarnings);
                netEarnings = CalNetEarnings(grossEarnings, tax);
            }
            else
            {
                MessageBox.Show("Invalid employee type.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Employee newEmployee = new Employee
            {
                EmployeeID = currentEmployeeID++,
                Type = employeeType, 
                Name = name,
                GrossEarnings = grossEarnings,
                Tax = tax,
                NetEarnings = netEarnings
            };
            employees_list.Add(newEmployee);
            EmployeeListBox.ItemsSource = null;
            EmployeeListBox.ItemsSource = employees_list;
            GrossEarningsTextBox1.Text = grossEarnings.ToString("C", CultureInfo.CreateSpecificCulture("en-US"));
            TaxTextBox1.Text = tax.ToString("C", CultureInfo.CreateSpecificCulture("en-US"));
            NetEarningsTextBox1.Text = netEarnings.ToString("C", CultureInfo.CreateSpecificCulture("en-US"));
        }
        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            NameTextBox1.Text = "";
            NameTextBox2.Text = "";
            NameTextBox3.Text = "";
            HoursWorkedTextBox.Text = "";
            HourlyWageTextBox.Text = "";
            GrossEarningsTextBox1.Text = "";
            TaxTextBox1.Text = "";
            NetEarningsTextBox1.Text = "";
            GrossSalesTextBox1.Text = "";
            CommissionRateTextBox1.Text = "";
            WeeklySalaryTextBox1.Text = "";
        }
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void EmployeeList_Changed(object sender, SelectionChangedEventArgs e)
        {
            if (EmployeeListBox.SelectedItem != null)
            {
                Employee selectedEmployee = (Employee)EmployeeListBox.SelectedItem;
                NameTextBox1.Text = selectedEmployee.Name;
                GrossEarningsTextBox1.Text = selectedEmployee.GrossEarnings.ToString("C", CultureInfo.CreateSpecificCulture("en-US"));
                TaxTextBox1.Text = selectedEmployee.Tax.ToString("C", CultureInfo.CreateSpecificCulture("en-US"));
                NetEarningsTextBox1.Text = selectedEmployee.NetEarnings.ToString("C", CultureInfo.CreateSpecificCulture("en-US"));
                if (selectedEmployee.Type == EmployeeType.Hourly)
                    HourlyRadioButton.IsChecked = true;
                else if (selectedEmployee.Type == EmployeeType.Commission)
                    CommissionRadioButton.IsChecked = true;
                else if (selectedEmployee.Type == EmployeeType.Weekly)
                    SalaryRadioButton.IsChecked = true;
            }
        }
        private double CalGrossEarnings(double hoursWorked, double hourlyWage)
        {
            if (hoursWorked <= 40)
            {
                return hoursWorked * hourlyWage;
            }
            else
            {
                double regularHoursEarnings = 40 * hourlyWage;
                double overtimeHoursEarnings = (hoursWorked - 40) * hourlyWage * 1.5;
                return regularHoursEarnings + overtimeHoursEarnings;
            }
        }
        private double CalCommissionEarnings(double grossSales, double commissionRate)
        {
            return grossSales * commissionRate/100;
        }
        private double CalNetEarnings(double fixedWeeklySalary)
        {
            return fixedWeeklySalary;
        }
        private double calSalaryTax(double grossEarnings)
        {
            return grossEarnings * 0.2;
        }
        private double CalNetEarnings(double grossEarnings, double tax)
        {
            return grossEarnings - tax;
        }
    }
}
