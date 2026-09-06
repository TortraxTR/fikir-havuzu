import ArrowForwardIcon from '@mui/icons-material/ArrowForward';
import { Avatar, Box, Button, Divider, Tab, Tabs, Typography } from '@mui/material';
import { useState, type ComponentType } from 'react';
import UserList from '../User/UserList';
import UserSetActive from '../User/UserSetActive';
import UserAdd from '../User/UserAdd';
import PermissionAdd from '../Permission/PermissionAdd';
import PermissionRemove from '../Permission/PermissionRemove';
import UserUpdate from '../User/UserUpdate';
import ProposalList from '../Proposal/ProposalList';

export type LandingTab = {
    label: string;
    permission?: string;
    description: string;
    icon: ComponentType;
    accent: string;
};

type WorkspaceOption = {
    label: string;
    content?: ComponentType;
};

type LandingWorkspaceProps = {
    tabs: LandingTab[];
    selectedTab: number;
    onTabChange: (tab: number) => void;
};

function getWorkspaceOptions(tab: LandingTab): WorkspaceOption[] {
    if (tab.permission === 'KullaniciYonetimi') {
        return [
            { label: 'Kullanıcıları listele', content: UserList },
            { label: 'Kullanıcıyı aktive/deaktive et', content: UserSetActive },
            { label: 'Yeni kullanıcı ekle', content: UserAdd },
            { label: 'Kullanıcı bilgilerini güncelle', content: UserUpdate },
        ];
    }

    if (tab.permission === 'Fikiryonetimi') {
        return [
            { label: 'Fikirleri/Önerileri listele', content: ProposalList },
        ];
    }

    if (tab.permission === 'YetkiYonetimi') {
        return [
            { label: 'Kullanıcıya yetki ekle', content: PermissionAdd },
            { label: 'Kullanıcıdan yetki sil', content: PermissionRemove },
        ];
    }

    return [{ label: 'Genel bakış' }];
}

export default function LandingWorkspace({ tabs, selectedTab, onTabChange }: LandingWorkspaceProps) {
    const tab = tabs[selectedTab];
    const Icon = tab.icon;
    const options = getWorkspaceOptions(tab);
    const [selectedOption, setSelectedOption] = useState(0);
    const option = options[selectedOption];
    const OptionContent = option.content;

    const handleTabChange = (nextTab: number) => {
        onTabChange(nextTab);
        setSelectedOption(0);
    };

    return (
        <>
            <Tabs
                value={selectedTab}
                onChange={(_, value: number) => handleTabChange(value)}
                variant="fullWidth"
                scrollButtons={false}
                aria-label="Kullanıcı çalışma alanları"
                sx={{ px: { xs: 0, md: 3 }, borderBottom: '1px solid #e5e0d8', '& .MuiTabs-flexContainer': { width: '100%' }, '& .MuiTab-root': { minWidth: 0, minHeight: { xs: 58, md: 72 }, px: { xs: 0.5, md: 2 }, textTransform: 'none', fontWeight: 700, fontSize: { xs: '0.68rem', sm: '0.8rem', md: '0.875rem' }, lineHeight: 1.2, whiteSpace: 'normal' }, '& .MuiTab-iconWrapper': { mr: { xs: 0.5, md: 1 } } }}
            >
                {tabs.map((workspaceTab) => {
                    const WorkspaceIcon = workspaceTab.icon;
                    return <Tab key={workspaceTab.label} icon={<WorkspaceIcon />} iconPosition="start" label={workspaceTab.label} />;
                })}
            </Tabs>

            <Box className="landing-tab-panel" role="tabpanel" sx={{ p: { xs: 3, md: 5 } }}>
                <Box sx={{ display: 'flex', flexDirection: { xs: 'column', sm: 'row' }, gap: 3 }}>
                    <Avatar sx={{ mb: 3, bgcolor: `${tab.accent}18`, color: tab.accent }}><Icon /></Avatar>
                    <Typography component="h2" sx={{ mb: 1, fontSize: '1.8rem', color: '#20201d' }}>{tab.label}</Typography>
                </Box>
                <Typography color="text.secondary">{tab.description}</Typography>
                <Divider sx={{ my: 4 }} />

                <Tabs
                    value={selectedOption}
                    onChange={(_, value: number) => setSelectedOption(value)}
                    variant="scrollable"
                    scrollButtons="auto"
                    aria-label={`${tab.label} seçenekleri`}
                    sx={{ mb: 3, borderBottom: '1px solid #e5e0d8' }}
                >
                    {options.map((workspaceOption) => (
                        <Tab key={workspaceOption.label} label={workspaceOption.label} sx={{ textTransform: 'none' }} />
                    ))}
                </Tabs>

                {OptionContent ? (
                    <OptionContent />
                ) : (
                    <Box sx={{ py: 2 }}>
                        <Typography color="text.secondary">Bu seçenek henüz kullanıma hazır değil.</Typography>
                        <Button variant="contained" endIcon={<ArrowForwardIcon />} sx={{ mt: 3, bgcolor: tab.accent, textTransform: 'none', boxShadow: 'none', '&:hover': { bgcolor: tab.accent, filter: 'brightness(0.9)', boxShadow: 'none' } }}>
                            Çalışma alanını aç
                        </Button>
                    </Box>
                )}
            </Box>
        </>
    );
}
