using System.Xml.Linq;

namespace AllPaths
{
    public class FindPath
    {
        private XDocument _doc;

        public FindPath()
        {
            var solutionPath = Directory.GetCurrentDirectory();
            for (var i = 0; i < 4; i++)
            {
                if (solutionPath == null) continue;
                solutionPath = Directory.GetParent(solutionPath)?.FullName;
            }

            try
            {
                _doc = XDocument.Load(Path.Combine(solutionPath, "pom.xml"));
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
                return _doc.Descendants("file")
                    .FirstOrDefault(e => e.Attribute("name")?.Value == name).Value;
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine("There is no file by the specified name");
                throw;
            }
        }
    }
}