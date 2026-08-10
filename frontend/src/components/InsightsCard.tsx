import { Sparkles } from 'lucide-react';
import Card from './Card';

function InsightsCard() {
    return (
        <Card title="AI Insights">
            <div className="mt-2 flex flex-1 flex-col items-center justify-center gap-3 py-6 text-center text-base text-muted">
                <Sparkles size={40} className="text-icon" />
                <p>Catch patterns and bait suggestions land here once the analytics API is built.</p>
            </div>
        </Card>
    );
}

export default InsightsCard;
