export type User = {
  id: number;
  name: string;
  surname: string;
  phone: string;
  registrationNo: string;
  governmentId: string;
  isActive: boolean;
}

const API_URL = import.meta.env.VITE_API_URL ?? 'http://localhost:5128/api';

export async function getUsers(): Promise<User[]> {
  const response = await fetch(`${API_URL}/users`);

  if (!response.ok) {
    throw new Error(`Failed to fetch users: ${response.statusText}`);
  }

  const data = await response.json();
  console.log('Fetched users:', data);
  return data;
}