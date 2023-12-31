namespace Db
{
    public static class PortfolioFactory
    {
        public static IPortfolio CreatePortfolio(string name)
        {
            return Portfolio.CreatePortfolio(name);
        }
        
        public static IPortfolio ReadPortfolio(string path)
        {
            return Portfolio.ReadPortfolio(path);
        }
    }
}
