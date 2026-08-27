import { useEffect, useState } from 'react';
import {
    Avatar,
    Box,
    Button,
    Container,
    Divider,
    Paper,
    Tab,
    Tabs,
    Typography,
} from '@mui/material';

import AddIdeaIcon from '@mui/icons-material/LightbulbOutlined';
import ArrowForwardIcon from '@mui/icons-material/ArrowForward';
import TaskAltOutlinedIcon from '@mui/icons-material/TaskAltOutlined';
import LogoutIcon from '@mui/icons-material/Logout';
import RateReviewIcon from '@mui/icons-material/RateReviewOutlined';
import ManageAccountIcon from '@mui/icons-material/ManageAccountsOutlined';
import { useNavigate } from 'react-router-dom';
import { fetchUserPermissions } from '../api';

type LoggedInUser = {
    id: string;
    name: string;
    surname: string;
    permissions?: string[];
};

type LandingTab = {
    label: string;
    permission?: string;
    description: string;
    icon: typeof AddIdeaIcon;
    accent: string;
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
            <Container maxWidth="md" sx={{ py: 10 }}>
                <Paper sx={{ p: { xs: 4, sm: 7 }, borderRadius: 2 }} component="section">
                    <Typography variant="h3" gutterBottom>Oturum bulunamadı</Typography>
                    <Typography color="text.secondary">Devam etmek için giriş yapmalısın.</Typography>
                </Paper>
            </Container>
        );
    }

    return (
        <Box sx={{ minHeight: '100vh', bgcolor: '#f7f4ee', py: { xs: 3, md: 6 } }}>
            <Container maxWidth="lg">
                <Box sx={{ display: 'flex', flexDirection: 'row', justifyContent: 'space-between', alignItems: 'center', mb: { xs: 6, md: 10 } }}>
                    <Box sx={{ display: 'flex', flexDirection: 'row', gap: 1.5, alignItems: 'center' }}>
                        <Box sx={{ width: 38, height: 38, display: 'grid', placeItems: 'center', bgcolor: '#d85c43', color: 'white', borderRadius: '12px 12px 12px 3px', fontFamily: 'Georgia', fontSize: 23, fontWeight: 700 }}>F</Box>
                        <Typography sx={{ fontWeight: 800, letterSpacing: '0.04em', color: '#20201d' }}>FİKİR HAVUZU</Typography>
                    </Box>
                    <Button
                        color="inherit"
                        startIcon={<LogoutIcon />}
                        onClick={() => { localStorage.removeItem('user'); navigate('/login'); }}
                        sx={{ textTransform: 'none', color: '#635f5a' }}
                    >
                        Çıkış yap
                    </Button>
                </Box>

                <Box sx={{ mb: 5 }}>
                    <Typography component="h1" sx={{ mt: 1, mb: 1, fontFamily: 'Georgia', fontSize: { xs: '2.5rem', md: '4.25rem' }, lineHeight: 1.05, color: '#20201d' }}>
                        Hoş geldin, {user.name}.
                    </Typography>
                </Box>

                <Paper elevation={0} component="section" sx={{ overflow: 'hidden', border: '1px solid #e5e0d8', borderRadius: 2, bgcolor: '#fffdf9' }}>
                    {permissionsLoading && (
                        <Box sx={{ p: { xs: 3, md: 5 } }}>
                            <Typography color="text.secondary">Yetkiler yükleniyor...</Typography>
                        </Box>
                    )}

                    {permissionsError && (
                        <Box sx={{ p: { xs: 3, md: 5 } }}>
                            <Typography color="error">{permissionsError}</Typography>
                        </Box>
                    )}

                    {!permissionsLoading && !permissionsError && (
                        <>
                    <Tabs
                        value={Math.min(selectedTab, Math.max(tabs.length - 1, 0))}
                        onChange={(_, value: number) => setSelectedTab(value)}
                        variant="scrollable"
                        scrollButtons="auto"
                        aria-label="Kullanıcı çalışma alanları"
                        sx={{ px: { xs: 1, md: 3 }, borderBottom: '1px solid #e5e0d8', '& .MuiTab-root': { minHeight: 72, textTransform: 'none', fontWeight: 700 } }}
                    >
                        {tabs.map((tab) => {
                            const Icon = tab.icon;
                            return <Tab key={tab.label} icon={<Icon />} iconPosition="start" label={tab.label} />;
                        })}
                    </Tabs>

                    {tabs.length > 0 && (
                        <Box className="landing-tab-panel" role="tabpanel" sx={{ p: { xs: 3, md: 5 } }}>
                            {(() => {
                                const tab = tabs[selectedTab];
                                const Icon = tab.icon;
                                return (
                                    <>
                                        <Box sx={{ display: 'flex', flexDirection: { xs: 'column', sm: 'row' }, justifyContent: 'normal' , gap: 3 }}>
                                            <Avatar sx={{ mb: 3, bgcolor: `${tab.accent}18`, color: tab.accent }}><Icon /></Avatar>
                                            <Typography component="h2" sx={{ mb: 1, fontSize: '1.8rem', color: '#20201d' }}>{tab.label}</Typography>
                                        </Box>
                                        <Typography color="text.secondary" sx={{ maxWidth: 520 }}>{tab.description}</Typography>
                                        <Divider sx={{ my: 4 }} />
                                        <Box sx={{ display: 'flex', flexDirection: { xs: 'column', sm: 'row' }, gap: 2 }}>
                                            <Button variant="contained" endIcon={<ArrowForwardIcon />} sx={{ bgcolor: tab.accent, textTransform: 'none', boxShadow: 'none', '&:hover': { bgcolor: tab.accent, filter: 'brightness(0.9)', boxShadow: 'none' } }}>
                                                Çalışma alanını aç
                                            </Button>
                                        </Box>
                                    </>
                                );
                            })()}
                        </Box>
                    )}

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