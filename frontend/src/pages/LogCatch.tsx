import { useEffect, useRef, useState } from 'react';
import type { FormEvent } from 'react';
import { useNavigate } from 'react-router-dom';
import { catchClient } from '../api';
import type { WeightMethod } from '../api';
import { useLogCatchData } from '../hooks/useLogCatchData';
import TextField from '../components/TextField';

type CoordsSource = 'pending' | 'device' | 'manual' | 'unavailable';

type SelectFieldProps = {
    id: string;
    label: string;
    value: string;
    onChange: (value: string) => void;
    options: { value: string; label: string }[];
    required?: boolean;
    placeholder?: string;
};

function SelectField({ id, label, value, onChange, options, required = false, placeholder }: SelectFieldProps) {
    return (
        <div>
            <label htmlFor={id} className="block text-sm font-medium text-heading">
                {label}
            </label>
            <select
                id={id}
                value={value}
                required={required}
                onChange={(e) => onChange(e.target.value)}
                className="mt-1 w-full rounded border border-line bg-sunken px-3 py-2 text-heading focus:border-primary focus:outline-none"
            >
                <option value="">{placeholder ?? `Select ${label.toLowerCase()}`}</option>
                {options.map((option) => (
                    <option key={option.value} value={option.value}>{option.label}</option>
                ))}
            </select>
        </div>
    );
}

// datetime-local inputs want "YYYY-MM-DDTHH:mm" in local time.
function localNow(): string {
    const now = new Date();
    now.setMinutes(now.getMinutes() - now.getTimezoneOffset());

    return now.toISOString().slice(0, 16);
}

const locationStatusText: Record<CoordsSource, string> = {
    pending: 'Getting your location…',
    device: "Using your device's location.",
    manual: 'Using manually entered coordinates.',
    unavailable: 'Location unavailable — enter coordinates manually.',
};

function LogCatch() {
    const navigate = useNavigate();
    const { data, loading, error } = useLogCatchData();

    const [speciesId, setSpeciesId] = useState('');
    const [spotId, setSpotId] = useState('');
    const [baitId, setBaitId] = useState('');
    const [caughtAt, setCaughtAt] = useState(localNow);
    const [weightLbs, setWeightLbs] = useState('');
    const [weightMethod, setWeightMethod] = useState<WeightMethod>('Estimated');
    const [lengthInches, setLengthInches] = useState('');
    const [waterTempF, setWaterTempF] = useState('');
    const [latitude, setLatitude] = useState('');
    const [longitude, setLongitude] = useState('');
    const [notes, setNotes] = useState('');

    const [coordsSource, setCoordsSource] = useState<CoordsSource>('pending');
    const [saving, setSaving] = useState(false);
    const [submitError, setSubmitError] = useState<string | null>(null);

    // Geolocation resolves asynchronously; if the user has already typed
    // coordinates by then, the device fix must not clobber them.
    const coordsTouchedRef = useRef(false);

    useEffect(() => {
        if (!('geolocation' in navigator)) {
            setCoordsSource('unavailable');
            return;
        }

        navigator.geolocation.getCurrentPosition(
            (position) => {
                if (coordsTouchedRef.current) {
                    return;
                }

                setLatitude(position.coords.latitude.toFixed(5));
                setLongitude(position.coords.longitude.toFixed(5));
                setCoordsSource('device');
            },
            () => {
                if (!coordsTouchedRef.current) {
                    setCoordsSource('unavailable');
                }
            }
        );
    }, []);

    function handleLatitudeChange(value: string) {
        coordsTouchedRef.current = true;
        setLatitude(value);
        setCoordsSource('manual');
    }

    function handleLongitudeChange(value: string) {
        coordsTouchedRef.current = true;
        setLongitude(value);
        setCoordsSource('manual');
    }

    async function handleSubmit(e: FormEvent) {
        e.preventDefault();

        if (latitude === '' || longitude === '') {
            setSubmitError('Location is required — enter coordinates.');
            return;
        }

        setSaving(true);
        setSubmitError(null);

        try {
            await catchClient.create({
                speciesId: Number(speciesId),
                spotId: spotId === '' ? undefined : Number(spotId),
                baitId: baitId === '' ? undefined : Number(baitId),
                latitude: Number(latitude),
                longitude: Number(longitude),
                caughtAt: new Date(caughtAt),
                weightLbs: weightLbs === '' ? undefined : Number(weightLbs),
                weightMethod,
                lengthInches: lengthInches === '' ? undefined : Number(lengthInches),
                waterTempF: waterTempF === '' ? undefined : Number(waterTempF),
                notes: notes.trim() === '' ? undefined : notes.trim(),
            });
            navigate('/history');
        } catch {
            setSubmitError('Could not save your catch. Please try again.');
            setSaving(false);
        }
    }

    if (loading) {
        return <p className="py-10 text-muted">Loading…</p>;
    }

    if (error !== null || data === null) {
        return <p className="py-10 text-danger">{error}</p>;
    }

    const byName = <T extends { name: string }>(a: T, b: T) => a.name.localeCompare(b.name);

    return (
        <div className="flex flex-1 flex-col py-8">
            <h1 className="text-4xl font-bold text-heading">Log Catch</h1>

            <form onSubmit={handleSubmit} className="mt-6 max-w-2xl rounded-lg border border-line bg-panel p-6">
                <div className="grid gap-4 sm:grid-cols-2">
                    <SelectField
                        id="species"
                        label="Species"
                        value={speciesId}
                        onChange={setSpeciesId}
                        required
                        options={[...data.species].sort(byName).map((s) => ({ value: String(s.id), label: s.name }))}
                    />
                    <TextField
                        id="caughtAt"
                        label="Caught at"
                        type="datetime-local"
                        value={caughtAt}
                        onChange={setCaughtAt}
                        required
                    />
                    <SelectField
                        id="spot"
                        label="Spot"
                        value={spotId}
                        onChange={setSpotId}
                        placeholder="No spot"
                        options={[...data.spots].sort(byName).map((s) => ({ value: String(s.id), label: s.name }))}
                    />
                    <SelectField
                        id="bait"
                        label="Bait"
                        value={baitId}
                        onChange={setBaitId}
                        placeholder="No bait"
                        options={[...data.baits].sort(byName).map((b) => ({ value: String(b.id), label: b.name }))}
                    />
                    <TextField
                        id="weightLbs"
                        label="Weight (lbs)"
                        type="number"
                        value={weightLbs}
                        onChange={setWeightLbs}
                    />
                    <SelectField
                        id="weightMethod"
                        label="Weight method"
                        value={weightMethod}
                        onChange={(value) => setWeightMethod(value as WeightMethod)}
                        required
                        options={[
                            { value: 'Exact', label: 'Exact' },
                            { value: 'Estimated', label: 'Estimated' },
                            { value: 'NoWeight', label: 'No weight' },
                        ]}
                    />
                    <TextField
                        id="lengthInches"
                        label="Length (inches)"
                        type="number"
                        value={lengthInches}
                        onChange={setLengthInches}
                    />
                    <TextField
                        id="waterTempF"
                        label="Water temp (°F)"
                        type="number"
                        value={waterTempF}
                        onChange={setWaterTempF}
                    />
                </div>

                <div className="mt-6">
                    <h2 className="text-sm font-medium text-heading">Location</h2>
                    <p className="mt-1 text-sm text-muted">{locationStatusText[coordsSource]}</p>
                    <div className="mt-2 grid gap-4 sm:grid-cols-2">
                        <TextField
                            id="latitude"
                            label="Latitude"
                            type="number"
                            value={latitude}
                            onChange={handleLatitudeChange}
                            required
                        />
                        <TextField
                            id="longitude"
                            label="Longitude"
                            type="number"
                            value={longitude}
                            onChange={handleLongitudeChange}
                            required
                        />
                    </div>
                    <p className="mt-2 text-sm text-muted">
                        Weather conditions are looked up automatically from the location and time.
                    </p>
                </div>

                <div className="mt-6">
                    <label htmlFor="notes" className="block text-sm font-medium text-heading">
                        Notes
                    </label>
                    <textarea
                        id="notes"
                        value={notes}
                        onChange={(e) => setNotes(e.target.value)}
                        rows={3}
                        className="mt-1 w-full rounded border border-line bg-sunken px-3 py-2 text-heading focus:border-primary focus:outline-none"
                    />
                </div>

                {submitError !== null && (
                    <p className="mt-4 text-sm text-danger">{submitError}</p>
                )}

                <button
                    type="submit"
                    disabled={saving}
                    className="mt-6 w-full rounded bg-primary py-2 text-heading hover:brightness-110 disabled:opacity-50"
                >
                    {saving ? 'Saving…' : 'Log catch'}
                </button>
            </form>
        </div>
    );
}

export default LogCatch;
