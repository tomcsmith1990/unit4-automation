using System;

namespace Unit4.Automation.Model
{
    internal class CostCentre : IComparable<CostCentre>
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

        public int CompareTo(CostCentre other)
        {
            if (other == null) return 1;

            var result = 0;
            if (result == 0) result = Tier1.NullSafeCompareTo(other.Tier1);
            if (result == 0) result = Tier2.NullSafeCompareTo(other.Tier2);
            if (result == 0) result = Tier3.NullSafeCompareTo(other.Tier3);
            if (result == 0) result = Tier4.NullSafeCompareTo(other.Tier4);
            if (result == 0) result = Code.NullSafeCompareTo(other.Code);

            return result;
        }

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
                hash = hash * 16777619 ^ Tier1?.GetHashCode() ?? 0;
                hash = hash * 16777619 ^ Tier2?.GetHashCode() ?? 0;
                hash = hash * 16777619 ^ Tier3?.GetHashCode() ?? 0;
                hash = hash * 16777619 ^ Tier4?.GetHashCode() ?? 0;
                hash = hash * 16777619 ^ Code?.GetHashCode() ?? 0;
                hash = hash * 16777619 ^ Tier1Name?.GetHashCode() ?? 0;
                hash = hash * 16777619 ^ Tier2Name?.GetHashCode() ?? 0;
                hash = hash * 16777619 ^ Tier3Name?.GetHashCode() ?? 0;
                hash = hash * 16777619 ^ Tier4Name?.GetHashCode() ?? 0;
                hash = hash * 16777619 ^ CostCentreName?.GetHashCode() ?? 0;
                return hash;
            }
        }
    }
}