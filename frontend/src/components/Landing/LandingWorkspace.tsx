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
import ProposalCreate from '../Proposal/ProposalCreate';

export type LandingTab = {
    label: string;
    permission?: string;
    description: string;
    icon: ComponentType;
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

    if (tab.permission === 'FikirYonetimi') {
        return [
            { label: 'Yeni fikir/öneri oluştur', content: ProposalCreate },
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
                className="landing-tabs"
            >
                {tabs.map((workspaceTab) => {
                    const WorkspaceIcon = workspaceTab.icon;
                    return <Tab key={workspaceTab.label} icon={<WorkspaceIcon />} iconPosition="start" label={workspaceTab.label} />;
                })}
            </Tabs>

            <Box className="landing-tab-panel" role="tabpanel">
                <Box className="workspace-heading">
                    <Avatar className={`workspace-avatar ${tab.permission === 'FikirYonetimi' ? 'workspace-avatar-proposals' : tab.permission === 'YetkiYonetimi' ? 'workspace-avatar-permissions' : 'workspace-avatar-users'}`}><Icon /></Avatar>
                    <Typography className="workspace-title" component="h2">{tab.label}</Typography>
                </Box>
                <Typography color="text.secondary">{tab.description}</Typography>
                <Divider className="workspace-divider" />

                <Tabs
                    value={selectedOption}
                    onChange={(_, value: number) => setSelectedOption(value)}
                    variant="scrollable"
                    scrollButtons="auto"
                    aria-label={`${tab.label} seçenekleri`}
                    className="workspace-options"
                >
                    {options.map((workspaceOption) => (
                        <Tab key={workspaceOption.label} label={workspaceOption.label} className="workspace-option-tab" />
                    ))}
                </Tabs>

                {OptionContent ? (
                    <OptionContent />
                ) : (
                    <Box className="workspace-placeholder">
                        <Typography color="text.secondary">Bu seçenek henüz kullanıma hazır değil.</Typography>
                        <Button variant="contained" endIcon={<ArrowForwardIcon />} className={`workspace-action ${tab.permission === 'FikirYonetimi' ? 'workspace-action-proposals' : tab.permission === 'YetkiYonetimi' ? 'workspace-action-permissions' : 'workspace-action-users'}`}>
                            Çalışma alanını aç
                        </Button>
                    </Box>
                )}
            </Box>
        </>
    );
}
