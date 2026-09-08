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
        <TableContainer component={Paper} elevation={0} className="user-table">
            <Table aria-label="Kullanıcı listesi" className="user-table-content">
                <TableHead>
                    <TableRow>
                        <TableCell>Ad Soyad</TableCell>
                        <TableCell>E-posta</TableCell>
                        <TableCell>Telefon</TableCell>
                        <TableCell className="mobile-hidden-sm">Sicil No.</TableCell>
                        <TableCell className="mobile-hidden-md">T.C. Kimlik No.</TableCell>
                        <TableCell>Durum</TableCell>
                    </TableRow>
                </TableHead>
                <TableBody>
                    {users.map((user) => (
                        <TableRow key={user.id} hover>
                            <TableCell>{user.name} {user.surname}</TableCell>
                            <TableCell>{user.email}</TableCell>
                            <TableCell>{user.phone}</TableCell>
                            <TableCell className="mobile-hidden-sm">{user.registrationNo}</TableCell>
                            <TableCell className="mobile-hidden-md">{user.governmentId}</TableCell>
                            <TableCell>
                                <Chip
                                    size="small"
                                    label={user.isActive ? 'Aktif' : 'İnaktif'}
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