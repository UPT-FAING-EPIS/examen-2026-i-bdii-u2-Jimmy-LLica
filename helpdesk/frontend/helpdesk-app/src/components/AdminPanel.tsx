import React, { useState, useEffect } from 'react';
import { api } from '../api/client';

export const AdminPanel: React.FC = () => {
  const [catName, setCatName] = useState('');
  const [categories, setCategories] = useState<any[]>([]);

  useEffect(() => {
    api.categories.getAll().then(res => setCategories(res.data));
  }, []);

  const createCategory = async () => {
    await api.categories.create({ name: catName });
    setCatName('');
    const res = await api.categories.getAll();
    setCategories(res.data);
  };

  return (
    <div>
      <h2>Administrar Categorías</h2>
      <input value={catName} onChange={e => setCatName(e.target.value)} />
      <button onClick={createCategory}>Crear</button>
      <ul>{categories.map(c => <li key={c.id}>{c.name}</li>)}</ul>
    </div>
  );
};