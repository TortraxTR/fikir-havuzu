import type { Permission } from "./types/Permission";
import type { User } from "./types/User";

const API_URL = import.meta.env.VITE_API_URL;

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
    const errorText = await response.text();
    console.log('Login error:', errorText);
    throw new Error(errorText);
  }

  const data = await response.json()
  return data
}

export async function fetchAllUsers(): Promise<User[]> {
  const response = await fetch(`${API_URL}/users`, {
    method: 'GET',
    headers: { Accept: 'application/json' },
  });

  if (!response.ok) {
    throw new Error(`Kullanıcılar alınamadı: ${response.statusText}`);
  }

  return (await response.json()) as User[];
}

export async function updateUser(user: User): Promise<User> {
  const response = await fetch(`${API_URL}/users/${user.id}`, {
    method: 'PUT',
    headers: {
      Accept: 'application/json',
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({
      name: user.name,
      surname: user.surname,
      phone: user.phone,
      registrationNo: user.registrationNo,
      governmentId: user.governmentId,
      isActive: user.isActive,
    }),
  });

  if (!response.ok) {
    const errorText = await response.text();
    throw new Error(errorText || `Kullanıcı güncellenemedi: ${response.statusText}`);
  }

  return (await response.json()) as User;
}

export async function createUser(data: {
  name: string;
  surname: string;
  phone: string;
  registrationNo: string;
  governmentId: string;
  password: string;
  isActive: boolean;
}): Promise<User> {
  const response = await fetch(`${API_URL}/users`, {
    method: 'POST',
    headers: {
      Accept: 'application/json',
      'Content-Type': 'application/json',
    },
    body: JSON.stringify(data),
  });

  if (!response.ok) {
    const errorText = await response.text();
    throw new Error(errorText || `Kullanıcı oluşturulamadı: ${response.statusText}`);
  }

  return (await response.json()) as User;
}

export async function setUserActive(userId: string, isActive: boolean): Promise<User> {
  const response = await fetch(`${API_URL}/users/${userId}/setActive`, {
    method: 'PUT',
    headers: {
      Accept: 'application/json',
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({ isActive }),
  });

  if (!response.ok) {
    const errorText = await response.text();
    throw new Error(errorText || `Kullanıcı durumu güncellenemedi: ${response.statusText}`);
  }

  return (await response.json()) as User;
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
  return permissions;
}

export async function fetchAllPermissions(): Promise<Permission[]> {
  const response = await fetch(`${API_URL}/permissions`, {
    method: 'GET',
    headers: { Accept: 'application/json' },
  });

  if (!response.ok) {
    throw new Error(`Yetkiler alınamadı: ${response.statusText}`);
  }
  return (await response.json()) as Permission[];
}

export async function addPermissionToUser(userId: string, permissionId: string): Promise<void> {
  const response = await fetch(`${API_URL}/users/${userId}/permissions`, {
    method: 'POST',
    headers: {
      Accept: 'application/json',
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({ permissionId }),
  });

  if (!response.ok) {
    const errorText = await response.text();
    throw new Error(errorText || `Yetki eklenemedi: ${response.statusText}`);
  }
}

export async function removePermissionFromUser(userId: string, permissionId: string): Promise<void> {
  const response = await fetch(`${API_URL}/users/${userId}/permissions/${permissionId}`, {
    method: 'DELETE',
    headers: { Accept: 'application/json' },
  });

  if (!response.ok) {
    const errorText = await response.text();
    throw new Error(errorText || `Yetki silinemedi: ${response.statusText}`);
  }
}