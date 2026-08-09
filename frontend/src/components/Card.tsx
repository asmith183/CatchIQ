import type { ReactNode } from 'react';

function Card({ title, children }: { title: string; children: ReactNode }) {
    return (
        <section className="rounded-lg border border-emerald-800 bg-emerald-900 p-4">
            <h2 className="text-sm font-medium text-emerald-300">{title}</h2>
            {children}
        </section>
    );
}

export default Card;
