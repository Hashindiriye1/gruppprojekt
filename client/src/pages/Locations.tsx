import { useEffect, useState } from 'react'
import { Link } from 'react-router-dom'
import { api, endpoints } from '../api'

type Location = { id: number; name: string; address: string }

export default function Locations() {
  const [list, setList] = useState<Location[]>([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)

  useEffect(() => {
    api<Location[]>(endpoints.locations)
      .then(setList)
      .catch((e) => setError(e.message))
      .finally(() => setLoading(false))
  }, [])

  if (loading) return <div className="loading">Loading locations…</div>
  if (error) return <div className="error">Error: {error}</div>

  return (
    <div className="page">
      <header className="page-header">
        <h1 className="page-title">Locations</h1>
        <Link to="/locations/new" className="action-link">+ New location</Link>
      </header>
      <div className="card">
        {list.length === 0 ? (
          <p className="empty-message">No locations defined. <Link to="/locations/new">Create your first location</Link>.</p>
        ) : (
          <ul className="list">
            {list.map((l) => (
              <li key={l.id} className="list-item">
                <span><strong>{l.name}</strong> – {l.address}</span>
                <Link to={`/locations/${l.id}/edit`} className="edit-link">Edit</Link>
              </li>
            ))}
          </ul>
        )}
      </div>
    </div>
  )
}
