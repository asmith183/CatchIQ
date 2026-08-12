import type { ReactNode } from 'react';
import type { LucideIcon } from 'lucide-react';

type Props = {
    title: string;
    icon?: LucideIcon;
    children: ReactNode;
};

function Card({ title, icon: Icon, children }: Props) {
    return (
        <section className="flex h-full flex-col rounded-lg border border-line bg-panel p-6">
            <h2 className="flex items-center gap-3 text-lg font-medium text-muted">
                {Icon && <Icon size={18} className="text-icon" />}
                {title}
            </h2>
            {children}
        </section>
    );
}

export default Card;
