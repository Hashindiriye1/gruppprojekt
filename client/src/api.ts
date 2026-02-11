const BASE = import.meta.env.VITE_API_URL ?? '';

export async function api<T>(path: string, options?: RequestInit): Promise<T> {
  const url = path.startsWith('http') ? path : `${BASE}${path}`;
  const res = await fetch(url, {
    ...options,
    headers: {
      'Content-Type': 'application/json',
      ...options?.headers,
    },
  });
  if (!res.ok) {
    const text = await res.text();
    throw new Error(text || `HTTP ${res.status}`);
  }
  if (res.status === 204) return undefined as T;
  return res.json();
}

export const endpoints = {
  customers: '/api/customers',
  vehicles: '/api/vehicles',
  vehiclesByCustomer: (customerId: number) => `/api/vehicles/by-customer/${customerId}`,
  services: '/api/services',
  servicesWithBookings: '/api/services/with-bookings',
  locations: '/api/locations',
  bookings: '/api/bookings',
  bookingsFiltered: '/api/bookings/filtered',
  upcomingByLocation: (id: number) => `/api/bookings/upcoming-by-location/${id}`,
} as const;
