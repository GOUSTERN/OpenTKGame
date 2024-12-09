using System.Globalization;

namespace OpenTKGame.Core.Formatters
{
    public struct ModelData
    {
        public string Name;

        public List<float> Vertices;
        public List<uint> Indices;

        public ModelData()
        {
            Vertices = new List<float>();
            Indices = new List<uint>();
        }
    }

    public static class ObjectFileReader
    {
        public static List<ModelData> ReadFile(string filePath, uint indecesStart = 0)
        {
            List<ModelData> models = new List<ModelData>();

            using (StreamReader streamReader = new StreamReader(Directory.GetCurrentDirectory() + "\\Resources\\Models\\" + filePath))
            {

                string line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    if (line.Contains('#'))
                        line = line.Substring(0, line.IndexOf('#'));

                    string[] elements = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                    if (elements.Count() != 0)
                        ReadElements(elements, models, indecesStart);
                }
            }

            return models;
        }

        private static void ReadElements(string[] elements, List<ModelData> models, uint indecesStart)
        {
            if (models.Count == 0)
            {
                ModelData newModel = new ModelData();
                newModel.Name = elements[1];
                models.Add(newModel);
                return;
            }

            ModelData curModel = models[models.Count - 1];
            switch (elements[0])
            {
                case "o":
                    ModelData newModel = new ModelData();
                    newModel.Name = elements[1];
                    models.Add(newModel);
                    break;

                case "v":
                    for (int i = 1; i <= 3; i++)//  add x y z
                        curModel.Vertices.Add(float.Parse(elements[i], CultureInfo.InvariantCulture));
                    break;
                
                case "f":
                    for (int i = 1; i <= 3; i++)
                        curModel.Indices.Add(uint.Parse(elements[i]) - indecesStart);
                    break;

                case "s":
                    break;

                default:
                    throw new Exception("Undefined befaviour: { elements[0] } on + { string.Join(' ', elements) }");
            }
        }
    }
}