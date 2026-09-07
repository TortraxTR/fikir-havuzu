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
import LandingHeader from '../components/Landing/LandingHeader';
import LandingStatus from '../components/Landing/LandingStatus';
import LandingWorkspace, { type LandingTab } from '../components/Landing/LandingWorkspace';
import { useNavigate } from 'react-router-dom';
import { fetchUserPermissions } from '../api';

type LoggedInUser = {
    id: string;
    name: string;
    surname: string;
    permissions?: string[];
};

const PERMISSIONS = {
    userManagement: 'USER_MANAGEMENT',
    permissionManagement: 'PERMISSION_MANAGEMENT',
    proposalCreate: 'PROPOSAL_CREATE',
    evaluationCreate: 'EVALUATION_CREATE',
} as const;

const availableTabs: LandingTab[] = [
    {
        label: 'Kullanıcı Yönetimi',
        permission: PERMISSIONS.userManagement,
        description: 'Yeni kullanıcılar ekle ve mevcut kullanıcıları düzenle.',
        icon: ManageAccountIcon,
    },
    {
        label: 'Fikir ve Öneri',
        permission: PERMISSIONS.proposalCreate,
        description: 'Yeni fikir ve öneri oluştur, mevcut fikir ve önerileri incele ve değerlendir.',
        icon: RateReviewIcon,
    },
    {
        label: 'Yetki Yönetimi',
        permission: PERMISSIONS.permissionManagement,
        description: 'Kullanıcı yetkilerini yönet.',
        icon: TaskAltOutlinedIcon,
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
                        setPermissions(userPermissions.map((permission) => permission.code));
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
        tab.permission && (
            permissionNames.has(tab.permission.trim().toLowerCase()) ||
            (tab.permission === PERMISSIONS.proposalCreate && permissionNames.has(PERMISSIONS.evaluationCreate.toLowerCase()))
        ),
    );

    if (!user) {
        return (
            <Container className="page-container" maxWidth={false}>
                <Paper className="login-paper" component="section">
                    <Typography variant="h3" gutterBottom>Oturum bulunamadı</Typography>
                    <Typography color="text.secondary">Devam etmek için giriş yapmalısın.</Typography>
                </Paper>
            </Container>
        );
    }

    return (
        <Box className="page-shell landing-page-shell">
            <Container className="page-container landing-container" maxWidth={false}>
                <LandingHeader onLogout={() => { localStorage.removeItem('user'); navigate('/'); }} />

                <Box className="landing-greeting-wrap">
                    <Typography className="landing-greeting" component="h1">
                        Hoş geldin, {user.name}.
                    </Typography>
                </Box>

                <Paper className="landing-paper" elevation={0} component="section">
                    {permissionsLoading && <LandingStatus message="Yetkiler yükleniyor..." />}

                    {permissionsError && <LandingStatus message={permissionsError} error />}

                    {!permissionsLoading && !permissionsError && (
                        <>
                    {tabs.length > 0 && <LandingWorkspace tabs={tabs} selectedTab={selectedTab} onTabChange={setSelectedTab} />}

                    {tabs.length === 0 && (
                        <Box className="landing-tab-panel">
                            <Typography component="h2">Henüz atanmış yetki yok</Typography>
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