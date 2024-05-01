using System.Globalization;
using AllPaths;
using PlayerDatabase;
using Python.Runtime;

namespace Rating;

public class PCAWorker
{
    public List<double[]> MakePCA(IEnumerable<TraditionalStatistics> tradStat, IEnumerable<AdvancedStatistics> advStat,
        bool isShowСhart = false)
    {
        var data = new List<double[]>();
        var path = new FindPath();
        Runtime.PythonDLL = path.GetPythonPath();
        PythonEngine.Initialize();
        using (Py.GIL())
        {
            dynamic sys = Py.Import("sys");
            sys.path.append(path.GetFullPath("PCA"));
            dynamic pcaModule = Py.Import("PCA");
            dynamic pcaBuilder = pcaModule.PCA_builder();
            dynamic result = pcaBuilder.make_PCA(ToListDouble(tradStat), ToListDouble(advStat), 4);
            foreach (var item in result)
            {
                string str = item.ToString();
                var newStr = new string(str.Where(c => c != '(' && c != ')' && c != ',').ToArray());
                data.Add(newStr.Replace(".", ",")
                    .Split().Where(x => !string.IsNullOrEmpty(x))
                    .Select(double.Parse)
                    .ToArray());
            }
            if(isShowСhart)
                pcaBuilder.show_new_component(TakeCoordinate(data, 2), TakeCoordinate(data, 3));
        }

        PythonEngine.Shutdown();


        return data;
    }

    private List<double[]> ToListDouble<T>(IEnumerable<T> stat)
    {
        var type = typeof(T);
        var properties = type.GetProperties().Skip(1);
        return stat
            .Select(item => properties
                .Select(p => Convert.ToDouble(p.GetValue(item)))
                .ToArray())
            .ToList();
    }

    private double[] TakeCoordinate(List<double[]> data, int i)
    {
        return data.Select(x => x[i]).ToArray();
    }
}