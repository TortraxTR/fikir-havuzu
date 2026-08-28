import LogoutIcon from '@mui/icons-material/Logout';
import { Box, Button, Typography } from '@mui/material';

type LandingHeaderProps = {
    onLogout: () => void;
};

export default function LandingHeader({ onLogout }: LandingHeaderProps) {
    return (
        <Box sx={{ display: 'flex', flexDirection: 'row', justifyContent: 'space-between', alignItems: 'center', mb: { xs: 3, md: 6 } }}>
            <Box sx={{ display: 'flex', flexDirection: 'row', gap: 1.5, alignItems: 'center' }}>
                <Box sx={{ width: 38, height: 38, display: 'grid', placeItems: 'center', bgcolor: '#d85c43', color: 'white', borderRadius: '12px 12px 12px 3px', fontFamily: 'Georgia', fontSize: 23, fontWeight: 700 }}>F</Box>
                <Typography sx={{ fontWeight: 800, letterSpacing: '0.04em', color: '#20201d' }}>FİKİR HAVUZU</Typography>
            </Box>
            <Button
                color="inherit"
                startIcon={<LogoutIcon />}
                onClick={onLogout}
                sx={{ textTransform: 'none', color: '#635f5a' }}
            >
                Çıkış yap
            </Button>
        </Box>
    );
}