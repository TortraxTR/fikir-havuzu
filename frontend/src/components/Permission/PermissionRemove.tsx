import { useEffect, useState, type SubmitEvent } from 'react';
import { Alert, Autocomplete, Box, Button, TextField, Typography } from '@mui/material';
import { fetchAllUsers, fetchUserPermissions, removePermissionFromUser } from '../../api';
import type { Permission } from '../../types/Permission';
import type { User } from '../../types/User';

export default function PermissionRemove() {
    const [users, setUsers] = useState<User[]>([]);
    const [permissions, setPermissions] = useState<Permission[]>([]);
    const [selectedUserId, setSelectedUserId] = useState('');
    const [selectedPermissionId, setSelectedPermissionId] = useState('');
    const [loading, setLoading] = useState(true);
    const [permissionsLoading, setPermissionsLoading] = useState(false);
    const [submitting, setSubmitting] = useState(false);
    const [error, setError] = useState<string | null>(null);
    const [success, setSuccess] = useState<string | null>(null);

    useEffect(() => {
        let isMounted = true;

        fetchAllUsers()
            .then((fetchedUsers) => {
                if (isMounted) setUsers(fetchedUsers);
            })
            .catch((requestError: unknown) => {
                if (isMounted) setError(requestError instanceof Error ? requestError.message : 'Kullanıcılar alınamadı.');
            })
            .finally(() => {
                if (isMounted) setLoading(false);
            });

        return () => {
            isMounted = false;
        };
    }, []);

    const handleUserChange = async (user: User | null) => {
        setSelectedUserId(user?.id ?? '');
        setSelectedPermissionId('');
        setPermissions([]);
        setError(null);

        if (!user) return;

        setPermissionsLoading(true);
        try {
            setPermissions(await fetchUserPermissions(user.id));
        } catch (requestError: unknown) {
            setError(requestError instanceof Error ? requestError.message : 'Kullanıcı yetkileri alınamadı.');
        } finally {
            setPermissionsLoading(false);
        }
    };

    const handleSubmit = async (event: SubmitEvent<HTMLFormElement>) => {
        event.preventDefault();
        setError(null);
        setSuccess(null);
        setSubmitting(true);

        try {
            await removePermissionFromUser(selectedUserId, selectedPermissionId);
            setPermissions((current) => current.filter((permission) => permission.id !== selectedPermissionId));
            setSelectedPermissionId('');
            setSuccess('Yetki kullanıcıdan başarıyla silindi.');
        } catch (requestError: unknown) {
            setError(requestError instanceof Error ? requestError.message : 'Yetki silinemedi.');
        } finally {
            setSubmitting(false);
        }
    };

    if (loading) return <Typography color="text.secondary">Kullanıcılar yükleniyor...</Typography>;

    return (
        <Box component="form" onSubmit={handleSubmit} sx={{ display: 'grid', gap: 2, width: '100%' }}>
            <Autocomplete
                options={users}
                getOptionLabel={(user) => `${user.name} ${user.surname} (${user.phone})`}
                value={users.find((user) => user.id === selectedUserId) ?? null}
                onChange={(_, user) => void handleUserChange(user)}
                disabled={submitting}
                renderInput={(params) => <TextField {...params} required label="Kullanıcı seç" />}
            />
            <Autocomplete
                options={permissions}
                getOptionLabel={(permission) => permission.name}
                value={permissions.find((permission) => permission.id === selectedPermissionId) ?? null}
                onChange={(_, permission) => setSelectedPermissionId(permission?.id ?? '')}
                loading={permissionsLoading}
                disabled={submitting || !selectedUserId}
                renderInput={(params) => <TextField {...params} required label="Silinecek yetkiyi seç" />}
            />
            <Button type="submit" variant="contained" color="error" disabled={submitting || !selectedUserId || !selectedPermissionId}>
                {submitting ? 'Yetki siliniyor...' : 'Yetkiyi sil'}
            </Button>
            {error && <Alert severity="error">{error}</Alert>}
            {success && <Alert severity="success">{success}</Alert>}
        </Box>
    );
}


