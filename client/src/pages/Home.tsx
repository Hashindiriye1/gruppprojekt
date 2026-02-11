export default function Home() {
  return (
    <div className="page page-home">
      <section className="hero">
        <h1>Car Service Booking</h1>
        <p>Book car service appointments easily. Use the menu to manage customers, vehicles, services, locations and bookings.</p>
      </section>
      <ul className="feature-list">
        <li><strong>Customers</strong> – Add and edit customers.</li>
        <li><strong>Vehicles</strong> – Register vehicles per customer.</li>
        <li><strong>Services</strong> – Add and edit car services and prices.</li>
        <li><strong>Locations</strong> – Add and edit service locations.</li>
        <li><strong>Bookings</strong> – Create and manage service bookings (filter and sort available).</li>
      </ul>
    </div>
  )
}
