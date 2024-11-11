using Newtonsoft.Json;

public class WatchProviders
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("results")]
    public Results Results { get; set; }
}

public class Results
{
    [JsonProperty("US")]
    public US US { get; set; }
}

public class US
{
    [JsonProperty("link")]
    public string Link { get; set; }

    [JsonProperty("buy")]
    public List<Buy> Buy { get; set; }

    [JsonProperty("rent")]
    public List<Rent> Rent { get; set; }

    [JsonProperty("flatrate")]
    public List<Flatrate> Flatrate { get; set; }

    [JsonProperty("ads")]
    public List<Ad> Ads { get; set; }
}

public class Ad
{
    [JsonProperty("logo_path")]
    public string LogoPath { get; set; }

    [JsonProperty("provider_id")]
    public int ProviderId { get; set; }

    [JsonProperty("provider_name")]
    public string ProviderName { get; set; }

    [JsonProperty("display_priority")]
    public int DisplayPriority { get; set; }
}

public class Buy
{
    [JsonProperty("logo_path")]
    public string LogoPath { get; set; }

    [JsonProperty("provider_id")]
    public int ProviderId { get; set; }

    [JsonProperty("provider_name")]
    public string ProviderName { get; set; }

    [JsonProperty("display_priority")]
    public int DisplayPriority { get; set; }
}

public class Flatrate
{
    [JsonProperty("logo_path")]
    public string LogoPath { get; set; }

    [JsonProperty("provider_id")]
    public int ProviderId { get; set; }

    [JsonProperty("provider_name")]
    public string ProviderName { get; set; }

    [JsonProperty("display_priority")]
    public int DisplayPriority { get; set; }
}

public class Rent
{
    [JsonProperty("logo_path")]
    public string LogoPath { get; set; }

    [JsonProperty("provider_id")]
    public int ProviderId { get; set; }

    [JsonProperty("provider_name")]
    public string ProviderName { get; set; }

    [JsonProperty("display_priority")]
    public int DisplayPriority { get; set; }
}
