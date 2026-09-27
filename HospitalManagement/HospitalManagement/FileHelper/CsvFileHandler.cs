namespace HospitalManagement.FileHelper
{
    public class CsvFileHandler<T>
    {
        private readonly string _filePath;
        private readonly Func<T, string> _convertToCsv;
        private readonly Func<string, T> _convertFromCsv;
        private readonly SemaphoreSlim _fileLock;
        public CsvFileHandler(string filePath, Func<T, string> convertToCsv, Func<string, T> convertFromCsv)
        {
            _filePath = filePath;
            _convertToCsv = convertToCsv;
            _convertFromCsv = convertFromCsv;
            _fileLock = new SemaphoreSlim(1, 1);
            CreateFileIfNotExists();
        }

        private void CreateFileIfNotExists()
        {
            if (!File.Exists(_filePath))
            {
                File.Create(_filePath).Dispose();
            }
        }

        public async Task WriteAsync(T data, CancellationToken cancellationToken = default)
        {
            string csvLine = _convertToCsv(data);
            await _fileLock.WaitAsync(cancellationToken);
            try
            {
                await File.AppendAllTextAsync(_filePath, csvLine + Environment.NewLine, cancellationToken);
            }
            finally
            {
                _fileLock.Release();
            }
        }

        public async Task WriteAllAsync(IEnumerable<T> data, CancellationToken cancellationToken = default)
            {
            IEnumerable<string> csvLines = data.Select(_convertToCsv);
            await _fileLock.WaitAsync(cancellationToken);
            try
            {
                await File.WriteAllLinesAsync(_filePath, csvLines, cancellationToken);
            }
            finally
            {
                _fileLock.Release();
            }
        }

        public async Task<List<T>> ReadAllAsync(CancellationToken cancellationToken = default)
        {
            await _fileLock.WaitAsync(cancellationToken);

            try
            {
                List<T> result = new List<T>();
                using StreamReader reader = new StreamReader(_filePath);
                while (true)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    string? line = await reader.ReadLineAsync();
                    if (line == null)
                    {
                        break;
                    }

                    if (string.IsNullOrWhiteSpace(line))
                    {
                        continue;
                    }

                    result.Add(_convertFromCsv(line));
                }

                return result;
            }
            finally
            {
                _fileLock.Release();
            }
        }

        public static string Escape(string value)
        {
            if (value.Contains('"'))
            {
                value = value.Replace("\"", "\"\"");
            }

            if (value.Contains(',') || value.Contains('"'))
            {
                return $"\"{value}\"";
            }

            return value;
        }

        public static List<string> Parse(string line)
        {
            List<string> fields = new List<string>();
            bool q = false;
            string currentField = string.Empty;
            for (int i = 0; i < line.Length; i++)
            {
                char current = line[i];
                if (current == '"')
                {
                    if (q && i + 1 < line.Length && line[i + 1] == '"')
                    {
                        currentField += '"';
                        i++;
                    }
                    else
                    {
                        q = !q;
                    }

                    continue;
                }

                if (current == ',' && !q)
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