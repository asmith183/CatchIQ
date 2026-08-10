import type { CatchResponseDto } from '../api';
import Card from './Card';

function TotalCatchesCard({ catches }: { catches: CatchResponseDto[] }) {
    return (
        <Card title="Total Catches">
            <p className="mt-2 text-5xl font-bold text-heading">{catches.length}</p>
        </Card>
    );
}

export default TotalCatchesCard;
