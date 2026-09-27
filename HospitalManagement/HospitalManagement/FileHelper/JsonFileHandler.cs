using System.Text.Json;

namespace HospitalManagement.FileHelper
{
    public class JsonFileHandler<T>
    {
        private readonly string _filePath;
        private readonly JsonSerializerOptions _jsonOptions;
        private readonly SemaphoreSlim _fileLock;
        public JsonFileHandler(string filePath)
        {
            _filePath = filePath;
            _jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            _fileLock = new SemaphoreSlim(1, 1);
            CreateFileIfNotExists();
        }

        private void CreateFileIfNotExists()
        {
            if (!File.Exists(_filePath))
            {
                File.WriteAllText(_filePath, "[]");
            }
        }

        public async Task WriteAsync(T data)
        {
            await _fileLock.WaitAsync();
            try
            {
                List<T> dataList = await ReadAllInternalAsync();
                dataList.Add(data);
                string json = JsonSerializer.Serialize(dataList, _jsonOptions);
                await File.WriteAllTextAsync(_filePath, json);
            }
            finally
            {
                _fileLock.Release();
            }
        }

        public async Task WriteAllAsync(IEnumerable<T> data)
        {
            await _fileLock.WaitAsync();
            try
            {
                string json = JsonSerializer.Serialize(data, _jsonOptions);
                await File.WriteAllTextAsync(_filePath, json);
            }
            finally
            {
                _fileLock.Release();
            }
        }

        public async Task<List<T>> ReadAllAsync()
        {
            await _fileLock.WaitAsync();
            try
            {
                return await ReadAllInternalAsync();
            }
            finally
            {
                _fileLock.Release();
            }
        }
        private async Task<List<T>> ReadAllInternalAsync()
        {
            string json = await File.ReadAllTextAsync(_filePath);

            if (string.IsNullOrWhiteSpace(json))
            {
                return new List<T>();
            }

            return JsonSerializer.Deserialize<List<T>>(json, _jsonOptions) ?? new List<T>();
        }
    }
}