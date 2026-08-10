import { Link } from 'react-router-dom';
import { MapContainer, TileLayer, CircleMarker, Tooltip } from 'react-leaflet';
import type { LatLngExpression } from 'leaflet';
import 'leaflet/dist/leaflet.css';
import type { SpotResponseDto } from '../api';
import Card from './Card';

function SpotsCard({ spots }: { spots: SpotResponseDto[] }) {
    const center: LatLngExpression = spots.length > 0
        ? [spots[0].latitude, spots[0].longitude]
        : [39.5, -98.35];

    return (
        <Card title="Spots">
            <div className="relative z-0 mt-2 h-48 overflow-hidden rounded bg-sunken md:h-auto md:min-h-0 md:flex-1">
                <MapContainer center={center} zoom={spots.length > 0 ? 9 : 3} scrollWheelZoom={false} className="h-full w-full">
                    <TileLayer
                        url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
                        attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
                    />
                    {spots.map((s) => (
                        <CircleMarker
                            key={s.id}
                            center={[s.latitude, s.longitude]}
                            radius={6}
                            pathOptions={{ color: '#7fc49a', fillColor: '#7fc49a', fillOpacity: 0.7 }}
                        >
                            <Tooltip>{s.name}</Tooltip>
                        </CircleMarker>
                    ))}
                </MapContainer>
            </div>

            <p className="mt-4 text-base text-muted">
                {spots.length} {spots.length === 1 ? 'spot' : 'spots'} saved
                {' · '}
                <Link to="/spots" className="text-icon">Manage</Link>
            </p>
        </Card>
    );
}

export default SpotsCard;
