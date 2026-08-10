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
            <p className="mt-2 text-5xl font-bold text-heading">{best === null ? '—' : `${best.toFixed(1)} lb`}</p>

            <select
                value={selectedId}
                onChange={(e) => setSelectedId(Number(e.target.value))}
                className="mt-4 w-full rounded border border-line bg-sunken px-3 py-2 text-base text-heading [&>option]:bg-sunken [&>option]:text-heading"
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
