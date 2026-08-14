using System.Text.Json;
using System.Text.Json.Serialization;
using CatchIQ.API.Accessors.Interfaces;
using CatchIQ.API.Engines.Interfaces;
using CatchIQ.API.Exceptions;
using CatchIQ.API.Managers.Interfaces;
using CatchIQ.API.Models.DTOs;
using Microsoft.Extensions.Caching.Memory;

namespace CatchIQ.API.Managers;
public class InsightsManager(ICatchAccessor catchAccessor, ISpeciesAccessor speciesAccessor, ISpotAccessor spotAccessor, IBaitAccessor baitAccessor, IInsightsEngine insightsEngine, IMemoryCache cache) : IInsightsManager
{
    private readonly ICatchAccessor _catchAccessor = catchAccessor;
    private readonly ISpeciesAccessor _speciesAccessor = speciesAccessor;
    private readonly ISpotAccessor _spotAccessor = spotAccessor;
    private readonly IBaitAccessor _baitAccessor = baitAccessor;
    private readonly IInsightsEngine _insightsEngine = insightsEngine;
    private readonly IMemoryCache _cache = cache;

    private const int MinimumCatches = 3;
    private static readonly TimeSpan CacheDuration = TimeSpan.FromHours(1);

    // Compact serialization for the data sent to the model: skip nulls and
    // write enums as names so the log reads naturally ("PartlyCloudy", "NW").
    private static readonly JsonSerializerOptions CatchDataOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        Converters = { new JsonStringEnumConverter() }
    };

    public async Task<InsightsResponseDto> GetForUserAsync(int userId)
    {
        var cacheKey = $"insights:{userId}";
        if (_cache.TryGetValue(cacheKey, out InsightsResponseDto? cached) && cached != null)
            return cached;

        var catches = await _catchAccessor.GetAllByUserAsync(userId);
        if (catches.Count < MinimumCatches)
            throw new InsufficientCatchDataException(MinimumCatches);

        var species = (await _speciesAccessor.GetAllAsync()).ToDictionary(s => s.Id);
        var spots = (await _spotAccessor.GetAllByUserAsync(userId)).ToDictionary(s => s.Id);
        var baits = (await _baitAccessor.GetAllByUserAsync(userId)).ToDictionary(b => b.Id);

        var catchData = catches.Select(c =>
        {
            var bait = c.BaitId == null ? null : baits.GetValueOrDefault(c.BaitId.Value);
            return new
            {
                caughtAt = c.CaughtAt,
                species = species.GetValueOrDefault(c.SpeciesId)?.Name,
                spot = c.SpotId == null ? null : spots.GetValueOrDefault(c.SpotId.Value)?.Name,
                bait = bait?.Name,
                baitType = bait?.Type,
                weightLbs = c.WeightLbs,
                lengthInches = c.LengthInches,
                airTempF = c.AirTempF,
                windSpeedMph = c.WindSpeedMph,
                windDirection = c.WindDirection,
                pressureMb = c.PressureMb,
                skyCondition = c.SkyCondition,
                waterTempF = c.WaterTempF
            };
        }).ToList();

        var result = await _insightsEngine.GenerateAsync(JsonSerializer.Serialize(catchData, CatchDataOptions));
        if (result == null)
            throw new InsightsGenerationFailedException();

        _cache.Set(cacheKey, result, CacheDuration);
        return result;
    }
}
