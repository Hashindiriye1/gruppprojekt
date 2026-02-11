import { useEffect, useState } from 'react'
import { useNavigate, useParams } from 'react-router-dom'
import { api, endpoints } from '../api'

type Customer = { id: number; name: string }
type Vehicle = { id: number; customerId: number; licensePlate: string; make: string; model: string }
type Service = { id: number; name: string; price: number }
type Location = { id: number; name: string; address: string }

export default function BookingForm() {
  const { id } = useParams()
  const navigate = useNavigate()
  const [customers, setCustomers] = useState<Customer[]>([])
  const [vehicles, setVehicles] = useState<Vehicle[]>([])
  const [services, setServices] = useState<Service[]>([])
  const [locations, setLocations] = useState<Location[]>([])
  const [customerId, setCustomerId] = useState(0)
  const [vehicleId, setVehicleId] = useState(0)
  const [serviceId, setServiceId] = useState(0)
  const [locationId, setLocationId] = useState(0)
  const [scheduledDate, setScheduledDate] = useState('')
  const [status, setStatus] = useState('Pending')
  const [notes, setNotes] = useState('')
  const [loading, setLoading] = useState(true)
  const [saving, setSaving] = useState(false)
  const [error, setError] = useState<string | null>(null)

  useEffect(() => {
    Promise.all([
      api<Customer[]>(endpoints.customers),
      api<Vehicle[]>(endpoints.vehicles),
      api<Service[]>(endpoints.services),
      api<Location[]>(endpoints.locations),
    ]).then(([c, v, s, l]) => {
      setCustomers(c)
      setVehicles(v)
      setServices(s)
      setLocations(l)
      if (c.length) setCustomerId(c[0].id)
      if (v.length) setVehicleId(v[0].id)
      if (s.length) setServiceId(s[0].id)
      if (l.length) setLocationId(l[0].id)
    }).catch((e) => setError(e.message)).finally(() => setLoading(false))
  }, [])

  useEffect(() => {
    if (!id) return
    api<{
      customerId: number
      vehicleId: number
      serviceId: number
      locationId: number
      scheduledDate: string
      status: string
      notes: string | null
    }>(`${endpoints.bookings}/${id}`)
      .then((d) => {
        setCustomerId(d.customerId)
        setVehicleId(d.vehicleId)
        setServiceId(d.serviceId)
        setLocationId(d.locationId)
        setScheduledDate(d.scheduledDate.slice(0, 16))
        setStatus(d.status)
        setNotes(d.notes ?? '')
      })
      .catch((e) => setError(e.message))
  }, [id])

  const submit = async (e: React.FormEvent) => {
    e.preventDefault()
    setSaving(true)
    setError(null)
    const body = id
      ? { scheduledDate: new Date(scheduledDate).toISOString(), status, notes }
      : { customerId, vehicleId, serviceId, locationId, scheduledDate: new Date(scheduledDate).toISOString(), notes }
    const method = id ? 'PUT' : 'POST'
    const path = id ? `${endpoints.bookings}/${id}` : endpoints.bookings
    try {
      await api(path, { method, body: JSON.stringify(body) })
      navigate('/bookings')
    } catch (e: unknown) {
      setError(e instanceof Error ? e.message : 'Failed to save')
    } finally {
      setSaving(false)
    }
  }

  const filteredVehicles = vehicles.filter((v) => v.customerId === customerId)
  useEffect(() => {
    if (filteredVehicles.length && !filteredVehicles.some((v) => v.id === vehicleId)) {
      setVehicleId(filteredVehicles[0].id)
    }
  }, [customerId, filteredVehicles.length])

  if (loading) return <div className="loading">Loading…</div>

  return (
    <div className="page">
      <h1 className="page-title">{id ? 'Edit booking' : 'New booking'}</h1>
      {error && <div className="error">{error}</div>}
      <div className="card form-card">
        <form onSubmit={submit}>
          <label>Customer
            <select value={customerId} onChange={(e) => setCustomerId(Number(e.target.value))} required disabled={!!id}>
              {customers.map((c) => (
                <option key={c.id} value={c.id}>{c.name}</option>
              ))}
            </select>
          </label>
          <label>Vehicle
            <select value={vehicleId} onChange={(e) => setVehicleId(Number(e.target.value))} required disabled={!!id}>
              {filteredVehicles.map((v) => (
                <option key={v.id} value={v.id}>{v.licensePlate} – {v.make} {v.model}</option>
              ))}
            </select>
          </label>
          <label>Service
            <select value={serviceId} onChange={(e) => setServiceId(Number(e.target.value))} required disabled={!!id}>
              {services.map((s) => (
                <option key={s.id} value={s.id}>{s.name}</option>
              ))}
            </select>
          </label>
          <label>Location
            <select value={locationId} onChange={(e) => setLocationId(Number(e.target.value))} required disabled={!!id}>
              {locations.map((l) => (
                <option key={l.id} value={l.id}>{l.name}</option>
              ))}
            </select>
          </label>
          <label>Date & time
            <input
              type="datetime-local"
              value={scheduledDate}
              onChange={(e) => setScheduledDate(e.target.value)}
              required
            />
          </label>
          {id && (
            <label>Status
              <select value={status} onChange={(e) => setStatus(e.target.value)}>
                <option value="Pending">Pending</option>
                <option value="Confirmed">Confirmed</option>
                <option value="Completed">Completed</option>
                <option value="Cancelled">Cancelled</option>
              </select>
            </label>
          )}
          <label>Notes <textarea value={notes} onChange={(e) => setNotes(e.target.value)} rows={2} /></label>
          <div className="form-actions">
            <button type="submit" className="btn-primary" disabled={saving}>{saving ? 'Saving…' : 'Save'}</button>
            <button type="button" className="btn-secondary" onClick={() => navigate('/bookings')}>Cancel</button>
          </div>
        </form>
      </div>
    </div>
  )
}
