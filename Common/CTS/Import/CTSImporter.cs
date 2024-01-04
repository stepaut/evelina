using Db;
using Microsoft.VisualBasic.FileIO;

namespace CTS.Import
{
    public class CTSImporter : IDisposable
    {
        private string _path;
        private List<CTS> _transactions;


        public CTSImporter(string path)
        {
            _path = path;
            _transactions = new List<CTS>();
        }


        public void Read()
        {
            Dictionary<ECTSFields, int> columnIndexes = new();

            using (TextFieldParser parser = new TextFieldParser(_path))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                while (!parser.EndOfData)
                {
                    //Processing row
                    string[] fields = parser.ReadFields();

                    if (string.IsNullOrEmpty(fields[0]))
                    {
                        // this means header row
                        for (int i = 0; i < fields.Length; i++)
                        {
                            if (Enum.TryParse<ECTSFields>(fields[i], out var res))
                            {
                                if (columnIndexes.ContainsKey(res))
                                {
                                    throw new Exception("wrong header");
                                }

                                columnIndexes[res] = i;
                            }
                        }

                        CheckAllRequiredExist(columnIndexes.Keys);

                        continue;
                    }

                    CTS cts = new CTS();

                    foreach (ECTSFields field in columnIndexes.Keys)
                    {
                        int i = columnIndexes[field];
                        cts.SetField(field, fields[i]);
                    }

                    _transactions.Add(cts);
                }
            }
        }

        public void Dispose()
        {
            _transactions.Clear();
        }

        public void AddToPortfolio(IPortfolio portfolio, bool createNewAssets = true)
        {
            foreach (CTS cts in _transactions)
            {
                IAsset asset = portfolio.GetAsset(cts.Symbol);
                if (asset is null)
                {
                    if (!createNewAssets)
                    {
                        continue;
                    }

                    asset = portfolio.CreateAsset(cts.Symbol);
                }

                ETransaction type = cts.Type == EType.buy ? ETransaction.Buy : ETransaction.Sell;

                asset.CreateTransaction(cts.Datetime, type, cts.Price, cts.Amount);
            }
        }

        private void CheckAllRequiredExist(IEnumerable<ECTSFields> source)
        {
            foreach (ECTSFields field in (ECTSFields[])Enum.GetValues(typeof(ECTSFields)))
            {
                if (field.IsRequired() && !source.Contains(field))
                {
                    throw new Exception($"missed required column: {field}");
                }
            }
        }
    }
}
