namespace HospitalManagement.FileHelper
{
    public class CsvFileHandler<T>
    {
        private readonly string _filePath;
        private readonly Func<T, string> _convertToCsv;
        private readonly Func<string, T> _convertFromCsv;

        public CsvFileHandler(string filePath,Func<T, string> convertToCsv,Func<string, T> convertFromCsv)
        {
            _filePath = filePath;
            _convertToCsv = convertToCsv;
            _convertFromCsv = convertFromCsv;
            CreateFileIfNotExists();
        }

        private void CreateFileIfNotExists()
        {
            if (!File.Exists(_filePath))
            {
                File.Create(_filePath).Dispose();
            }
        }

        public void Write(T data)
        {
            string csvLine = _convertToCsv(data);
            File.AppendAllText(_filePath, csvLine + Environment.NewLine);
        }

        public void WriteAll(IEnumerable<T> data)
        {
            IEnumerable<string> csvLines = data.Select(_convertToCsv);
            File.WriteAllLines(_filePath, csvLines);
        }

        public List<T> ReadAll()
        {
            List<T> result = new List<T>();
            foreach (string line in File.ReadLines(_filePath))
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                result.Add(_convertFromCsv(line));
            }

            return result;
        }

        public static string Escape(string value)
        {
            if (value.Contains('"'))
            {
                value = value.Replace("\"", "\"\"");
            }

            if (value.Contains(',') || value.Contains('"') || value.Contains('\n') || value.Contains('\r'))
            {
                return $"\"{value}\"";
            }

            return value;
        }

        public static List<string> Parse(string line)
        {
            List<string> fields = new List<string>();
            bool insideQuotes = false;
            string currentField = string.Empty;
            for (int i = 0; i < line.Length; i++)
            {
                char current = line[i];
                if (current == '"')
                {
                    if (insideQuotes && i + 1 < line.Length && line[i + 1] == '"')
                    {
                        currentField += '"';
                        i++;
                    }
                    else
                    {
                        insideQuotes = !insideQuotes;
                    }

                    continue;
                }

                if (current == ',' && !insideQuotes)
                {
                    fields.Add(currentField);
                    currentField = string.Empty;
                    continue;
                }

                currentField += current;
            }

            fields.Add(currentField);
            return fields;
        }
    }
}