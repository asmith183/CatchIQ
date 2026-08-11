import { useMemo, useState } from 'react';
import { Search } from 'lucide-react';
import { useHistoryData } from '../hooks/useHistoryData';
import CatchCard from '../components/CatchCard';

type Row = {
    id: number;
    speciesName: string;
    spotName: string | undefined;
    baitName: string | undefined;
    weightLbs: number | undefined;
    caughtAt: Date;
};

type FilterSelectProps = {
    label: string;
    value: string;
    options: string[];
    onChange: (value: string) => void;
};

function FilterSelect({ label, value, options, onChange }: FilterSelectProps) {
    return (
        <select
            value={value}
            onChange={(e) => onChange(e.target.value)}
            className="rounded border border-line bg-sunken px-3 py-2 text-body focus:border-primary focus:outline-none"
        >
            <option value="">All {label}</option>
            {options.map((option) => (
                <option key={option} value={option}>{option}</option>
            ))}
        </select>
    );
}

function sortedNames(values: (string | undefined)[]): string[] {
    return [...new Set(values.filter((v) => v !== undefined))].sort();
}

function History() {
    const { data, loading, error } = useHistoryData();
    const [search, setSearch] = useState('');
    const [species, setSpecies] = useState('');
    const [bait, setBait] = useState('');
    const [spot, setSpot] = useState('');

    const rows: Row[] = useMemo(() => {
        if (data === null) {
            return [];
        }

        const speciesById = new Map(data.species.map((s) => [s.id, s.name]));
        const spotById = new Map(data.spots.map((s) => [s.id, s.name]));
        const baitById = new Map(data.baits.map((b) => [b.id, b.name]));

        return data.catches
            .map((c) => ({
                id: c.id,
                speciesName: speciesById.get(c.speciesId) ?? 'Unknown',
                spotName: c.spotId === undefined ? undefined : spotById.get(c.spotId),
                baitName: c.baitId === undefined ? undefined : baitById.get(c.baitId),
                weightLbs: c.weightLbs,
                caughtAt: c.caughtAt,
            }))
            .sort((a, b) => b.caughtAt.getTime() - a.caughtAt.getTime());
    }, [data]);

    const query = search.trim().toLowerCase();

    const visible = rows.filter((r) => {
        const text = `${r.speciesName} ${r.spotName ?? ''} ${r.baitName ?? ''}`.toLowerCase();

        return text.includes(query)
            && (species === '' || r.speciesName === species)
            && (bait === '' || r.baitName === bait)
            && (spot === '' || r.spotName === spot);
    });

    if (loading) {
        return <p className="py-10 text-muted">Loading your catches…</p>;
    }

    if (error !== null) {
        return <p className="py-10 text-danger">{error}</p>;
    }

    return (
        <div className="flex flex-1 flex-col py-8">
            <h1 className="text-4xl font-bold text-heading">History</h1>

            <div className="mt-6 flex flex-col gap-3 md:flex-row">
                <div className="relative md:flex-1">
                    <Search size={16} className="absolute left-3 top-1/2 -translate-y-1/2 text-muted" />
                    <input
                        type="search"
                        value={search}
                        onChange={(e) => setSearch(e.target.value)}
                        placeholder="Search species, spot, or bait"
                        className="w-full rounded border border-line bg-sunken py-2 pl-9 pr-3 text-heading focus:border-primary focus:outline-none"
                    />
                </div>

                <div className="grid grid-cols-2 gap-3 md:flex">
                    <FilterSelect
                        label="species"
                        value={species}
                        options={sortedNames(rows.map((r) => r.speciesName))}
                        onChange={setSpecies}
                    />
                    <FilterSelect
                        label="baits"
                        value={bait}
                        options={sortedNames(rows.map((r) => r.baitName))}
                        onChange={setBait}
                    />
                    <FilterSelect
                        label="spots"
                        value={spot}
                        options={sortedNames(rows.map((r) => r.spotName))}
                        onChange={setSpot}
                    />
                </div>
            </div>

            <p className="mt-4 text-sm text-muted">
                {visible.length} of {rows.length} {rows.length === 1 ? 'catch' : 'catches'}
            </p>

            {visible.length === 0 ? (
                <p className="mt-6 text-base text-muted">
                    {rows.length === 0 ? 'Nothing logged yet.' : 'No catches match those filters.'}
                </p>
            ) : (
                <div className="mt-4 grid gap-4 sm:grid-cols-2 lg:grid-cols-3">
                    {visible.map((r) => (
                        <CatchCard
                            key={r.id}
                            speciesName={r.speciesName}
                            spotName={r.spotName}
                            baitName={r.baitName}
                            weightLbs={r.weightLbs}
                            caughtAt={r.caughtAt}
                        />
                    ))}
                </div>
            )}
        </div>
    );
}

export default History;
