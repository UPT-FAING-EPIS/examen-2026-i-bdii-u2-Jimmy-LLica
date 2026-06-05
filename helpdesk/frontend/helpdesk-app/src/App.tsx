import React from 'react';
import { BrowserRouter, Routes, Route, Link } from 'react-router-dom';
import { TicketForm } from './components/TicketForm';
import { UserDashboard } from './components/UserDashboard';
import { AgentDashboard } from './components/AgentDashboard';
import { AdminPanel } from './components/AdminPanel';

function App() {
  const currentUserId = "user123"; // En una app real vendría de login
  const currentAgentId = "agent456";
  const isAdmin = true;

  return (
    <BrowserRouter>
      <nav>
        <Link to="/">Crear Ticket</Link> | <Link to="/user">Mis Tickets</Link> | 
        <Link to="/agent">Panel Agente</Link> | <Link to="/admin">Admin</Link>
      </nav>
      <Routes>
        <Route path="/" element={<TicketForm userId={currentUserId} />} />
        <Route path="/user" element={<UserDashboard userId={currentUserId} />} />
        <Route path="/agent" element={<AgentDashboard agentId={currentAgentId} />} />
        <Route path="/admin" element={isAdmin ? <AdminPanel /> : <div>Acceso denegado</div>} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;