import { useEffect, useState } from 'react';
import type { FormEvent } from 'react';
import { MapPin, Plus } from 'lucide-react';
import { ApiException, spotClient } from '../api';
import type { SpotResponseDto, WaterBodyType } from '../api';
import TextField from '../components/TextField';

function Spots() {
    const [spots, setSpots] = useState<SpotResponseDto[] | null>(null);
    const [loading, setLoading] = useState(true);
    const [loadError, setLoadError] = useState<string | null>(null);

    const [formOpen, setFormOpen] = useState(false);
    const [editingId, setEditingId] = useState<number | null>(null);
    const [name, setName] = useState('');
    const [latitude, setLatitude] = useState('');
    const [longitude, setLongitude] = useState('');
    const [waterBodyType, setWaterBodyType] = useState<WaterBodyType>('Lake');
    const [notes, setNotes] = useState('');
    const [saving, setSaving] = useState(false);
    const [submitError, setSubmitError] = useState<string | null>(null);

    const [confirmingId, setConfirmingId] = useState<number | null>(null);
    const [listError, setListError] = useState<string | null>(null);

    useEffect(() => {
        spotClient.getAll()
            .then(setSpots)
            .catch(() => setLoadError('Could not load your spots.'))
            .finally(() => setLoading(false));
    }, []);

    function openCreate() {
        setEditingId(null);
        setName('');
        setLatitude('');
        setLongitude('');
        setWaterBodyType('Lake');
        setNotes('');
        setSubmitError(null);
        setFormOpen(true);
    }

    function openEdit(spot: SpotResponseDto) {
        setEditingId(spot.id);
        setName(spot.name);
        setLatitude(String(spot.latitude));
        setLongitude(String(spot.longitude));
        setWaterBodyType(spot.waterBodyType);
        setNotes(spot.notes ?? '');
        setSubmitError(null);
        setFormOpen(true);
    }

    async function handleSubmit(e: FormEvent) {
        e.preventDefault();
        setSaving(true);
        setSubmitError(null);

        const body = {
            name: name.trim(),
            latitude: Number(latitude),
            longitude: Number(longitude),
            waterBodyType,
            notes: notes.trim() === '' ? undefined : notes.trim(),
        };

        try {
            if (editingId === null) {
                const created = await spotClient.create(body);
                setSpots((prev) => [...(prev ?? []), created]);
            } else {
                const updated = await spotClient.update(editingId, body);
                setSpots((prev) => (prev ?? []).map((s) => (s.id === updated.id ? updated : s)));
            }
            setFormOpen(false);
        } catch {
            setSubmitError('Could not save the spot. Please try again.');
        } finally {
            setSaving(false);
        }
    }

    async function handleDelete(id: number) {
        setListError(null);

        try {
            await spotClient.delete(id);
            setSpots((prev) => (prev ?? []).filter((s) => s.id !== id));

            if (editingId === id) {
                setFormOpen(false);
            }
        } catch (err) {
            if (err instanceof ApiException && err.status === 409) {
                setListError("That spot has catches logged to it, so it can't be deleted.");
            } else {
                setListError('Could not delete the spot. Please try again.');
            }
        } finally {
            setConfirmingId(null);
        }
    }

    if (loading) {
        return <p className="py-10 text-muted">Loading your spots…</p>;
    }

    if (loadError !== null || spots === null) {
        return <p className="py-10 text-danger">{loadError}</p>;
    }

    const sorted = [...spots].sort((a, b) => a.name.localeCompare(b.name));

    return (
        <div className="flex flex-1 flex-col py-8">
            <div className="flex items-center justify-between">
                <h1 className="text-4xl font-bold text-heading">Spots</h1>
                {!formOpen && (
                    <button
                        type="button"
                        onClick={openCreate}
                        className="flex items-center gap-1.5 rounded bg-primary px-4 py-2 text-heading hover:brightness-110"
                    >
                        <Plus size={16} />
                        New spot
                    </button>
                )}
            </div>

            {formOpen && (
                <form onSubmit={handleSubmit} className="mt-6 max-w-2xl rounded-lg border border-line bg-panel p-6">
                    <h2 className="text-lg font-medium text-heading">
                        {editingId === null ? 'New spot' : 'Edit spot'}
                    </h2>

                    <div className="mt-4 grid gap-4 sm:grid-cols-2">
                        <TextField
                            id="name"
                            label="Name"
                            value={name}
                            onChange={setName}
                            required
                        />
                        <div>
                            <label htmlFor="waterBodyType" className="block text-sm font-medium text-heading">
                                Water body
                            </label>
                            <select
                                id="waterBodyType"
                                value={waterBodyType}
                                onChange={(e) => setWaterBodyType(e.target.value as WaterBodyType)}
                                className="mt-1 w-full rounded border border-line bg-sunken px-3 py-2 text-heading focus:border-primary focus:outline-none"
                            >
                                <option value="Lake">Lake</option>
                                <option value="Pond">Pond</option>
                                <option value="River">River</option>
                                <option value="Other">Other</option>
                            </select>
                        </div>
                        <TextField
                            id="latitude"
                            label="Latitude"
                            type="number"
                            value={latitude}
                            onChange={setLatitude}
                            required
                        />
                        <TextField
                            id="longitude"
                            label="Longitude"
                            type="number"
                            value={longitude}
                            onChange={setLongitude}
                            required
                        />
                    </div>

                    <div className="mt-4">
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

                    <div className="mt-6 flex gap-3">
                        <button
                            type="submit"
                            disabled={saving}
                            className="rounded bg-primary px-4 py-2 text-heading hover:brightness-110 disabled:opacity-50"
                        >
                            {saving ? 'Saving…' : editingId === null ? 'Add spot' : 'Save changes'}
                        </button>
                        <button
                            type="button"
                            onClick={() => setFormOpen(false)}
                            className="rounded border border-line px-4 py-2 text-body hover:bg-sunken"
                        >
                            Cancel
                        </button>
                    </div>
                </form>
            )}

            {listError !== null && (
                <p className="mt-4 text-sm text-danger">{listError}</p>
            )}

            {sorted.length === 0 ? (
                <p className="mt-6 text-base text-muted">No spots yet — add your first one.</p>
            ) : (
                <div className="mt-6 grid gap-4 sm:grid-cols-2 lg:grid-cols-3">
                    {sorted.map((spot) => (
                        <article key={spot.id} className="rounded-lg border border-line bg-panel p-4">
                            <div className="flex items-baseline justify-between gap-3">
                                <h2 className="truncate text-lg font-medium text-heading">{spot.name}</h2>
                                <span className="shrink-0 text-sm text-muted">{spot.waterBodyType}</span>
                            </div>

                            <p className="mt-2 flex items-center gap-1.5 text-sm text-muted">
                                <MapPin size={14} />
                                {spot.latitude.toFixed(5)}, {spot.longitude.toFixed(5)}
                            </p>

                            {spot.notes !== undefined && (
                                <p className="mt-2 text-sm text-body">{spot.notes}</p>
                            )}

                            <div className="mt-4 flex gap-2">
                                {confirmingId === spot.id ? (
                                    <>
                                        <button
                                            type="button"
                                            onClick={() => handleDelete(spot.id)}
                                            className="rounded bg-danger px-3 py-1 text-sm text-heading hover:brightness-110"
                                        >
                                            Confirm delete
                                        </button>
                                        <button
                                            type="button"
                                            onClick={() => setConfirmingId(null)}
                                            className="rounded border border-line px-3 py-1 text-sm text-body hover:bg-sunken"
                                        >
                                            Cancel
                                        </button>
                                    </>
                                ) : (
                                    <>
                                        <button
                                            type="button"
                                            onClick={() => openEdit(spot)}
                                            className="rounded border border-line px-3 py-1 text-sm text-body hover:bg-sunken"
                                        >
                                            Edit
                                        </button>
                                        <button
                                            type="button"
                                            onClick={() => setConfirmingId(spot.id)}
                                            className="rounded border border-line px-3 py-1 text-sm text-danger hover:bg-sunken"
                                        >
                                            Delete
                                        </button>
                                    </>
                                )}
                            </div>
                        </article>
                    ))}
                </div>
            )}
        </div>
    );
}

export default Spots;
