import { useEffect, useState } from 'react';
import { insightsClient, ApiException } from '../api';
import type { InsightsResponseDto } from '../api';

/// Force concurrent callers to share the same in-flight request, 
/// so we don't spam the server with multiple requests for the same data.
let inFlight: Promise<InsightsResponseDto> | null = null;

function fetchInsights() {
    inFlight ??= insightsClient.get().finally(() => { inFlight = null; });
    return inFlight;
}

export function useInsightsData() {
    const [data, setData] = useState<InsightsResponseDto | null>(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [needsMoreCatches, setNeedsMoreCatches] = useState(false);

    useEffect(() => {
        fetchInsights()
            .then(setData)
            .catch((err: unknown) => {
                // 400 => not enough data to generate insights, not an error
                if (ApiException.isApiException(err) && err.status === 400) {
                    setNeedsMoreCatches(true);
                } else {
                    setError('Could not generate your insights. Please try again later.');
                }
            })
            .finally(() => setLoading(false));
    }, []);

    return { data, loading, error, needsMoreCatches };
}
