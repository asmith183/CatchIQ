import { useDashboardData } from '../hooks/useDashboardData';
import TotalCatchesCard from '../components/TotalCatchesCard';
import PersonalBestCard from '../components/PersonalBestCard';
import BestSpotCard from '../components/BestSpotCard';
import RecentCatchesCard from '../components/RecentCatchesCard';
import InsightsCard from '../components/InsightsCard';
import SpotsCard from '../components/SpotsCard';

function Dashboard() {
    const { data, loading, error } = useDashboardData();

    if (loading) {
        return <p className="py-10 text-muted">Loading your dashboard…</p>;
    }

    if (error !== null || data === null) {
        return <p className="py-10 text-danger">{error ?? 'Could not load your dashboard.'}</p>;
    }

    return (
        <div className="flex flex-1 flex-col py-8">
            <h1 className="text-4xl font-bold text-heading">Dashboard</h1>

            <div className="mt-6 grid gap-6 md:grid-cols-3">
                <TotalCatchesCard catches={data.catches} />
                <PersonalBestCard catches={data.catches} species={data.species} />
                <BestSpotCard catches={data.catches} spots={data.spots} />
            </div>

            <div className="mt-6 grid gap-6 md:max-h-[498px] md:flex-1 md:grid-cols-3">
                <RecentCatchesCard catches={data.catches} species={data.species} />
                <InsightsCard />
                <SpotsCard spots={data.spots} />
            </div>
        </div>
    );
}

export default Dashboard;
