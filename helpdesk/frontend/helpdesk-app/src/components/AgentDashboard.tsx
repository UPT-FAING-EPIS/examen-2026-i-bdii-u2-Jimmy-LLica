import React, { useEffect, useState } from 'react';
import { api, Ticket } from '../api/client';

export const AgentDashboard: React.FC<{ agentId: string }> = ({ agentId }) => {
  const [tickets, setTickets] = useState<Ticket[]>([]);

  useEffect(() => {
    api.tickets.getAll({ agentId }).then(res => setTickets(res.data));
  }, [agentId]);

  const updateStatus = async (id: string, newStatus: string) => {
    const ticket = tickets.find(t => t.id === id);
    if (ticket) {
      ticket.status = newStatus;
      await api.tickets.update(id, ticket);
      setTickets(prev => prev.map(t => t.id === id ? { ...t, status: newStatus } : t));
    }
  };

  return (
    <div>
      <h2>Tickets Asignados</h2>
      {tickets.map(t => (
        <div key={t.id}>
          <strong>{t.title}</strong> - Estado: {t.status}
          <select onChange={e => updateStatus(t.id!, e.target.value)} value={t.status}>
            <option value="abierto">Abierto</option><option value="en progreso">En progreso</option>
            <option value="resuelto">Resuelto</option><option value="cerrado">Cerrado</option>
          </select>
        </div>
      ))}
    </div>
  );
};