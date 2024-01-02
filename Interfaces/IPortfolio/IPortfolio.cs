namespace Db
{
    public interface IPortfolio : IItem
    {
        string Name { get; }
        string Description { get; }


        IAsset CreateAsset(string assetName);
        IList<IAsset> GetAssets();

        Task<bool> Save();
        Task<bool> SaveAs(string path);

        void ChangeName(string newName);
        void ChangeDescription(string newDescription);
    }
}
