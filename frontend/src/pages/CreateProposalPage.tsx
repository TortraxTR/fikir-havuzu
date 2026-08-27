import { useEffect, useState } from 'react'
import { getUsers, type User } from '../api/users'

export default function UsersPage() {
  const [users, setUsers] = useState<User[]>([])
  const [error, setError] = useState<string | null>(null)

  useEffect(() => {
    getUsers()
      .then(setUsers)
      .catch((err: Error) => setError(err.message))
  }, [])

  if (error) return <p>{error}</p>

  return (
    <ul>
      {users.map((user) => (
        <li key={user.id}>
          {user.name} {user.surname}
        </li>
      ))}
    </ul>
  )
}