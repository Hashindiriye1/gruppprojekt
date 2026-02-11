import { useEffect, useState } from 'react'
import { Link } from 'react-router-dom'
import { api, endpoints } from '../api'

type ServiceBooking = { bookingId: number; customerName: string; vehicleInfo: string; scheduledDate: string; status: string }
type ServiceWithBookings = {
  id: number
  name: string
  description: string
  price: number
  durationMinutes: number
  bookings: ServiceBooking[]
}

export default function Services() {
  const [list, setList] = useState<ServiceWithBookings[]>([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)

  const fetchList = () => {
    setLoading(true)
    setError(null)
    api<ServiceWithBookings[]>(endpoints.servicesWithBookings)
      .then(setList)
      .catch((e) => setError(e.message))
      .finally(() => setLoading(false))
  }

  useEffect(() => {
    fetchList()
  }, [])

  if (loading && list.length === 0) return <div className="loading">Loading services…</div>
  if (error && list.length === 0) return <div className="error">Error: {error}</div>

  return (
    <div className="page">
      <header className="page-header">
        <h1 className="page-title">Car services</h1>
        <Link to="/services/new" className="action-link">+ New service</Link>
      </header>
      {error && <div className="error">{error}</div>}
      <div className="card">
        {list.length === 0 ? (
          <p className="empty-message">No services defined. <Link to="/services/new">Create your first service</Link>.</p>
        ) : (
          <ul className="list">
            {list.map((s) => (
              <li key={s.id} className="list-item service-item">
                <div className="list-item-content">
                  <span><strong>{s.name}</strong> – {s.description} – {s.price} kr – {s.durationMinutes} min</span>
                  <Link to={`/services/${s.id}/edit`} className="edit-link">Edit</Link>
                </div>
                {s.bookings.length > 0 ? (
                  <ul className="booking-sublist">
                    {s.bookings.map((b) => (
                      <li key={b.bookingId}>
                        {b.customerName} – {b.vehicleInfo} – {new Date(b.scheduledDate).toLocaleString()} – {b.status}
                        <Link to={`/bookings/${b.bookingId}/edit`} className="edit-booking-link">Edit booking</Link>
                      </li>
                    ))}
                  </ul>
                ) : (
                  <p className="no-bookings-note">No bookings for this service yet.</p>
                )}
              </li>
            ))}
          </ul>
        )}
      </div>
    </div>
  )
}
