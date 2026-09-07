import { useEffect, useState, type SubmitEvent } from 'react';
import {
	Alert,
	Autocomplete,
	Box,
	Button,
	TextField,
	Typography,
} from '@mui/material';
import { addPermissionToUser, fetchAllPermissions, fetchAllUsers } from '../../api';
import type { Permission } from '../../types/Permission';
import type { User } from '../../types/User';

export default function PermissionAdd() {
	const [users, setUsers] = useState<User[]>([]);
	const [permissions, setPermissions] = useState<Permission[]>([]);
	const [selectedUserId, setSelectedUserId] = useState('');
	const [selectedPermissionId, setSelectedPermissionId] = useState('');
	const [loading, setLoading] = useState(true);
	const [submitting, setSubmitting] = useState(false);
	const [error, setError] = useState<string | null>(null);
	const [success, setSuccess] = useState<string | null>(null);

	useEffect(() => {
		let isMounted = true;

		Promise.all([fetchAllUsers(), fetchAllPermissions()])
			.then(([fetchedUsers, fetchedPermissions]) => {
				if (isMounted) {
					setUsers(fetchedUsers);
					setPermissions(fetchedPermissions);
				}
			})
			.catch((requestError: unknown) => {
				if (isMounted) {
					setError(requestError instanceof Error ? requestError.message : 'Veriler alınamadı.');
				}
			})
			.finally(() => {
				if (isMounted) {
					setLoading(false);
				}
			});

		return () => {
			isMounted = false;
		};
	}, []);

	const handleSubmit = async (event: SubmitEvent<HTMLFormElement>) => {
		event.preventDefault();
		setError(null);
		setSuccess(null);
		setSubmitting(true);

		try {
			await addPermissionToUser(selectedUserId, selectedPermissionId);
			setSuccess('Yetki kullanıcıya başarıyla eklendi.');
			setSelectedPermissionId('');
		} catch (requestError: unknown) {
			setError(requestError instanceof Error ? requestError.message : 'Yetki eklenemedi.');
		} finally {
			setSubmitting(false);
		}
	};

	if (loading) {
		return <Typography color="text.secondary">Kullanıcılar ve yetkiler yükleniyor...</Typography>;
	}

	return (
		<Box component="form" onSubmit={handleSubmit} className="form-stack">
			<Autocomplete
				options={users}
				getOptionLabel={(user) => `${user.name} ${user.surname} (${user.phone})`}
				value={users.find((user) => user.id === selectedUserId) ?? null}
				onChange={(_, user) => setSelectedUserId(user?.id ?? '')}
				disabled={submitting}
				renderInput={(params) => (
					<TextField
						{...params}
						required
						label="Kullanıcı seç"
					/>
				)}
			/>

			<Autocomplete
				options={permissions}
				getOptionLabel={(permission) => permission.name}
				value={permissions.find((permission) => permission.id === selectedPermissionId) ?? null}
				onChange={(_, permission) => setSelectedPermissionId(permission?.id ?? '')}
				disabled={submitting}
				renderInput={(params) => (
					<TextField
						{...params}
						required
						label="Yetki seç"
					/>
				)}
			/>

			<Button type="submit" variant="contained" disabled={submitting || !selectedUserId || !selectedPermissionId}>
				{submitting ? 'Yetki ekleniyor...' : 'Yetki ekle'}
			</Button>

			{error && <Alert severity="error">{error}</Alert>}
			{success && <Alert severity="success">{success}</Alert>}
		</Box>
	);
}


