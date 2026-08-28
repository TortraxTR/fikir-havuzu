import { useEffect, useState } from 'react';
import {
    Box,
    Container,
    Paper,
    Typography,
} from '@mui/material';

import TaskAltOutlinedIcon from '@mui/icons-material/TaskAltOutlined';
import RateReviewIcon from '@mui/icons-material/RateReviewOutlined';
import ManageAccountIcon from '@mui/icons-material/ManageAccountsOutlined';
import LandingHeader from '../components/LandingHeader';
import LandingStatus from '../components/LandingStatus';
import LandingWorkspace, { type LandingTab } from '../components/LandingWorkspace';
import { useNavigate } from 'react-router-dom';
import { fetchUserPermissions } from '../api';

type LoggedInUser = {
    id: string;
    name: string;
    surname: string;
    permissions?: string[];
};

const availableTabs: LandingTab[] = [
    {
        label: 'Kullanıcı Yönetimi',
        permission: 'KullaniciYonetimi',
        description: 'Yeni kullanıcılar ekle ve mevcut kullanıcıları düzenle.',
        icon: ManageAccountIcon,
        accent: '#d85c43',
    },
    {
        label: 'Fikir ve Öneri Yönetimi',
        permission: 'Fikiryonetimi',
        description: 'Yeni fikir ve öneri oluştur, mevcut fikir ve önerileri incele ve değerlendir.',
        icon: RateReviewIcon,
        accent: '#237a73',
    },
    {
        label: 'Yetki Yönetimi',
        permission: 'YetkiYonetimi',
        description: 'Kullanıcı yetkilerini yönet.',
        icon: TaskAltOutlinedIcon,
        accent: '#364f7d',
    },
];

function getLoggedInUser(): LoggedInUser | null {
    const savedUser = localStorage.getItem('user');

    if (!savedUser) {
        return null;
    }

    try {
        return JSON.parse(savedUser) as LoggedInUser;
    } catch {
        localStorage.removeItem('user');
        return null;
    }
}

export default function LandingPage() {
    const navigate = useNavigate();
    const [user] = useState<LoggedInUser | null>(() => getLoggedInUser());
    const [selectedTab, setSelectedTab] = useState(0);
    const [permissions, setPermissions] = useState<string[]>(user?.permissions ?? []);
    const [permissionsLoading, setPermissionsLoading] = useState(
        Boolean(user && !user.permissions?.length),
    );
    const [permissionsError, setPermissionsError] = useState<string | null>(null);

    useEffect(() => {
        if (!user) {
            return;
        }

        let isMounted = true;

        fetchUserPermissions(user.id)
            .then((userPermissions) => {
                if (isMounted) {
                    if (userPermissions.length > 0) {
                        setPermissions(userPermissions.map((permission) => permission.name));
                    }
                }
            })
            .catch((error: unknown) => {
                if (isMounted) {
                    setPermissionsError(error instanceof Error ? error.message : 'Yetkiler alınamadı.');
                }
            })
            .finally(() => {
                if (isMounted) {
                    setPermissionsLoading(false);
                }
            });

        return () => {
            isMounted = false;
        };
    }, [user]);

    const permissionNames = new Set(
        permissions.map((permission) => permission.trim().toLowerCase()),
    );

    const tabs = availableTabs.filter((tab) =>
        tab.permission && permissionNames.has(tab.permission.trim().toLowerCase()),
    );

    if (!user) {
        return (
            <Container maxWidth={false} sx={{ py: 10 }}>
                <Paper sx={{ p: { xs: 4, sm: 7 }, borderRadius: 2 }} component="section">
                    <Typography variant="h3" gutterBottom>Oturum bulunamadı</Typography>
                    <Typography color="text.secondary">Devam etmek için giriş yapmalısın.</Typography>
                </Paper>
            </Container>
        );
    }

    return (
        <Box sx={{ height: '100svh', boxSizing: 'border-box', overflow: 'hidden', bgcolor: '#f7f4ee', py: { xs: 2, md: 4 } }}>
            <Container maxWidth={false} sx={{ height: '100%', display: 'flex', flexDirection: 'column' }}>
                <LandingHeader onLogout={() => { localStorage.removeItem('user'); navigate('/'); }} />

                <Box sx={{ mb: { xs: 2, md: 4 } }}>
                    <Typography component="h1" sx={{ mt: 1, mb: 1, fontFamily: 'Georgia', fontSize: { xs: '2.25rem', md: '4.25rem' }, lineHeight: 1.05, color: '#20201d' }}>
                        Hoş geldin, {user.name}.
                    </Typography>
                </Box>

                <Paper elevation={0} component="section" sx={{ overflow: 'hidden', border: '1px solid #e5e0d8', borderRadius: 2, bgcolor: '#fffdf9' }}>
                    {permissionsLoading && <LandingStatus message="Yetkiler yükleniyor..." />}

                    {permissionsError && <LandingStatus message={permissionsError} error />}

                    {!permissionsLoading && !permissionsError && (
                        <>
                    {tabs.length > 0 && <LandingWorkspace tabs={tabs} selectedTab={selectedTab} onTabChange={setSelectedTab} />}

                    {tabs.length === 0 && (
                        <Box sx={{ p: { xs: 3, md: 5 } }}>
                            <Typography component="h2" sx={{ mb: 1 }}>Henüz atanmış yetki yok</Typography>
                            <Typography color="text.secondary">Çalışma alanlarını görmek için bir yetki atanmasını bekle.</Typography>
                        </Box>
                    )}
                        </>
                    )}
                </Paper>
            </Container>
        </Box>
    );
}