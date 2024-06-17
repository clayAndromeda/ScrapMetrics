using System.Text.Json.Serialization;

namespace ScrapMetrics;

/// <summary>
/// Scrapboxのjsonファイル定義に対応するクラス
/// （ChatGPTによる自動生成）
/// </summary>
public class ScrapboxFormat 
{
    public class User
    {
        [JsonPropertyName("userId")] public string UserId { get; set; }

        [JsonPropertyName("created")] public long Created { get; set; }

        [JsonPropertyName("updated")] public long Updated { get; set; }
    }

    public class Line
    {
        [JsonPropertyName("text")] public string Text { get; set; }

        [JsonPropertyName("created")] public long Created { get; set; }

        [JsonPropertyName("updated")] public long Updated { get; set; }

        [JsonPropertyName("userId")] public string UserId { get; set; }
    }

    public class Page
    {
        [JsonPropertyName("title")] public string Title { get; set; }

        [JsonPropertyName("created")] public long Created { get; set; }

        [JsonPropertyName("updated")] public long Updated { get; set; }

        [JsonPropertyName("id")] public string Id { get; set; }

        [JsonPropertyName("lines")] public List<Line> Lines { get; set; }

        [JsonPropertyName("linksLc")] public List<string> LinksLc { get; set; }
    }

    [JsonPropertyName("name")] public string Name { get; set; }

    [JsonPropertyName("displayName")] public string DisplayName { get; set; }

    [JsonPropertyName("exported")] public long Exported { get; set; }

    [JsonPropertyName("users")] public List<User> Users { get; set; }

    [JsonPropertyName("pages")] public List<Page> Pages { get; set; }
}