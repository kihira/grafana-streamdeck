using Newtonsoft.Json;

namespace GrafanaStreamdeck
{
    public class RenderGraphSettings
    {
        [JsonProperty(PropertyName = "api_key")]
        public string ApiKey { get; set; }

        [JsonProperty(PropertyName = "refresh_interval")]
        public int RefreshInterval { get; set; } = 60;
        
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }
        
        [JsonProperty(PropertyName = "org_id")]
        public int OrgId { get; set; }
        
        [JsonProperty(PropertyName = "panel_id")]
        public int PanelId { get; set; }

        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(ApiKey) && !string.IsNullOrWhiteSpace(Url);
        }
    }
}