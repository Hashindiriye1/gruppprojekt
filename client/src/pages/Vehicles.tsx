import { useEffect, useState } from 'react'
import { Link } from 'react-router-dom'
import { api, endpoints } from '../api'

type Vehicle = { id: number; customerId: number; licensePlate: string; make: string; model: string }

export default function Vehicles() {
  const [list, setList] = useState<Vehicle[]>([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)

  useEffect(() => {
    api<Vehicle[]>(endpoints.vehicles)
      .then(setList)
      .catch((e) => setError(e.message))
      .finally(() => setLoading(false))
  }, [])

  if (loading) return <div className="loading">Loading vehicles…</div>
  if (error) return <div className="error">Error: {error}</div>

  return (
    <div className="page">
      <header className="page-header">
        <h1 className="page-title">Vehicles</h1>
        <Link to="/vehicles/new" className="action-link">+ Register vehicle</Link>
      </header>
      <div className="card">
        {list.length === 0 ? (
          <p className="empty-message">No vehicles yet. Add a customer first, then <Link to="/vehicles/new">register a vehicle</Link>.</p>
        ) : (
          <ul className="list">
            {list.map((v) => (
              <li key={v.id} className="list-item">
                <span><strong>{v.licensePlate}</strong> – {v.make} {v.model} (Customer #{v.customerId})</span>
                <Link to={`/vehicles/${v.id}/edit`} className="edit-link">Edit</Link>
              </li>
            ))}
          </ul>
        )}
      </div>
    </div>
  )
}
