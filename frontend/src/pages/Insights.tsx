import { Link } from 'react-router-dom';
import { Clock, Worm, CloudSun, Sparkles } from 'lucide-react';
import { useInsightsData } from '../hooks/useInsightsData';
import Card from '../components/Card';

function SkeletonLines() {
    return (
        <div className="mt-4 animate-pulse space-y-2">
            <div className="h-4 rounded bg-line" />
            <div className="h-4 rounded bg-line" />
            <div className="h-4 w-2/3 rounded bg-line" />
        </div>
    );
}

function Insights() {
    const { data, loading, error, needsMoreCatches } = useInsightsData();

    if (loading) {
        return (
            <div className="flex flex-1 flex-col py-8">
                <h1 className="text-4xl font-bold text-heading">Insights</h1>
                <p className="mt-2 text-muted">Analyzing your catches — this can take a few seconds…</p>

                <div className="mt-6 grid gap-6 md:grid-cols-3">
                    <Card title="Best Time of Day" icon={Clock}><SkeletonLines /></Card>
                    <Card title="Best Bait" icon={Worm}><SkeletonLines /></Card>
                    <Card title="Best Conditions" icon={CloudSun}><SkeletonLines /></Card>
                </div>

                <div className="mt-6">
                    <Card title="Summary" icon={Sparkles}><SkeletonLines /></Card>
                </div>
            </div>
        );
    }

    if (needsMoreCatches) {
        return (
            <div className="flex flex-1 flex-col py-8">
                <h1 className="text-4xl font-bold text-heading">Insights</h1>

                <div className="mt-16 flex flex-col items-center gap-4 text-center">
                    <Sparkles size={48} className="text-icon" />
                    <p className="max-w-md text-lg text-muted">
                        Log at least 3 catches and Claude will start finding patterns
                        in your timing, bait, and conditions.
                    </p>
                    <Link
                        to="/logCatch"
                        className="mt-2 rounded bg-primary px-4 py-2 hover:brightness-110"
                    >
                        Log a catch
                    </Link>
                </div>
            </div>
        );
    }

    if (error !== null || data === null) {
        return <p className="py-10 text-danger">{error ?? 'Could not generate your insights.'}</p>;
    }

    return (
        <div className="flex flex-1 flex-col py-8">
            <h1 className="text-4xl font-bold text-heading">Insights</h1>

            <div className="mt-6 grid gap-6 md:grid-cols-3">
                <Card title="Best Time of Day" icon={Clock}>
                    <p className="mt-4 text-body">{data.bestTimeOfDay}</p>
                </Card>
                <Card title="Best Bait" icon={Worm}>
                    <p className="mt-4 text-body">{data.bestBait}</p>
                </Card>
                <Card title="Best Conditions" icon={CloudSun}>
                    <p className="mt-4 text-body">{data.bestConditions}</p>
                </Card>
            </div>

            <div className="mt-6">
                <Card title="Summary" icon={Sparkles}>
                    <p className="mt-4 text-body">{data.summary}</p>
                    <p className="mt-4 text-sm text-muted">
                        Generated {data.generatedAt.toLocaleString(undefined, {
                            month: 'short', day: 'numeric', hour: 'numeric', minute: '2-digit',
                        })}
                    </p>
                </Card>
            </div>
        </div>
    );
}

export default Insights;
