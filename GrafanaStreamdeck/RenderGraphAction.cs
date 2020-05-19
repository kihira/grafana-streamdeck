using System;
using System.Drawing;
using System.Net;
using System.Threading.Tasks;
using BarRaider.SdTools;
using Newtonsoft.Json.Linq;

namespace GrafanaStreamdeck
{
    [PluginActionId("rocks.foxes.grafana-streamdeck.action.rendergraph")]
    public class RenderGraphAction : PluginBase
    {
        public RenderGraphAction(SDConnection connection, InitialPayload payload) : base(connection, payload)
        {
            if (payload.Settings == null || payload.Settings.Count == 0)
            {
                _settings = new RenderGraphSettings();
                Connection.SetSettingsAsync(JObject.FromObject(_settings));
            }
            else
            {
                _settings = payload.Settings.ToObject<RenderGraphSettings>();
            }
        }

        public override async void KeyPressed(KeyPayload payload)
        {
            await UpdateKey();
        }

        public override void KeyReleased(KeyPayload payload) { }

        public override void ReceivedSettings(ReceivedSettingsPayload payload)
        {
            Tools.AutoPopulateSettings(_settings, payload.Settings);
        }

        public override void ReceivedGlobalSettings(ReceivedGlobalSettingsPayload payload) { }

        public override async void OnTick()
        {
            if (_settings.RefreshInterval <= 0) return;
            
            if (DateTimeOffset.Now - _lastUpdateTime > TimeSpan.FromSeconds(_settings.RefreshInterval))
            {
                _from = (DateTimeOffset.Now - TimeSpan.FromSeconds(_settings.RefreshInterval)).ToUnixTimeMilliseconds();
                _to = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                
                await UpdateKey();
            }
        }

        public override void Dispose() { }

        private async Task UpdateKey()
        {
            _lastUpdateTime = DateTimeOffset.Now;
            
            if (!_settings.IsValid())
            {
                return;
            }
            
            var request = WebRequest.CreateHttp($"{_settings.Url}?orgId={_settings.OrgId}&from={_from}&to={_to}&var-interval=1m&panelId={_settings.PanelId}&width={_dimensions}&height={_dimensions}&tz=Europe%2FLondon");
            request.Headers.Add(HttpRequestHeader.Authorization, $"Bearer {_settings.ApiKey}");

            try
            {
                var response = request.GetResponse();
                var image = Image.FromStream(response.GetResponseStream());

                await Connection.SetImageAsync(image);
            }
            // Invalid credentials
            catch (WebException e) when (e.Status == WebExceptionStatus.ProtocolError)
            {
            }
            catch (WebException e)
            {
                // Everything other web exception
            }
        }

        private long _from = 1589231352591;
        private long _to = 1589234952591;
        private DateTimeOffset _lastUpdateTime;
        
        private readonly RenderGraphSettings _settings;
        
        private const int _dimensions = 144;
    }
}