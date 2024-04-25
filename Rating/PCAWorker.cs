using System.Globalization;
using AllPaths;
using PlayerDatabase;
using Python.Runtime;

namespace Rating;

public class PCAWorker
{
    public void MakePCA(IEnumerable<TraditionalStatistics> tradStat, IEnumerable<AdvancedStatistics> advStat)
    {
        var data = new List<double>();
        var path = new FindPath();
        Runtime.PythonDLL = path.GetPythonPath();
        PythonEngine.Initialize();
        using (Py.GIL())
        {
            dynamic sys = Py.Import("sys");
            sys.path.append(path.GetFullPath("PCA"));
            dynamic pcaModule = Py.Import("PCA");
            dynamic pcaBuilder = pcaModule.PCA_builder();
            dynamic result = pcaBuilder.make_PCA(ToListDouble(tradStat), ToListDouble(advStat));
            foreach (var item in result)
            {
                string str = item.ToString();
                var newStr = new string(str.Where(c => c != '[' && c != ']').ToArray());
                data.Add(double.Parse(newStr, CultureInfo.InvariantCulture));
            }
        }

        PythonEngine.Shutdown();
    }

    private List<double[]> ToListDouble<T>(IEnumerable<T> stat)
    {
        var type = typeof(T);
        var properties = type.GetProperties();
        return stat.Select(item => properties.Select(p => Convert.ToDouble(p.GetValue(item))).ToArray()).ToList();
    }
}