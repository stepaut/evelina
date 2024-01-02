namespace Db
{
    public interface IPortfolio : IItem
    {
        string Name { get; set; }
        string Description { get; set; }


        IAsset CreateAsset(string assetName);
        IList<IAsset> GetAssets();

        Task<bool> Save();
        Task<bool> SaveAs(string path);
    }
}
