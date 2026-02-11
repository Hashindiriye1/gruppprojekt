import { useEffect, useState } from 'react'
import { Link } from 'react-router-dom'
import { api, endpoints } from '../api'

type Customer = { id: number; name: string; email: string; phone: string; createdAt: string }

export default function Customers() {
  const [list, setList] = useState<Customer[]>([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)

  useEffect(() => {
    api<Customer[]>(endpoints.customers)
      .then(setList)
      .catch((e) => setError(e.message))
      .finally(() => setLoading(false))
  }, [])

  if (loading) return <div className="loading">Loading customers…</div>
  if (error) return <div className="error">Error: {error}</div>

  return (
    <div className="page">
      <header className="page-header">
        <h1 className="page-title">Customers</h1>
        <Link to="/customers/new" className="action-link">+ New customer</Link>
      </header>
      <div className="card">
        {list.length === 0 ? (
          <p className="empty-message">No customers yet.</p>
        ) : (
          <ul className="list">
            {list.map((c) => (
              <li key={c.id} className="list-item">
                <span><strong>{c.name}</strong> – {c.email} – {c.phone}</span>
                <Link to={`/customers/${c.id}/edit`} className="edit-link">Edit</Link>
              </li>
            ))}
          </ul>
        )}
      </div>
    </div>
  )
}
