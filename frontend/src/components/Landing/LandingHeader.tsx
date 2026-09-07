import LogoutIcon from '@mui/icons-material/Logout';
import { Box, Button, Typography } from '@mui/material';

type LandingHeaderProps = {
    onLogout: () => void;
};

export default function LandingHeader({ onLogout }: LandingHeaderProps) {
    return (
        <Box className="landing-header">
            <Box className="landing-brand">
                <Box className="landing-mark">F</Box>
                <Typography className="landing-brand-name">FİKİR HAVUZU</Typography>
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