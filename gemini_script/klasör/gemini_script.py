import os
import google.generativeai as genai
import time

# Google Gemini API anahtarıyla konfigürasyon
genai.configure(api_key="AIzaSyDYm5jAn7xVH-B4uysxFrqgUg0PRBU2Uxo")

# Log dosyasını okuma
log_file_path = 'C:/Users/New/Desktop/log.txt'  # C# bu dosyaya veri yazıyor.
if os.path.exists(log_file_path):
    with open(log_file_path, 'r') as file:
        log_content = file.read()  # C# tarafından yazılan logu okuyoruz.
        print("Log Dosyasından Veri Okundu: \n", log_content)
else:
    print("Log dosyası bulunamadı.")
    log_content = ""

# Yanıt üretim yapılandırması
generation_config = {
    "temperature": 2,
    "top_p": 0.9,
    "top_k": 32,
    "max_output_tokens": 2048,
    "response_mime_type": "text/plain",
}

# Modeli başlat
model = genai.GenerativeModel(
    model_name="gemini-1.5-flash",
    generation_config=generation_config,
)

# Sohbet oturumunu başlat
history = []
chat_session = model.start_chat(history=history)

# Log dosyasındaki veriyi Google Gemini API'ye gönderiyoruz
response = chat_session.send_message(log_content)
print("Gemini Yanıtı:", response.text)
