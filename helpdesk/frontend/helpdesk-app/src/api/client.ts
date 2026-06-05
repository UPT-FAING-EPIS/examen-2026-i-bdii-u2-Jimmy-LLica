import axios from 'axios';

const API_BASE = process.env.REACT_APP_API_URL || 'http://localhost:5000/api';

export interface Ticket {
  id?: string;
  title: string;
  description: string;
  priority: string;
  categoryId: string;
  userId: string;
  assignedAgentId?: string;
  status: string;
  comments: Comment[];
  attachments: string[];
  createdAt?: string;
  updatedAt?: string;
}

export interface Comment {
  userId: string;
  text: string;
  createdAt?: string;
}

export const api = {
  tickets: {
    create: (ticket: Ticket) => axios.post(`${API_BASE}/tickets`, ticket),
    getAll: (params?: { userId?: string; agentId?: string }) =>
      axios.get<Ticket[]>(`${API_BASE}/tickets`, { params }),
    getById: (id: string) => axios.get<Ticket>(`${API_BASE}/tickets/${id}`),
    update: (id: string, ticket: Ticket) => axios.put(`${API_BASE}/tickets/${id}`, ticket),
    addComment: (id: string, comment: Comment) =>
      axios.post(`${API_BASE}/tickets/${id}/comments`, comment),
  },
  categories: {
    getAll: () => axios.get<any[]>(`${API_BASE}/categories`),
    create: (cat: any) => axios.post(`${API_BASE}/categories`, cat),
  },
};