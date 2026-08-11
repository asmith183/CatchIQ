import { useEffect, useState } from 'react';
import { baitClient, catchClient, speciesClient, spotClient } from '../api';
import type { BaitResponseDto, CatchResponseDto, SpeciesResponseDto, SpotResponseDto } from '../api';

export type HistoryData = {
    catches: CatchResponseDto[];
    species: SpeciesResponseDto[];
    spots: SpotResponseDto[];
    baits: BaitResponseDto[];
};

export function useHistoryData() {
    const [data, setData] = useState<HistoryData | null>(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        Promise.all([catchClient.getAll(), speciesClient.getAll(), spotClient.getAll(), baitClient.getAll()])
            .then(([catches, species, spots, baits]) => setData({ catches, species, spots, baits }))
            .catch(() => setError('Could not load your catch history.'))
            .finally(() => setLoading(false));
    }, []);

    return { data, loading, error };
}
