using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Library.Tools.Conversions
{
    public static class JsonConvert
    {

        public static async Task<T?> DeserializeJsonAsync<T>(string Content) =>
            await DeserializeJsonAsync<T>(new System.IO.MemoryStream(Encoding.UTF8.GetBytes(Content)));

        public static async Task<T?> DeserializeJsonAsync<T>(Stream Content)
        {
            try
            {
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                };

                return await System.Text.Json.JsonSerializer.DeserializeAsync<T>(Content, options);
            }
            catch (Exception) // Invalid JSON
            {
                throw new Exception("Can not Deserialize Class Object");
            }
        }

        public static async Task<string?> SerializeJsonAsync<T>(T Content)
        {
            try
            {
                string json = string.Empty;
                using (var stream = new MemoryStream())
                {
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true, 
                        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                    };

                    await JsonSerializer.SerializeAsync<T>(stream, Content, options);
                    stream.Position = 0;
                    using (var reader = new StreamReader(stream))
                    {
                        return await reader.ReadToEndAsync();
                    }
                }
            }
            catch (Exception) // Invalid JSON
            {
                return null;
            }
        }
    }
}
