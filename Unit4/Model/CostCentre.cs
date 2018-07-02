namespace Unit4.Automation.Model
{
    internal class CostCentre
    {
        public string Tier1 { get; set; }
        public string Tier2 { get; set; }
        public string Tier3 { get; set; }
        public string Tier4 { get; set; }
        public string Code { get; set; }

        public string Tier1Name { get; set; }
        public string Tier2Name { get; set; }
        public string Tier3Name { get; set; }
        public string Tier4Name { get; set; }
        public string CostCentreName { get; set; }

        public override bool Equals(object obj)
        {
            var other = obj as CostCentre;
            if (other == null)
            {
                return false;
            }

            return Tier1 == other.Tier1 &&
                Tier2 == other.Tier2 &&
                Tier3 == other.Tier3 &&
                Tier4 == other.Tier4 &&
                Code == other.Code &&
                Tier1Name == other.Tier1Name &&
                Tier2Name == other.Tier2Name &&
                Tier3Name == other.Tier3Name &&
                Tier4Name == other.Tier4Name &&
                CostCentreName == other.CostCentreName;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = (int) 2166136261;
                hash = hash * 16777619 + Tier1.GetHashCode();
                hash = hash * 16777619 + Tier2.GetHashCode();
                hash = hash * 16777619 + Tier3.GetHashCode();
                hash = hash * 16777619 + Tier4.GetHashCode();
                hash = hash * 16777619 + Code.GetHashCode();
                hash = hash * 16777619 + Tier1Name.GetHashCode();
                hash = hash * 16777619 + Tier2Name.GetHashCode();
                hash = hash * 16777619 + Tier3Name.GetHashCode();
                hash = hash * 16777619 + Tier4Name.GetHashCode();
                hash = hash * 16777619 + CostCentreName.GetHashCode();
                return hash;
            }
        }

    }
}