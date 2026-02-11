import { useEffect, useState } from 'react'
import { Link } from 'react-router-dom'
import { api, endpoints } from '../api'

type Booking = {
  id: number
  customerId: number
  customerName: string
  vehicleId: number
  vehicleInfo: string
  serviceId: number
  serviceName: string
  locationId: number
  locationName: string
  scheduledDate: string
  status: string
  notes: string | null
  createdAt: string
}

export default function Bookings() {
  const [list, setList] = useState<Booking[]>([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)
  const [status, setStatus] = useState('')
  const [sortBy, setSortBy] = useState('ScheduledDate')
  const [sortOrder, setSortOrder] = useState('asc')
  const [useFilter, setUseFilter] = useState(false)

  const fetchAll = () => {
    setLoading(true)
    setError(null)
    api<Booking[]>(endpoints.bookings)
      .then(setList)
      .catch((e) => setError(e.message))
      .finally(() => setLoading(false))
  }

  const fetchFiltered = () => {
    setLoading(true)
    setError(null)
    const params = new URLSearchParams()
    if (status) params.set('status', status)
    params.set('sortBy', sortBy)
    params.set('sortOrder', sortOrder)
    api<Booking[]>(`${endpoints.bookingsFiltered}?${params}`)
      .then(setList)
      .catch((e) => setError(e.message))
      .finally(() => setLoading(false))
  }

  useEffect(() => {
    if (useFilter) fetchFiltered()
    else fetchAll()
  // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [useFilter, status, sortBy, sortOrder])

  const refresh = () => (useFilter ? fetchFiltered() : fetchAll())

  if (loading && list.length === 0) return <div className="loading">Loading bookings…</div>
  if (error && list.length === 0) return <div className="error">Error: {error}</div>

  return (
    <div className="page">
      <header className="page-header">
        <h1 className="page-title">Bookings</h1>
        <Link to="/bookings/new" className="action-link">+ New booking</Link>
      </header>
      <div className="card filter-card">
        <h3>Filter & sort (advanced)</h3>
        <label>
          <input type="checkbox" checked={useFilter} onChange={(e) => setUseFilter(e.target.checked)} />
          Use filter/sort
        </label>
        {useFilter && (
          <div className="filter-row">
            <label>Status
              <select value={status} onChange={(e) => setStatus(e.target.value)}>
                <option value="">All</option>
                <option value="Pending">Pending</option>
                <option value="Confirmed">Confirmed</option>
                <option value="Completed">Completed</option>
                <option value="Cancelled">Cancelled</option>
              </select>
            </label>
            <label>Sort by
              <select value={sortBy} onChange={(e) => setSortBy(e.target.value)}>
                <option value="ScheduledDate">Date</option>
                <option value="Status">Status</option>
                <option value="CreatedAt">Created</option>
              </select>
            </label>
            <label>Order
              <select value={sortOrder} onChange={(e) => setSortOrder(e.target.value)}>
                <option value="asc">Ascending</option>
                <option value="desc">Descending</option>
              </select>
            </label>
            <button type="button" className="btn-secondary" onClick={refresh}>Apply</button>
          </div>
        )}
      </div>
      {error && <div className="error">{error}</div>}
      <div className="card">
        {list.length === 0 ? (
          <p className="empty-message">No bookings yet.</p>
        ) : (
          <ul className="list">
            {list.map((b) => (
              <li key={b.id} className="list-item">
                <span>
                  <strong>#{b.id}</strong> – {b.customerName} – {b.vehicleInfo} – {b.serviceName} @ {b.locationName} – {new Date(b.scheduledDate).toLocaleString()}{' '}
                  <span className={`status-badge status-${b.status.toLowerCase()}`}>{b.status}</span>
                </span>
                <Link to={`/bookings/${b.id}/edit`} className="edit-link">Edit</Link>
              </li>
            ))}
          </ul>
        )}
      </div>
    </div>
  )
}
