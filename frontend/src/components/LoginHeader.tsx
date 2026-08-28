import { Box, Typography } from '@mui/material';

export default function LoginHeader() {
    return (
        <Box>
            <Typography sx={{ mb: 1, color: '#d85c43', fontWeight: 800, letterSpacing: '0.04em' }}>
                FİKİR HAVUZU
            </Typography>
            <Typography component="h1" sx={{ mb: 1, fontFamily: 'Georgia', fontSize: '2.5rem', color: '#20201d' }}>
                Giriş yap
            </Typography>
        </Box>
    );
}