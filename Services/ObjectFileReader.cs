using System.Globalization;

namespace OpenTKGame.Core.Formatters
{
    internal struct ModelData
    {
        public string Name;

        public List<float> Positions;
        public List<float> UVCords;
        public List<Tuple<int, int, int>> Indices;

        public ModelData()
        {
            Positions = new List<float>();
            UVCords = new List<float>();
            Indices = new List<Tuple<int, int, int>>();
        }
    }

    internal static class ObjectFileReader
    {
        public static List<ModelData> ReadFile(string filePath)
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
                        ReadElements(ref elements, models);
                }
            }

            return models;
        }

        private static void ReadElements(ref string[] elements, List<ModelData> models)
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
                case "o"://new object
                    ModelData newModel = new ModelData();
                    newModel.Name = elements[1];
                    models.Add(newModel);
                    break;

                case "v"://positions
                    for (int i = 1; i <= 3; i++)
                        curModel.Positions.Add(float.Parse(elements[i], CultureInfo.InvariantCulture));
                    break;

                case "vt"://uv cords
                    for (int i = 1; i <= 2; i++)
                        curModel.UVCords.Add(float.Parse(elements[i], CultureInfo.InvariantCulture));
                    break;

                case "f"://indices
                    for (int i = 1; i <= 3; i++)
                    {
                        string[] nums = elements[i].Split('/');

                        int pos = int.Parse(nums[0]) - 1;
                        int uv = nums.Length > 1 && nums[1] != "" ? int.Parse(nums[1]) - 1 : 0;
                        int normal = nums.Length > 2 && nums[2] != "" ? int.Parse(nums[2]) - 1 : 0;

                        curModel.Indices.Add(new Tuple<int, int, int>(pos, uv, normal));
                    }
                    break;

                case "s":
                    break;

                default:
                    throw new Exception($"Undefined befaviour: { elements[0] } in \"{ string.Join(' ', elements) }\"");
            }
        }
    }
}