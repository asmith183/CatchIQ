import { Clock, MapPin } from 'lucide-react';

type Props = {
    speciesName: string;
    spotName: string | undefined;
    baitName: string | undefined;
    weightLbs: number | undefined;
    caughtAt: Date;
};

function CatchCard({ speciesName, spotName, baitName, weightLbs, caughtAt }: Props) {
    return (
        <article className="rounded-lg border border-line bg-panel p-4">
            <div className="flex items-baseline justify-between gap-3">
                <h2 className="truncate text-lg font-medium text-heading">{speciesName}</h2>
                <span className="shrink-0 text-lg font-medium text-icon">
                    {weightLbs === undefined ? '—' : `${weightLbs.toFixed(1)} lb`}
                </span>
            </div>

            <p className="mt-2 flex items-center gap-1.5 text-sm text-muted">
                <MapPin size={14} />
                <span className="truncate">{spotName ?? 'No spot'}</span>
            </p>

            <p className="mt-1 flex items-center gap-1.5 text-sm text-muted">
                <Clock size={14} />
                {caughtAt.toLocaleDateString(undefined, { month: 'short', day: 'numeric', year: 'numeric' })}
                {' · '}
                {caughtAt.toLocaleTimeString(undefined, { hour: 'numeric', minute: '2-digit' })}
            </p>

            {baitName !== undefined && (
                <span className="mt-3 inline-block rounded bg-tag-bait-bg px-2 py-0.5 text-xs text-tag-bait">
                    {baitName}
                </span>
            )}
        </article>
    );
}

export default CatchCard;
