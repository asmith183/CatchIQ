import { useEffect, useState } from 'react';
import { catchClient, speciesClient, spotClient } from '../api';
import type { CatchResponseDto, SpeciesResponseDto, SpotResponseDto } from '../api';

export type DashboardData = {
    catches: CatchResponseDto[];
    species: SpeciesResponseDto[];
    spots: SpotResponseDto[];
};

export function useDashboardData() {
    const [data, setData] = useState<DashboardData | null>(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        Promise.all([catchClient.getAll(), speciesClient.getAll(), spotClient.getAll()])
            .then(([catches, species, spots]) => setData({ catches, species, spots }))
            .catch(() => setError('Could not load your dashboard.'))
            .finally(() => setLoading(false));
    }, []);

    return { data, loading, error };
}
