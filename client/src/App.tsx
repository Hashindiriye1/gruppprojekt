import { Routes, Route, NavLink } from 'react-router-dom'
import Home from './pages/Home'
import Customers from './pages/Customers'
import CustomerForm from './pages/CustomerForm'
import Vehicles from './pages/Vehicles'
import VehicleForm from './pages/VehicleForm'
import Services from './pages/Services'
import ServiceForm from './pages/ServiceForm'
import Locations from './pages/Locations'
import LocationForm from './pages/LocationForm'
import Bookings from './pages/Bookings'
import BookingForm from './pages/BookingForm'

export default function App() {
  return (
    <div className="app-layout">
      <nav className="app-nav">
        <NavLink to="/" end className={({ isActive }) => isActive ? 'active' : ''}>Home</NavLink>
        <NavLink to="/customers" className={({ isActive }) => isActive ? 'active' : ''}>Customers</NavLink>
        <NavLink to="/vehicles" className={({ isActive }) => isActive ? 'active' : ''}>Vehicles</NavLink>
        <NavLink to="/services" className={({ isActive }) => isActive ? 'active' : ''}>Services</NavLink>
        <NavLink to="/locations" className={({ isActive }) => isActive ? 'active' : ''}>Locations</NavLink>
        <NavLink to="/bookings" className={({ isActive }) => isActive ? 'active' : ''}>Bookings</NavLink>
      </nav>
      <main className="app-main">
        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/customers" element={<Customers />} />
          <Route path="/customers/new" element={<CustomerForm />} />
          <Route path="/customers/:id/edit" element={<CustomerForm />} />
          <Route path="/vehicles" element={<Vehicles />} />
          <Route path="/vehicles/new" element={<VehicleForm />} />
          <Route path="/vehicles/:id/edit" element={<VehicleForm />} />
          <Route path="/services" element={<Services />} />
          <Route path="/services/new" element={<ServiceForm />} />
          <Route path="/services/:id/edit" element={<ServiceForm />} />
          <Route path="/locations" element={<Locations />} />
          <Route path="/locations/new" element={<LocationForm />} />
          <Route path="/locations/:id/edit" element={<LocationForm />} />
          <Route path="/bookings" element={<Bookings />} />
          <Route path="/bookings/new" element={<BookingForm />} />
          <Route path="/bookings/:id/edit" element={<BookingForm />} />
        </Routes>
      </main>
    </div>
  )
}
