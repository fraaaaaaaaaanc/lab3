namespace MakingDecisionSolver
{
    class Alternative
    {
        public string Name { get; set; } = "";        
        public decimal incProbability { get; set; } = 0M;
        public decimal nchangeProbability { get; set; } = 0M;
        public decimal decProbability { get; set; } = 0M;
        public decimal incProfit { get; set; } = 0M;
        public decimal nchangeProfit { get; set; } = 0M;
        public decimal decProfit { get; set; } = 0M;
        public decimal FindAlternativeProfit()
        {
            return incProbability * incProfit + nchangeProbability * nchangeProfit + decProbability * decProfit;
        }
        public void SetField(string name, decimal value)
        {
            typeof(Alternative).GetProperty(name).SetValue(this, value);            
        }
        public T GetField<T>(string name)
        {
            return (T)typeof(Alternative).GetProperty(name).GetValue(this);            
        }
    }
}
