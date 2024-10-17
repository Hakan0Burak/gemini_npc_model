# -*- coding: utf-8 -*-
import os
import google.generativeai as genai
import time

# Google Gemini API anahtarıyla konfigürasyon
genai.configure(api_key="AIzaSyDYm5jAn7xVH-B4uysxFrqgUg0PRBU2Uxo")

# Yanıt üretim yapılandırması (parametreler ayarlanabilir)
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

# Log dosyasını okuma
log_file_path = 'C:/Users/New/Desktop/log.txt'
if os.path.exists(log_file_path):
    with open(log_file_path, 'r', encoding='utf-8') as file:
        log_content = file.read()
        print("Log Dosyasından Veri Okundu: \n", log_content)
else:
    print("Log dosyası bulunamadı.")
    log_content = ""

# Sohbet oturumunu başlat
history = []
chat_session = model.start_chat(history=history)

# Kullanıcıdan giriş alıp sohbeti başlat
while True:
    input_user = input("Sen: ")

    # Giriş 'exit' olduğunda döngüden çık
    if input_user.lower() == 'exit':
        print("Sohbet sonlandırıldı.")
        break
    
    # Zaman ölçümüne başla
    start_time = time.time()

    # Gemini API'ye mesaj gönder ve yanıtı al
    response = chat_session.send_message(input_user)
    
    # Zaman ölçümünü bitir
    end_time = time.time()

    # Yanıtı yazdır
    model_response = response.text
    print("\n")
    print("Bot:", model_response)

    # Yanıt süresini hesapla
    response_time = end_time - start_time
    print(f"Yanıt süresi: {response_time:.2f} saniye")
    print()

    # Sohbet geçmişini güncelle
    history.append({'role': 'user', 'part': [input_user]})
    history.append({'role': 'bot', 'part': [model_response]})
