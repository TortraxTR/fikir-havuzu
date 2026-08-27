export async function login(phoneNumber: string, password: string) {
  const response = await fetch('http://localhost:5128/api/auth/login', {
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
  console.log('Login response:', data)
  return data
}