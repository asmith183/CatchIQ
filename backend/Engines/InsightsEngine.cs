using System.Text.Json;
using System.Text.Json.Serialization;
using Anthropic;
using Anthropic.Exceptions;
using Anthropic.Models.Messages;
using CatchIQ.API.Engines.Interfaces;
using CatchIQ.API.Models.DTOs;

namespace CatchIQ.API.Engines;
public class InsightsEngine(AnthropicClient client, ILogger<InsightsEngine> logger) : IInsightsEngine
{
    private readonly AnthropicClient _client = client;
    private readonly ILogger<InsightsEngine> _logger = logger;

    private const string SystemPrompt = """
        You are a fishing analytics expert writing personalized insights for an angler
        based on their catch log. The user message contains a JSON array of their catches,
        each with the time it was caught, species, bait, spot, size, and the weather
        conditions at the time.

        Look for patterns that cross multiple factors rather than isolated stats. For
        example, a bait insight is stronger when it also notes where or in what conditions
        that bait shines: "Jigs are your most productive bait overall, particularly at
        Branched Oak. Soft plastics outperform in warmer afternoon conditions."

        Fill each field as follows:
        - bestTimeOfDay: when this angler catches the most (and biggest) fish, 2-3 sentences.
        - bestBait: their most productive bait and the context it works best in, 2-3 sentences.
        - bestConditions: the weather, sky, and pressure conditions their catches cluster
          around, 2-3 sentences.
        - summary: one paragraph (4-6 sentences) tying together their overall tendencies
          across species, spots, baits, timing, and conditions.

        Write directly to the angler ("you", "your"). Ground every claim in the data
        provided - cite spot names, bait names, and counts where they help - and never
        invent catches or conditions that are not in the log.
        """;

    public async Task<InsightsResponseDto?> GenerateAsync(string catchDataJson)
    {
        try
        {
            var response = await _client.Messages.Create(new MessageCreateParams
            {
                Model = "claude-sonnet-5",
                MaxTokens = 16000,
                System = SystemPrompt,
                OutputConfig = new OutputConfig { Format = new JsonOutputFormat { Schema = BuildSchema() } },
                Messages = [new() { Role = Role.User, Content = catchDataJson }]
            });

            if (response.StopReason == "refusal")
            {
                _logger.LogWarning("Insight generation was refused by the model.");
                return null;
            }

            var text = response.Content
                .Select(block => block.Value)
                .OfType<TextBlock>()
                .FirstOrDefault()?.Text;

            if (text == null)
            {
                _logger.LogWarning("Insight generation returned no text content (stop reason: {StopReason}).", response.StopReason);
                return null;
            }

            var payload = JsonSerializer.Deserialize<InsightsPayload>(text);
            if (payload == null)
                return null;

            return new InsightsResponseDto(
                BestTimeOfDay: payload.BestTimeOfDay,
                BestBait: payload.BestBait,
                BestConditions: payload.BestConditions,
                Summary: payload.Summary,
                GeneratedAt: DateTimeOffset.UtcNow
            );
        }
        catch (Exception ex) when (ex is AnthropicApiException or JsonException)
        {
            _logger.LogWarning(ex, "Insight generation failed.");
            return null;
        }
    }

    #region Helper Methods
    private static Dictionary<string, JsonElement> BuildSchema()
    {
        return new Dictionary<string, JsonElement>
        {
            ["type"] = JsonSerializer.SerializeToElement("object"),
            ["properties"] = JsonSerializer.SerializeToElement(new
            {
                bestTimeOfDay = new { type = "string" },
                bestBait = new { type = "string" },
                bestConditions = new { type = "string" },
                summary = new { type = "string" }
            }),
            ["required"] = JsonSerializer.SerializeToElement(
                new[] { "bestTimeOfDay", "bestBait", "bestConditions", "summary" }),
            ["additionalProperties"] = JsonSerializer.SerializeToElement(false)
        };
    }
    #endregion

    #region Response Models
    private sealed class InsightsPayload
    {
        [JsonPropertyName("bestTimeOfDay")]
        public string BestTimeOfDay { get; set; } = null!;

        [JsonPropertyName("bestBait")]
        public string BestBait { get; set; } = null!;

        [JsonPropertyName("bestConditions")]
        public string BestConditions { get; set; } = null!;

        [JsonPropertyName("summary")]
        public string Summary { get; set; } = null!;
    }
    #endregion
}
