import React from 'react';
import { Ticket } from '../api/client';

interface Props {
  tickets: Ticket[];
  onStatusChange?: (id: string, newStatus: string) => void;
  onCommentAdd?: (id: string, comment: string) => void;
  isAgent?: boolean;
}

export const TicketList: React.FC<Props> = ({ tickets, onStatusChange, onCommentAdd, isAgent = false }) => {
  const [commentText, setCommentText] = React.useState<{ [key: string]: string }>({});

  const handleComment = (id: string) => {
    const text = commentText[id];
    if (text && onCommentAdd) {
      onCommentAdd(id, text);
      setCommentText(prev => ({ ...prev, [id]: '' }));
    }
  };

  return (
    <div className="ticket-list">
      {tickets.length === 0 && <p>No hay tickets.</p>}
      {tickets.map(ticket => (
        <div key={ticket.id} className="ticket-card" style={{ border: '1px solid #ccc', margin: '10px', padding: '10px' }}>
          <h3>{ticket.title}</h3>
          <p>{ticket.description}</p>
          <p><strong>Prioridad:</strong> {ticket.priority}</p>
          <p><strong>Estado:</strong> {ticket.status}</p>
          <p><strong>Categoría:</strong> {ticket.categoryId}</p>
          {isAgent && onStatusChange && (
            <select
              value={ticket.status}
              onChange={(e) => onStatusChange(ticket.id!, e.target.value)}
            >
              <option value="abierto">Abierto</option>
              <option value="en progreso">En progreso</option>
              <option value="resuelto">Resuelto</option>
              <option value="cerrado">Cerrado</option>
            </select>
          )}
          <div className="comments">
            <h4>Comentarios</h4>
            {ticket.comments.map((c, idx) => (
              <div key={idx}><strong>{c.userId}:</strong> {c.text} <small>{new Date(c.createdAt!).toLocaleString()}</small></div>
            ))}
            {onCommentAdd && (
              <div>
                <input
                  type="text"
                  value={commentText[ticket.id!] || ''}
                  onChange={(e) => setCommentText(prev => ({ ...prev, [ticket.id!]: e.target.value }))}
                  placeholder="Agregar comentario..."
                />
                <button onClick={() => handleComment(ticket.id!)}>Comentar</button>
              </div>
            )}
          </div>
        </div>
      ))}
    </div>
  );
};