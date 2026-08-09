import { useState } from 'react';
import type { CatchResponseDto, SpeciesResponseDto } from '../api';
import Card from './Card';

type Props = {
    catches: CatchResponseDto[];
    species: SpeciesResponseDto[];
};

function PersonalBestCard({ catches, species }: Props) {
    const caughtSpecies = species.filter((s) => catches.some((c) => c.speciesId === s.id));

    const [selectedId, setSelectedId] = useState(() => {
        const bass = caughtSpecies.find((s) => s.name === 'Largemouth Bass');
        return bass?.id ?? caughtSpecies[0]?.id ?? 0;
    });

    const weights = catches
        .filter((c) => c.speciesId === selectedId)
        .map((c) => c.weightLbs)
        .filter((w) => w !== undefined);

    const best = weights.length > 0 ? Math.max(...weights) : null;

    return (
        <Card title="Personal Best">
            <p className="mt-2 text-4xl font-bold">{best === null ? '—' : `${best.toFixed(1)} lb`}</p>

            <select
                value={selectedId}
                onChange={(e) => setSelectedId(Number(e.target.value))}
                className="mt-3 w-full rounded border border-emerald-700 bg-emerald-950 px-2 py-1 text-sm"
            >
                {caughtSpecies.length === 0 && <option value={0}>No catches yet</option>}
                {caughtSpecies.map((s) => (
                    <option key={s.id} value={s.id}>{s.name}</option>
                ))}
            </select>
        </Card>
    );
}

export default PersonalBestCard;
