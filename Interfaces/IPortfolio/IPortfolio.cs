namespace Db
{
    public delegate void UpdateVisualStat();

    public interface IPortfolio : IItem
    {
        event UpdateVisualStat UpdateVisualStatEvent;

        #region required
        string Name { get; set; }
        #endregion

        #region additional
        string Description { get; set; }
        #endregion

        #region not_saveable
        double Volume { get; set; }
        #endregion

        IAsset CreateAsset(string assetName);
        IList<IAsset> GetAssets();
        void DeleteAsset(IAsset asset);

        Task<bool> Save();
        Task<bool> SaveAs(string path);
    }
}
