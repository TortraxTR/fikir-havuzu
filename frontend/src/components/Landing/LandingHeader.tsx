import LogoutIcon from '@mui/icons-material/Logout';
import MenuBookOutlinedIcon from '@mui/icons-material/MenuBookOutlined';
import { Box, Button, Typography } from '@mui/material';

type LandingHeaderProps = {
    onLogout: () => void;
};

export default function LandingHeader({ onLogout }: LandingHeaderProps) {
    return (
        <Box className="landing-header">
            <Box className="landing-brand">
                <Box className="landing-mark"><MenuBookOutlinedIcon /></Box>
                <Typography className="landing-brand-name">Fikir Havuzu</Typography>
            </Box>
            <Button
                color="inherit"
                startIcon={<LogoutIcon />}
                onClick={onLogout}
                className="landing-logout"
            >
                Çıkış yap
            </Button>
        </Box>
    );
}