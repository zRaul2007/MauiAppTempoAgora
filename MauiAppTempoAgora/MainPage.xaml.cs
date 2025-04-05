using MauiAppTempoAgora.Models;
using MauiAppTempoAgora.Services;

namespace MauiAppTempoAgora
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txt_cidade.Text))
                {
                    Tempo? t = await DataService.GetPrevisao(txt_cidade.Text);

                    if (t != null)
                    {
                        string dados_previsao = "";

                        dados_previsao = $"📍 Localização: {t.lat}, {t.lon}\n" +
                                         $"🌡️ Temp. Mínima: {t.temp_min}°C\n" +
                                         $"🌡️ Temp. Máxima: {t.temp_max}°C\n" +
                                         $"🌤️ Clima: {t.main} - {t.description}\n" +
                                         $"🌬️ Vento: {t.speed} m/s\n" +
                                         $"👁️ Visibilidade: {t.visibility} m\n" +
                                         $"🌅 Nascer do Sol: {t.sunrise}\n" +
                                         $"🌇 Pôr do Sol: {t.sunset}";

                        lbl_res.Text = dados_previsao;

                    }
                    else
                    {

                        lbl_res.Text = "❌ Sem dados de Previsão";
                    }

                }
                else
                {
                    lbl_res.Text = "Preencha a cidade.";
                }

            }
            catch (Exception ex)
            {
                await DisplayAlert("Ops", ex.Message, "OK");
            }
        }
    }

}