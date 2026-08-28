import { useEffect, useState } from 'react';
import {
    Alert,
    Chip,
    Paper,
    Table,
    TableBody,
    TableCell,
    TableContainer,
    TableHead,
    TableRow,
    Typography,
} from '@mui/material';

import type { User } from '../../types/User';
import { fetchAllUsers } from '../../api';

export default function UserList() {
    const [users, setUsers] = useState<User[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

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

    if (loading) {
        return <Typography color="text.secondary">Kullanıcılar yükleniyor...</Typography>;
    }

    if (error) {
        return <Alert severity="error">{error}</Alert>;
    }

    if (users.length === 0) {
        return <Typography color="text.secondary">Henüz kullanıcı bulunmuyor.</Typography>;
    }

    return (
        <TableContainer component={Paper} elevation={0} sx={{ width: '100%', border: '1px solid #e5e0d8' }}>
            <Table aria-label="Kullanıcı listesi" sx={{ minWidth: 0 }}>
                <TableHead>
                    <TableRow>
                        <TableCell>Ad Soyad</TableCell>
                        <TableCell>Telefon</TableCell>
                        <TableCell sx={{ display: { xs: 'none', sm: 'table-cell' } }}>Sicil No.</TableCell>
                        <TableCell sx={{ display: { xs: 'none', md: 'table-cell' } }}>T.C. Kimlik No.</TableCell>
                        <TableCell>Durum</TableCell>
                    </TableRow>
                </TableHead>
                <TableBody>
                    {users.map((user) => (
                        <TableRow key={user.id} hover>
                            <TableCell>{user.name} {user.surname}</TableCell>
                            <TableCell>{user.phone}</TableCell>
                            <TableCell sx={{ display: { xs: 'none', sm: 'table-cell' } }}>{user.registrationNo}</TableCell>
                            <TableCell sx={{ display: { xs: 'none', md: 'table-cell' } }}>{user.governmentId}</TableCell>
                            <TableCell>
                                <Chip
                                    size="small"
                                    label={user.isActive ? 'Aktif' : 'Deaktif'}
                                    color={user.isActive ? 'success' : 'default'}
                                />
                            </TableCell>
                        </TableRow>
                    ))}
                </TableBody>
            </Table>
        </TableContainer>
    );
}