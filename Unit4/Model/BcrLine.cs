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

        public override bool Equals(object obj)
        {
            var other = obj as BcrLine;
            if (other == null)
            {
                return false;
            }

            return CostCentre == other.CostCentre &&
                Account == other.Account &&
                AccountName == other.AccountName &&
                Budget == other.Budget &&
                Profile == other.Profile &&
                Actuals == other.Actuals &&
                Variance == other.Variance &&
                Forecast == other.Forecast &&
                OutturnVariance == other.OutturnVariance;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = (int) 2166136261;
                hash = hash * 16777619 + CostCentre?.GetHashCode() ?? 0;
                hash = hash * 16777619 + Account?.GetHashCode() ?? 0;
                hash = hash * 16777619 + AccountName?.GetHashCode() ?? 0;
                hash = hash * 16777619 + Budget.GetHashCode();
                hash = hash * 16777619 + Profile.GetHashCode();
                hash = hash * 16777619 + Actuals.GetHashCode();
                hash = hash * 16777619 + Variance.GetHashCode();
                hash = hash * 16777619 + Forecast.GetHashCode();
                hash = hash * 16777619 + OutturnVariance.GetHashCode();
                return hash;
            }
        }
    }
}