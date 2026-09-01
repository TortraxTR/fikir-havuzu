import { useEffect, useState, type SubmitEvent } from 'react';
import { Alert, Autocomplete, Box, Button, TextField, Typography } from '@mui/material';
import { fetchAllUsers, setUserActive } from '../../api';
import type { User } from "../../types/User";

export default function UserSetActive() {
    const [users, setUsers] = useState<User[]>([]);
    const [selectedUserId, setSelectedUserId] = useState('');
    const [loading, setLoading] = useState(true);
    const [submitting, setSubmitting] = useState(false);
    const [error, setError] = useState<string | null>(null);
    const [success, setSuccess] = useState<string | null>(null);

    useEffect(() => {
        let isMounted = true;

        fetchAllUsers()
            .then((fetchedUsers) => {
                if (isMounted) {
                    setUsers(fetchedUsers);
                }
            })
            .catch((requestError: unknown) => {
                if (isMounted) {
                    setError(requestError instanceof Error ? requestError.message : 'Kullanıcılar alınamadı.');
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

    const selectedUser = users.find((user) => user.id === selectedUserId) ?? null;

    const handleSubmit = async (event: SubmitEvent<HTMLFormElement>) => {
        event.preventDefault();
        if (!selectedUser) return;

        setError(null);
        setSuccess(null);
        setSubmitting(true);

        try {
            const updatedUser = await setUserActive(selectedUser.id, !selectedUser.isActive);
            setUsers((currentUsers) => currentUsers.map((user) => (
                user.id === updatedUser.id ? updatedUser : user
            )));
            setSelectedUserId('');
            setSuccess(`Kullanıcı ${updatedUser.isActive ? 'aktive' : 'deaktive'} edildi.`);
        } catch (requestError: unknown) {
            setError(requestError instanceof Error ? requestError.message : 'Kullanıcı durumu güncellenemedi.');
        } finally {
            setSubmitting(false);
        }
    };

    if (loading) {
        return <Typography color="text.secondary">Kullanıcılar yükleniyor...</Typography>;
    }

    return (
        <Box component="form" onSubmit={handleSubmit} sx={{ display: 'grid', gap: 2, width: '100%' }}>
            <Autocomplete
                options={users}
                getOptionLabel={(user) => `${user.name} ${user.surname} (${user.phone})`}
                value={selectedUser}
                onChange={(_, user) => {
                    setSelectedUserId(user?.id ?? '');
                    setError(null);
                    setSuccess(null);
                }}
                disabled={submitting}
                renderOption={(props, user) => (
                    <li {...props} key={user.id}>
                        {user.name} {user.surname} ({user.phone}) - {user.isActive ? 'Aktif' : 'İnaktif'}
                    </li>
                )}
                renderInput={(params) => <TextField {...params} required label="Kullanıcı seç" />}
            />
            {selectedUser && (
                <Typography color="text.secondary">
                    Mevcut durum: {selectedUser.isActive ? 'Aktif' : 'Aktif değil'}
                </Typography>
            )}
            <Button type="submit" variant="contained" disabled={submitting || !selectedUser}>
                {submitting ? 'Durum güncelleniyor...' : selectedUser?.isActive ? 'Deaktive et' : 'Aktive et'}
            </Button>
            {error && <Alert severity="error">{error}</Alert>}
            {success && <Alert severity="success">{success}</Alert>}
        </Box>
    );
}