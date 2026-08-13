import { useEffect, useState } from 'react';
import { baitClient, speciesClient, spotClient } from '../api';
import type { BaitResponseDto, SpeciesResponseDto, SpotResponseDto } from '../api';

export type LogCatchData = {
    species: SpeciesResponseDto[];
    spots: SpotResponseDto[];
    baits: BaitResponseDto[];
};

export function useLogCatchData() {
    const [data, setData] = useState<LogCatchData | null>(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        Promise.all([speciesClient.getAll(), spotClient.getAll(), baitClient.getAll()])
            .then(([species, spots, baits]) => setData({ species, spots, baits }))
            .catch(() => setError('Could not load species, spots, and baits.'))
            .finally(() => setLoading(false));
    }, []);

    return { data, loading, error };
}
