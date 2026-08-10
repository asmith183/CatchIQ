import { NavLink, Link, Outlet } from 'react-router-dom';
import { FishSymbol, Plus, User } from 'lucide-react';
import { useAuth } from '../auth/AuthContext';

function navLinkClass({ isActive }: { isActive: boolean }) {
    const base = 'flex items-center px-3 py-2';
    return isActive ? `${base} text-icon` : `${base} text-body`;
}

function NavLinks() {
    return (
        <>
            <NavLink to="/" end className={navLinkClass}>Home</NavLink>
            <NavLink to="/history" className={navLinkClass}>History</NavLink>
            <NavLink to="/spots" className={navLinkClass}>Spots</NavLink>
            <NavLink to="/map" className={navLinkClass}>Map</NavLink>
            <NavLink to="/insights" className={navLinkClass}>Insights</NavLink>
        </>
    );
}

function AppLayout() {
    const { logout } = useAuth();

    return (
        <div className="flex min-h-screen flex-col">
            <nav className="sticky top-0 z-20 border-b border-line bg-panel">
                <div className="mx-auto flex max-w-6xl items-center justify-between px-4 py-3">
                    <Link to="/" className="flex items-center gap-2 text-xl font-bold">
                        <FishSymbol />
                        <span>Catch<span className="text-icon">IQ</span></span>
                    </Link>

                    <div className="hidden gap-6 md:flex">
                        <NavLinks />
                    </div>

                    <div className="flex items-center gap-3">
                        <Link
                            to="/logCatch"
                            className="flex items-center gap-1 rounded bg-primary px-3 py-1.5 text-sm hover:brightness-110"
                        >
                            <Plus size={16} />
                            Log Catch
                        </Link>

                        <button onClick={logout} title="Log out" className="rounded-full border border-line p-2">
                            <User size={20} />
                        </button>
                    </div>
                </div>
            </nav>

            <main className="mx-auto flex w-full max-w-6xl flex-1 flex-col px-4 pb-20 md:pb-0">
                <Outlet />
            </main>

            <div className="fixed inset-x-0 bottom-0 z-20 flex justify-around border-t border-line bg-panel pb-[env(safe-area-inset-bottom)] text-xs md:hidden">
                <NavLinks />
            </div>
        </div>
    );
}

export default AppLayout;
