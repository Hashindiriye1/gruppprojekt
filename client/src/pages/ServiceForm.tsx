import { useEffect, useState } from 'react'
import { useNavigate, useParams } from 'react-router-dom'
import { api, endpoints } from '../api'

export default function ServiceForm() {
  const { id } = useParams()
  const navigate = useNavigate()
  const [name, setName] = useState('')
  const [description, setDescription] = useState('')
  const [price, setPrice] = useState('')
  const [durationMinutes, setDurationMinutes] = useState('')
  const [loading, setLoading] = useState(!!id)
  const [saving, setSaving] = useState(false)
  const [error, setError] = useState<string | null>(null)

  useEffect(() => {
    if (!id) return
    api<{ name: string; description: string; price: number; durationMinutes: number }>(`${endpoints.services}/${id}`)
      .then((d) => {
        setName(d.name)
        setDescription(d.description)
        setPrice(String(d.price))
        setDurationMinutes(String(d.durationMinutes))
      })
      .catch((e) => setError(e.message))
      .finally(() => setLoading(false))
  }, [id])

  const submit = async (e: React.FormEvent) => {
    e.preventDefault()
    setSaving(true)
    setError(null)
    const body = {
      name,
      description,
      price: Number(price),
      durationMinutes: Number(durationMinutes),
    }
    try {
      if (id) {
        await api(`${endpoints.services}/${id}`, { method: 'PUT', body: JSON.stringify(body) })
      } else {
        await api(endpoints.services, { method: 'POST', body: JSON.stringify(body) })
      }
      navigate('/services')
    } catch (e: unknown) {
      setError(e instanceof Error ? e.message : 'Failed to save')
    } finally {
      setSaving(false)
    }
  }

  if (loading) return <div className="loading">Loading…</div>

  return (
    <div className="page">
      <h1 className="page-title">{id ? 'Edit service' : 'New service'}</h1>
      {error && <div className="error">{error}</div>}
      <div className="card form-card">
        <form onSubmit={submit}>
          <label>Name <input value={name} onChange={(e) => setName(e.target.value)} required /></label>
          <label>Description <input value={description} onChange={(e) => setDescription(e.target.value)} required /></label>
          <label>Price (kr) <input type="number" min="0" step="0.01" value={price} onChange={(e) => setPrice(e.target.value)} required /></label>
          <label>Duration (minutes) <input type="number" min="1" value={durationMinutes} onChange={(e) => setDurationMinutes(e.target.value)} required /></label>
          <div className="form-actions">
            <button type="submit" className="btn-primary" disabled={saving}>{saving ? 'Saving…' : 'Save'}</button>
            <button type="button" className="btn-secondary" onClick={() => navigate('/services')}>Cancel</button>
          </div>
        </form>
      </div>
    </div>
  )
}
