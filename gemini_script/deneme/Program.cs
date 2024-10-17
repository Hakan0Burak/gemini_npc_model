using System;
using System.IO;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using System.Threading.Tasks;

namespace game_extension
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string logFilePath = "log.txt";

            // NPC ile iletişime geçildiğinde Python scriptini çalıştır
            Console.WriteLine("NPC ile iletişime geçildi. Bir metin girin:");

            // Kullanıcıdan metin girişi alma
            string? userInput = Console.ReadLine();

            if (!string.IsNullOrEmpty(userInput))
            {
                // Python scriptini çalıştır ve sonucu al
                var result = await ExecutePythonScript(userInput);

                // Sonuçları ekrana ve log dosyasına yazdır
                Console.WriteLine($"Sonuç: {result}");
                LogToFile(logFilePath, userInput, result);
            }
            else
            {
                Console.WriteLine("Geçersiz giriş.");
                LogToFile(logFilePath, "Geçersiz giriş");
            }
        }

        static async Task<dynamic> ExecutePythonScript(string inputText)
        {
            try
            {
                // Python motorunu başlatma
                ScriptEngine engine = Python.CreateEngine();
                ScriptScope scope = engine.CreateScope();

                // Python dosyasını yükleme
                await Task.Run(() => engine.ExecuteFile("gemini_script.py", scope));

                // Python fonksiyonunu çağırma
                dynamic processInput = scope.GetVariable("process_input");

                // Python fonksiyonuna metni gönderme
                return processInput(inputText);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Bir hata oluştu: " + ex.Message);
                return "Hata: " + ex.Message;
            }
        }

        static void LogToFile(string filePath, string inputText, dynamic result)
        {
            using (StreamWriter sw = File.AppendText(filePath))
            {
                sw.WriteLine($"Giriş: {inputText}, Çıkış: {result}");
            }
        }

        static void LogToFile(string filePath, string message)
        {
            using (StreamWriter sw = File.AppendText(filePath))
            {
                sw.WriteLine(message);
            }
        }
    }
}
