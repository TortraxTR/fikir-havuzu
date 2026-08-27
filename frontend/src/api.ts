const API_URL = import.meta.env.VITE_API_URL;

export type Permission = {
  id: string;
  name: string;
};

// Login function to authenticate user
export async function login(phoneNumber: string, password: string) {
  const response = await fetch(`${API_URL}/auth/login`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({ phoneNumber, password }),
  })

  if (!response.ok) {
    throw new Error('Telefon numarası veya şifre hatalı. Lütfen tekrar deneyin.')
  }

  const data = await response.json()
  // console.log('Login response:', data)
  return data
}

// Fetch user permissions function
export async function fetchUserPermissions(id: string): Promise<Permission[]> {
  const response = await fetch(`${API_URL}/users/${id}/permissions`, {
    method: 'GET',
    headers: { Accept: 'application/json' },
  });

  if (!response.ok) {
    if (response.status === 404) {
      throw new Error('Kullanıcı bulunamadı.');
    }

    throw new Error(`Yetkiler alınamadı: ${response.statusText}`);
  }

  const permissions = (await response.json()) as Permission[];
  // console.log('User permissions:', permissions);
  return permissions;
}
