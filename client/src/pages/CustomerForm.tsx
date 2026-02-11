import { useEffect, useState } from 'react'
import { useNavigate, useParams } from 'react-router-dom'
import { api, endpoints } from '../api'

export default function CustomerForm() {
  const { id } = useParams()
  const navigate = useNavigate()
  const [name, setName] = useState('')
  const [email, setEmail] = useState('')
  const [phone, setPhone] = useState('')
  const [loading, setLoading] = useState(!!id)
  const [saving, setSaving] = useState(false)
  const [error, setError] = useState<string | null>(null)

  useEffect(() => {
    if (!id) return
    api<{ name: string; email: string; phone: string }>(`${endpoints.customers}/${id}`)
      .then((d) => {
        setName(d.name)
        setEmail(d.email)
        setPhone(d.phone)
      })
      .catch((e) => setError(e.message))
      .finally(() => setLoading(false))
  }, [id])

  const submit = async (e: React.FormEvent) => {
    e.preventDefault()
    setSaving(true)
    setError(null)
    const body = { name, email, phone }
    try {
      if (id) {
        await api(`${endpoints.customers}/${id}`, { method: 'PUT', body: JSON.stringify(body) })
      } else {
        await api(endpoints.customers, { method: 'POST', body: JSON.stringify(body) })
      }
      navigate('/customers')
    } catch (e: unknown) {
      setError(e instanceof Error ? e.message : 'Failed to save')
    } finally {
      setSaving(false)
    }
  }

  if (loading) return <div className="loading">Loading…</div>

  return (
    <div className="page">
      <h1 className="page-title">{id ? 'Edit customer' : 'New customer'}</h1>
      {error && <div className="error">{error}</div>}
      <div className="card form-card">
        <form onSubmit={submit}>
          <label>Name <input value={name} onChange={(e) => setName(e.target.value)} required /></label>
          <label>Email <input type="email" value={email} onChange={(e) => setEmail(e.target.value)} required /></label>
          <label>Phone <input value={phone} onChange={(e) => setPhone(e.target.value)} required /></label>
          <div className="form-actions">
            <button type="submit" className="btn-primary" disabled={saving}>{saving ? 'Saving…' : 'Save'}</button>
            <button type="button" className="btn-secondary" onClick={() => navigate('/customers')}>Cancel</button>
          </div>
        </form>
      </div>
    </div>
  )
}
