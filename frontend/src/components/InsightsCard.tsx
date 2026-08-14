import { Link } from 'react-router-dom';
import { Sparkles } from 'lucide-react';
import Card from './Card';

function InsightsCard() {
    return (
        <Card title="AI Insights">
            <div className="mt-2 flex flex-1 flex-col items-center justify-center gap-3 py-6 text-center text-base text-muted">
                <Sparkles size={40} className="text-icon" />
                <p>Claude looks for patterns in your timing, bait, and conditions.</p>
                <Link
                    to="/insights"
                    className="mt-1 rounded bg-primary px-3 py-1.5 text-sm hover:brightness-110"
                >
                    View insights
                </Link>
            </div>
        </Card>
    );
}

export default InsightsCard;
