import React, { useState, useEffect } from 'react';
import { api, Ticket } from '../api/client';

interface Props {
  userId: string;
  onSuccess?: () => void;
}

export const TicketForm: React.FC<Props> = ({ userId, onSuccess }) => {
  const [title, setTitle] = useState('');
  const [description, setDescription] = useState('');
  const [priority, setPriority] = useState('media');
  const [categoryId, setCategoryId] = useState('');
  const [categories, setCategories] = useState<any[]>([]);

  useEffect(() => {
    api.categories.getAll().then(res => setCategories(res.data));
  }, []);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    const ticket: Ticket = { title, description, priority, categoryId, userId, status: 'abierto', comments: [], attachments: [] };
    await api.tickets.create(ticket);
    onSuccess?.();
    setTitle(''); setDescription('');
  };

  return (
    <form onSubmit={handleSubmit}>
      <input type="text" placeholder="Título" value={title} onChange={e => setTitle(e.target.value)} required />
      <textarea placeholder="Descripción" value={description} onChange={e => setDescription(e.target.value)} required />
      <select value={priority} onChange={e => setPriority(e.target.value)}>
        <option value="baja">Baja</option><option value="media">Media</option><option value="alta">Alta</option>
      </select>
      <select value={categoryId} onChange={e => setCategoryId(e.target.value)} required>
        <option value="">Selecciona categoría</option>
        {categories.map(c => <option key={c.id} value={c.id}>{c.name}</option>)}
      </select>
      <button type="submit">Crear Ticket</button>
    </form>
  );
};