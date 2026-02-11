import { useEffect, useState } from 'react'
import { useNavigate, useParams } from 'react-router-dom'
import { api, endpoints } from '../api'

type Customer = { id: number; name: string }

export default function VehicleForm() {
  const { id } = useParams()
  const navigate = useNavigate()
  const [customers, setCustomers] = useState<Customer[]>([])
  const [customerId, setCustomerId] = useState(0)
  const [licensePlate, setLicensePlate] = useState('')
  const [make, setMake] = useState('')
  const [model, setModel] = useState('')
  const [loading, setLoading] = useState(true)
  const [saving, setSaving] = useState(false)
  const [error, setError] = useState<string | null>(null)

  useEffect(() => {
    api<Customer[]>(endpoints.customers)
      .then((list) => {
        setCustomers(list)
        if (list.length) setCustomerId(list[0].id)
      })
      .catch((e) => setError(e.message))
      .finally(() => { if (!id) setLoading(false) })
  }, [id])

  useEffect(() => {
    if (!id) return
    setLoading(true)
    api<{ customerId: number; licensePlate: string; make: string; model: string }>(`${endpoints.vehicles}/${id}`)
      .then((v) => {
        setCustomerId(v.customerId)
        setLicensePlate(v.licensePlate)
        setMake(v.make)
        setModel(v.model)
      })
      .catch((e) => setError(e.message))
      .finally(() => setLoading(false))
  }, [id])

  const submit = async (e: React.FormEvent) => {
    e.preventDefault()
    setSaving(true)
    setError(null)
    try {
      if (id) {
        await api(`${endpoints.vehicles}/${id}`, {
          method: 'PUT',
          body: JSON.stringify({ licensePlate, make, model }),
        })
      } else {
        await api(endpoints.vehicles, {
          method: 'POST',
          body: JSON.stringify({ customerId, licensePlate, make, model }),
        })
      }
      navigate('/vehicles')
    } catch (e: unknown) {
      setError(e instanceof Error ? e.message : 'Failed to save')
    } finally {
      setSaving(false)
    }
  }

  if (loading) return <div className="loading">Loading…</div>

  return (
    <div className="page">
      <h1 className="page-title">{id ? 'Edit vehicle' : 'Register vehicle'}</h1>
      {error && <div className="error">{error}</div>}
      <div className="card form-card">
        <form onSubmit={submit}>
          <label>
            Customer
            <select
              value={customerId}
              onChange={(e) => setCustomerId(Number(e.target.value))}
              required
              disabled={!!id}
            >
              <option value={0}>Select customer</option>
              {customers.map((c) => (
                <option key={c.id} value={c.id}>{c.name}</option>
              ))}
            </select>
          </label>
          <label>
            License plate
            <input
              value={licensePlate}
              onChange={(e) => setLicensePlate(e.target.value)}
              required
              placeholder="e.g. ABC123"
            />
          </label>
          <label>
            Make
            <input
              value={make}
              onChange={(e) => setMake(e.target.value)}
              required
              placeholder="e.g. Volvo"
            />
          </label>
          <label>
            Model
            <input
              value={model}
              onChange={(e) => setModel(e.target.value)}
              required
              placeholder="e.g. V60"
            />
          </label>
          <div className="form-actions">
            <button type="submit" className="btn-primary" disabled={saving}>
              {saving ? 'Saving…' : 'Save'}
            </button>
            <button type="button" className="btn-secondary" onClick={() => navigate('/vehicles')}>
              Cancel
            </button>
          </div>
        </form>
      </div>
    </div>
  )
}
