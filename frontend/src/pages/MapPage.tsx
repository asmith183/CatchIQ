import { useEffect, useState } from 'react';
import { MapContainer, TileLayer, CircleMarker, Popup, useMap } from 'react-leaflet';
import type { LatLngTuple } from 'leaflet';
import 'leaflet/dist/leaflet.css';
import { useMapData } from '../hooks/useMapData';

function FitToMarkers({ points }: { points: LatLngTuple[] }) {
    const map = useMap();

    useEffect(() => {
        if (points.length > 0) {
            map.fitBounds(points, { padding: [40, 40], maxZoom: 13 });
        }
    }, [map, points]);

    return null;
}

function MapPage() {
    const { data, loading, error } = useMapData();
    const [showCatches, setShowCatches] = useState(true);

    if (loading) {
        return <p className="py-10 text-muted">Loading your map…</p>;
    }

    if (error !== null || data === null) {
        return <p className="py-10 text-danger">{error}</p>;
    }

    const { catches, species, spots } = data;

    const points: LatLngTuple[] = [
        ...spots.map((s): LatLngTuple => [s.latitude, s.longitude]),
        ...catches.map((c): LatLngTuple => [c.latitude, c.longitude]),
    ];

    function speciesName(id: number) {
        return species.find((s) => s.id === id)?.name ?? 'Unknown species';
    }

    return (
        <div className="py-8">
            <div className="flex flex-wrap items-center justify-between gap-3">
                <h1 className="text-4xl font-bold text-heading">Map</h1>
                <label className="flex items-center gap-2 text-body">
                    <input
                        type="checkbox"
                        checked={showCatches}
                        onChange={(e) => setShowCatches(e.target.checked)}
                    />
                    Show catches
                </label>
            </div>

            <div className="relative z-0 mt-6 h-[70vh] overflow-hidden rounded border border-line bg-sunken">
                <MapContainer center={[39.5, -98.35]} zoom={3} className="h-full w-full">
                    <TileLayer
                        url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
                        attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
                    />
                    <FitToMarkers points={points} />

                    {showCatches && catches.map((c) => (
                        <CircleMarker
                            key={`catch-${c.id}`}
                            center={[c.latitude, c.longitude]}
                            radius={4}
                            pathOptions={{ color: '#f0a050', fillColor: '#f0a050', fillOpacity: 0.8 }}
                        >
                            <Popup>
                                <p className="font-medium">{speciesName(c.speciesId)}</p>
                                <p>
                                    {c.weightLbs !== undefined ? `${c.weightLbs} lbs · ` : ''}
                                    {c.caughtAt.toLocaleDateString()}
                                </p>
                            </Popup>
                        </CircleMarker>
                    ))}

                    {spots.map((s) => {
                        const spotCatches = catches.filter((c) => c.spotId === s.id);
                        const best = spotCatches
                            .filter((c) => c.weightLbs !== undefined)
                            .sort((a, b) => b.weightLbs! - a.weightLbs!)[0];

                        return (
                            <CircleMarker
                                key={`spot-${s.id}`}
                                center={[s.latitude, s.longitude]}
                                radius={9}
                                pathOptions={{ color: '#7fc49a', fillColor: '#7fc49a', fillOpacity: 0.7 }}
                            >
                                <Popup>
                                    <p className="font-medium">{s.name}</p>
                                    {s.notes && <p>{s.notes}</p>}
                                    <p>
                                        {spotCatches.length} {spotCatches.length === 1 ? 'catch' : 'catches'}
                                    </p>
                                    {best && (
                                        <p>Best: {speciesName(best.speciesId)}, {best.weightLbs} lbs</p>
                                    )}
                                </Popup>
                            </CircleMarker>
                        );
                    })}
                </MapContainer>
            </div>

            <p className="mt-3 text-sm text-muted">
                Green markers are spots, amber markers are catches.
            </p>
        </div>
    );
}

export default MapPage;
