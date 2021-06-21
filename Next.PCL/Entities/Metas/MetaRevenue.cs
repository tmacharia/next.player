namespace Next.PCL.Entities
{
    public class MetaRevenue
    {
        public double? Budget { get; set; }
        public double? CumulativeGross { get; set; }
        public double? Profit => CumulativeGross - Budget;
        public double? ProfitMargin
        {
            get
            {
                if (Budget > 0)
                    return Profit / Budget * 100;
                return null;
            }
        }
        public override string ToString()
        {
            return string.Format("{0:N2}% profit, Budget: {1:C0}, Revenue: {2:C0}", ProfitMargin, Budget, CumulativeGross);
        }
    }
}