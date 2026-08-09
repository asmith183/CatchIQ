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
        return <p className="py-10 text-emerald-300">Loading your dashboard…</p>;
    }

    if (error !== null || data === null) {
        return <p className="py-10 text-red-300">{error ?? 'Could not load your dashboard.'}</p>;
    }

    return (
        <div className="py-6">
            <h1 className="text-3xl font-bold">Dashboard</h1>

            <div className="mt-6 grid gap-4 md:grid-cols-3">
                <TotalCatchesCard catches={data.catches} />
                <PersonalBestCard catches={data.catches} species={data.species} />
                <BestSpotCard catches={data.catches} spots={data.spots} />
            </div>

            <div className="mt-4 grid gap-4 md:grid-cols-3">
                <RecentCatchesCard catches={data.catches} species={data.species} />
                <InsightsCard />
                <SpotsCard spots={data.spots} />
            </div>
        </div>
    );
}

export default Dashboard;
