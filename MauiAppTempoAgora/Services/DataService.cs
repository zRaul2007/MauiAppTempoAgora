﻿using MauiAppTempoAgora.Models;
using Newtonsoft.Json.Linq;

namespace MauiAppTempoAgora.Services
{
    public class DataService
    {
        public static async Task<Tempo?> GetPrevisao(string cidade)
        {
            Tempo? t = null;

            string chave = "6135072afe7f6cec1537d5cb08a5a1a2";

            string url = $"https://api.openweathermap.org/data/2.5/weather?" +
                         $"q={cidade}&units=metric&appid={chave}";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage resp = await client.GetAsync(url);

                    if (resp.IsSuccessStatusCode)
                    {
                        string json = await resp.Content.ReadAsStringAsync();

                        var rascunho = JObject.Parse(json);

                        DateTime time = new();
                        DateTime sunrise = time.AddSeconds((double)rascunho["sys"]["sunrise"]).ToLocalTime();
                        DateTime sunset = time.AddSeconds((double)rascunho["sys"]["sunset"]).ToLocalTime();

                        t = new()
                        {
                            lat = (double)rascunho["coord"]["lat"],
                            lon = (double)rascunho["coord"]["lon"],
                            description = (string)rascunho["weather"][0]["description"],
                            main = (string)rascunho["weather"][0]["main"],
                            temp_min = (double)rascunho["main"]["temp_min"],
                            temp_max = (double)rascunho["main"]["temp_max"],
                            speed = (double)rascunho["wind"]["speed"],
                            visibility = (int)rascunho["visibility"],
                            sunrise = sunrise.ToString(),
                            sunset = sunset.ToString(),
                        }; // Fecha obj do Tempo.
                    } // Fecha if se o status do servidor foi de sucesso
                    else if (resp.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        await Application.Current.MainPage.DisplayAlert("Erro", "Cidade não encontrada.", "OK");
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Erro", $"Erro ao buscar dados: {resp.StatusCode}", "OK");
                    }
                }
                catch (HttpRequestException)
                {
                    await Application.Current.MainPage.DisplayAlert("Sem conexão", "Verifique sua internet e tente novamente.", "OK");
                }
            } // fecha laço using


            return t;
        }
    }
}