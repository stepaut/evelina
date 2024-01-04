namespace Db
{
    public interface IPortfolio : IItem
    {
        #region required
        string Name { get; set; }
        #endregion

        #region additional
        string Description { get; set; }
        #endregion


        IAsset CreateAsset(string assetName);
        IList<IAsset> GetAssets();
        void DeleteAsset(IAsset asset);

        Task<bool> Save();
        Task<bool> SaveAs(string path);
    }
}
