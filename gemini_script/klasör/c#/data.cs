using System;
using LiteDB;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace database_log_deneme
{
    class GeminiLogger
    {
        private static string logFilePath = @"C:\Users\New\Desktop\log.txt";
        private static string databasePath = @"MyData.db";
        private static string apiKey = "AIzaSyDvhqhfAVCtVuiSKicolVnCsPCr8G8BfQY";  // Gemini API anahtarı

        // Kullanıcı girdisini log dosyasına yazan metot
        public static void LogPrompt(string prompt)
        {
            Console.WriteLine("LogPrompt metodu çağrıldı: " + prompt);
            lock (logFilePath)
            {
                try
                {
                    using (StreamWriter writer = new StreamWriter(logFilePath, true))
                    {
                        writer.WriteLine("Player: " + prompt);
                        writer.WriteLine();
                    }
                    Console.WriteLine("Prompt log dosyasına kaydedildi.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Log dosyasına yazma sırasında hata oluştu: " + ex.Message);
                }
            }
        }

        // Gemini API'ye veri gönderme ve sonucu alma işlemi
        public static async Task<string> SendDataToGemini(string prompt)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var requestUrl = $"https://api.example.com/v1/gemini?key={apiKey}";  // Örnek URL, doğru URL'yi kullanın
                    var jsonContent = new StringContent($"{{\"prompt\": \"{prompt}\"}}", Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PostAsync(requestUrl, jsonContent);
                    response.EnsureSuccessStatusCode();

                    string responseBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Gemini'den gelen yanıt: " + responseBody);

                    // Yanıtı log dosyasına kaydet
                    LogResponse(responseBody);

                    return responseBody;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Gemini API isteğinde hata oluştu: " + ex.Message);
                    return string.Empty;
                }
            }
        }

        // API'den gelen yanıtı log dosyasına yazan metot
        private static void LogResponse(string response)
        {
            lock (logFilePath)
            {
                try
                {
                    using (StreamWriter writer = new StreamWriter(logFilePath, true))
                    {
                        writer.WriteLine("Gemini Response: " + response);
                        writer.WriteLine();
                    }
                    Console.WriteLine("Gemini yanıtı log dosyasına kaydedildi.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Log dosyasına yazma sırasında hata oluştu: " + ex.Message);
                }
            }
        }
    }

    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Log dosyasının içeriği:");
            Console.WriteLine(File.ReadAllText(@"C:\Users\New\Desktop\log.txt"));

            string? input;
            do
            {
                Console.WriteLine("Log dosyasına yazılacak yeni bir prompt girin (çıkmak için 'exit' yazın):");
                input = Console.ReadLine();

                if (input != null && input.ToLower() != "exit")
                {
                    GeminiLogger.LogPrompt(input);
                    string response = await GeminiLogger.SendDataToGemini(input);
                    Console.WriteLine("Gemini yanıtı: " + response);
                }

            } while (input != null && input.ToLower() != "exit");
        }
    }
}
