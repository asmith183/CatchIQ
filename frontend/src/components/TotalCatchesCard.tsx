import { FishSymbol } from 'lucide-react';
import type { CatchResponseDto } from '../api';
import Card from './Card';

function TotalCatchesCard({ catches }: { catches: CatchResponseDto[] }) {
    return (
        <Card title="Total Catches" icon={FishSymbol}>
            <p className="mt-2 text-2xl font-bold text-heading">{catches.length}</p>
            <p className="mt-2 text-sm text-muted">All time</p>
        </Card>
    );
}

export default TotalCatchesCard;
