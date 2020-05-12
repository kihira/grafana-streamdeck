using System;
using System.Drawing;
using System.Net;
using BarRaider.SdTools;

namespace GrafanaStreamdeck
{
    [PluginActionId("rocks.foxes.grafana-streamdeck.action.rendergraph")]
    public class RenderGraphAction : PluginBase
    {
        public RenderGraphAction(SDConnection connection, InitialPayload payload) : base(connection, payload)
        {
        }

        public override void KeyPressed(KeyPayload payload)
        {
            try
            {
                var request = WebRequest.CreateHttp($"");
                request.Headers.Add(HttpRequestHeader.Authorization, $"Bearer {_apiKey}");

                var response = request.GetResponse();
                var image = Image.FromStream(response.GetResponseStream());
            
                Connection.SetImageAsync(image);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public override void KeyReleased(KeyPayload payload) { }

        public override void ReceivedSettings(ReceivedSettingsPayload payload) { }

        public override void ReceivedGlobalSettings(ReceivedGlobalSettingsPayload payload) { }

        public override void OnTick() { }

        public override void Dispose() { }

        private long _from = 1589231352591;
        private long _to = 1589234952591;
        
        private int _refreshInterval = 60;
        private string _apiKey = "";

        private static int _dimensions = 144;
    }
}