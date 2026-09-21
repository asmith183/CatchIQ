import { useEffect, useState } from 'react';
import { catchClient, speciesClient, spotClient } from '../api';
import type { CatchResponseDto, SpeciesResponseDto, SpotResponseDto } from '../api';

export type MapData = {
    catches: CatchResponseDto[];
    species: SpeciesResponseDto[];
    spots: SpotResponseDto[];
};

export function useMapData() {
    const [data, setData] = useState<MapData | null>(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        Promise.all([catchClient.getAll(), speciesClient.getAll(), spotClient.getAll()])
            .then(([catches, species, spots]) => setData({ catches, species, spots }))
            .catch(() => setError('Could not load your map.'))
            .finally(() => setLoading(false));
    }, []);

    return { data, loading, error };
}
