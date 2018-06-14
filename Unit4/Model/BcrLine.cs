namespace Unit4.Automation.Model
{
    internal class BcrLine
    {
        public CostCentre CostCentre { get; set; }
        
        public string Account { get; set; }
        public string AccountName { get; set; }

        public double Budget { get; set; }
        public double Profile { get; set; }
        public double Actuals { get; set; }
        public double Variance { get; set; }
        public double Forecast { get; set; }
        public double OutturnVariance { get; set; }
    }
}