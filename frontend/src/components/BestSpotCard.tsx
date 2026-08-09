import type { CatchResponseDto, SpotResponseDto } from '../api';
import Card from './Card';

type Props = {
    catches: CatchResponseDto[];
    spots: SpotResponseDto[];
};

function BestSpotCard({ catches, spots }: Props) {
    const counts = new Map<number, number>();
    for (const c of catches) {
        if (c.spotId !== undefined) {
            counts.set(c.spotId, (counts.get(c.spotId) ?? 0) + 1);
        }
    }

    let bestId = 0;
    let bestCount = 0;
    for (const [spotId, count] of counts) {
        if (count > bestCount) {
            bestId = spotId;
            bestCount = count;
        }
    }

    const best = spots.find((s) => s.id === bestId);

    return (
        <Card title="Best Spot">
            <p className="mt-2 truncate text-4xl font-bold">{best?.name ?? '—'}</p>
            <p className="mt-1 text-sm text-emerald-300">
                {best ? `${bestCount} ${bestCount === 1 ? 'catch' : 'catches'}` : 'No catches logged to a spot'}
            </p>
        </Card>
    );
}

export default BestSpotCard;
