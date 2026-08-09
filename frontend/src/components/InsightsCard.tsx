import { Sparkles } from 'lucide-react';
import Card from './Card';

function InsightsCard() {
    return (
        <Card title="AI Insights">
            <div className="mt-2 flex flex-col items-center gap-2 py-6 text-center text-sm text-emerald-300">
                <Sparkles size={28} className="text-teal-400" />
                <p>Catch patterns and bait suggestions land here once the analytics API is built.</p>
            </div>
        </Card>
    );
}

export default InsightsCard;
