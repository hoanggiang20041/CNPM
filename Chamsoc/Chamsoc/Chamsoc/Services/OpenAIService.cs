﻿using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace Chamsoc.Services
{
    public class OpenAIService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public OpenAIService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<string> AskChatGPT(string prompt)
        {
            var apiKey = _configuration["OpenAI:ApiKey"];
            var baseUrl = _configuration["OpenAI:BaseUrl"];
            var referer = _configuration["OpenAI:Referer"];
            var title = _configuration["OpenAI:Title"];

            if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(baseUrl))
                return "❌ Cấu hình API Key hoặc BaseUrl chưa chính xác.";

            // Dọn dẹp headers trước khi thêm mới (tránh trùng lặp nếu gọi nhiều lần)
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", apiKey);
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Add("HTTP-Referer", referer ?? "");
            _httpClient.DefaultRequestHeaders.Add("X-Title", title ?? "");

            var requestBody = new
            {
                model = "gpt-3.5-turbo",
                messages = new[]
                {
                    new { role = "user", content = prompt }
                },
                temperature = 0.7
            };

            var requestJson = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(requestJson, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync(baseUrl, content);

                if (!response.IsSuccessStatusCode)
                {
                    return $"❌ Lỗi xảy ra từ OpenAI: {response.StatusCode}";
                }

                var responseJson = await response.Content.ReadAsStringAsync();

                using var doc = JsonDocument.Parse(responseJson);
                var result = doc.RootElement
                                .GetProperty("choices")[0]
                                .GetProperty("message")
                                .GetProperty("content")
                                .GetString();

                return result?.Trim() ?? "❌ Không có nội dung phản hồi.";
            }
            catch (Exception ex)
            {
                return $"❌ Đã xảy ra lỗi khi gọi OpenAI: {ex.Message}";
            }
        }
    }
}
