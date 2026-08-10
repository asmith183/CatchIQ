import type { ReactNode } from 'react';

function Card({ title, children }: { title: string; children: ReactNode }) {
    return (
        <section className="flex h-full flex-col rounded-lg border border-line bg-panel p-6">
            <h2 className="text-xl font-medium text-muted">{title}</h2>
            {children}
        </section>
    );
}

export default Card;
