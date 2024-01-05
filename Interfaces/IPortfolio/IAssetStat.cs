namespace Db
{
    public interface IAssetStat
    {
        double Volume { get; set; }
        double SellPrice { get; set; }
        double Share { get; set; }
        double BuyedVolume { get; set; }
        double BuyedShare { get; set; }
        EAssetStatus Status { get; set; }
    }
}
