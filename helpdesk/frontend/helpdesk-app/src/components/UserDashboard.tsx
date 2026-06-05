import React, { useEffect, useState } from 'react';
import { api, Ticket } from '../api/client';

export const UserDashboard: React.FC<{ userId: string }> = ({ userId }) => {
  const [tickets, setTickets] = useState<Ticket[]>([]);

  useEffect(() => {
    api.tickets.getAll({ userId }).then(res => setTickets(res.data));
  }, [userId]);

  return (
    <div>
      <h2>Mis Tickets</h2>
      <ul>
        {tickets.map(t => (
          <li key={t.id}>{t.title} - {t.status} - Prioridad: {t.priority}</li>
        ))}
      </ul>
    </div>
  );
};