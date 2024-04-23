using AllPaths;
using PlayerDatabase;
using Python.Runtime;

namespace Rating;

public class PCAWorker
{
    public void MakePCA(IEnumerable<TraditionalStatistics> tradStat, IEnumerable<AdvancedStatistics> advStat)
    {
        var path = new FindPath();
        // PythonEngine.PythonHome = path.GetFullPath("PCA");
        PythonEngine.PythonHome = @"C:\Users\Pasha\AppData\Local\Programs\Python\Python311";
        PythonEngine.Initialize();

        dynamic module = Py.Import("PCA");
        dynamic pcaBuilder = module.PCA_builder();
        dynamic transformedData = pcaBuilder.make_PCA(ToListDouble(tradStat), ToListDouble(advStat));
        foreach (var item in transformedData)
        {
            Console.WriteLine(item);
        }
        PythonEngine.Shutdown();
    }

    private List<double[]> ToListDouble<T>(IEnumerable<T> stat)
    {
        var type = typeof(T);
        var properties = type.GetProperties();
        return stat.Select(item => properties.Select(p => (double)p.GetValue(item)).ToArray()).ToList();
    }
}