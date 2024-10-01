namespace PRO2TS2324EX2
{
    public interface IResultsService
    {
        string Url { get; set; }
        Results GetResults(string student);
    }
}