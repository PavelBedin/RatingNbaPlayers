using System.Xml.Linq;

namespace AllPaths
{
    public class FindPath
    {
        private string _solutionPath;
        private XDocument _doc;

        public FindPath()
        {
            _solutionPath = Directory.GetCurrentDirectory();
            for (var i = 0; i < 4; i++)
            {
                if (_solutionPath == null) continue;
                _solutionPath = Directory.GetParent(_solutionPath)?.FullName;
            }

            try
            {
                _doc = XDocument.Load(Path.Combine(_solutionPath, "pom.xml"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        public string GetFullPath(string name)
        {
            try
            {
                return Path.Combine(_solutionPath, _doc.Descendants("file")
                    .FirstOrDefault(e => e.Attribute("name")?.Value == name).Attribute("path").Value);
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine("There is no file by the specified name");
                throw;
            }
        }
    }
}