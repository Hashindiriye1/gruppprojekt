import { useEffect, useState } from 'react'
import { useNavigate, useParams } from 'react-router-dom'
import { api, endpoints } from '../api'

export default function LocationForm() {
  const { id } = useParams()
  const navigate = useNavigate()
  const [name, setName] = useState('')
  const [address, setAddress] = useState('')
  const [loading, setLoading] = useState(!!id)
  const [saving, setSaving] = useState(false)
  const [error, setError] = useState<string | null>(null)

  useEffect(() => {
    if (!id) return
    api<{ name: string; address: string }>(`${endpoints.locations}/${id}`)
      .then((d) => {
        setName(d.name)
        setAddress(d.address)
      })
      .catch((e) => setError(e.message))
      .finally(() => setLoading(false))
  }, [id])

  const submit = async (e: React.FormEvent) => {
    e.preventDefault()
    setSaving(true)
    setError(null)
    const body = { name, address }
    try {
      if (id) {
        await api(`${endpoints.locations}/${id}`, { method: 'PUT', body: JSON.stringify(body) })
      } else {
        await api(endpoints.locations, { method: 'POST', body: JSON.stringify(body) })
      }
      navigate('/locations')
    } catch (e: unknown) {
      setError(e instanceof Error ? e.message : 'Failed to save')
    } finally {
      setSaving(false)
    }
  }

  if (loading) return <div className="loading">Loading…</div>

  return (
    <div className="page">
      <h1 className="page-title">{id ? 'Edit location' : 'New location'}</h1>
      {error && <div className="error">{error}</div>}
      <div className="card form-card">
        <form onSubmit={submit}>
          <label>Name <input value={name} onChange={(e) => setName(e.target.value)} required /></label>
          <label>Address <input value={address} onChange={(e) => setAddress(e.target.value)} required /></label>
          <div className="form-actions">
            <button type="submit" className="btn-primary" disabled={saving}>{saving ? 'Saving…' : 'Save'}</button>
            <button type="button" className="btn-secondary" onClick={() => navigate('/locations')}>Cancel</button>
          </div>
        </form>
      </div>
    </div>
  )
}
