namespace MarketSpot.Models.Interface
{
    public interface ICSVManipulator
    {
        string[] ReadSymbolList(string symbolpath);
    }
}