import { Link } from 'react-router-dom';
import type { CatchResponseDto, SpeciesResponseDto } from '../api';
import Card from './Card';

type Props = {
    catches: CatchResponseDto[];
    species: SpeciesResponseDto[];
};

function RecentCatchesCard({ catches, species }: Props) {
    const recent = [...catches]
        .sort((a, b) => b.caughtAt.getTime() - a.caughtAt.getTime())
        .slice(0, 5);

    return (
        <Card title="Recent Catches">
            {recent.length === 0 ? (
                <p className="mt-2 text-base text-muted">Nothing logged yet.</p>
            ) : (
                <ul className="mt-2 divide-y divide-line">
                    {recent.map((c) => (
                        <li key={c.id} className="flex items-baseline justify-between gap-2 py-3 text-base">
                            <span className="truncate">
                                {species.find((s) => s.id === c.speciesId)?.name ?? 'Unknown'}
                            </span>
                            <span className="shrink-0 text-muted">
                                {c.weightLbs === undefined ? '—' : `${c.weightLbs.toFixed(1)} lb`}
                                {' · '}
                                {c.caughtAt.toLocaleDateString()}
                            </span>
                        </li>
                    ))}
                </ul>
            )}

            <Link to="/history" className="mt-auto inline-block pt-4 text-base text-icon">View all</Link>
        </Card>
    );
}

export default RecentCatchesCard;
