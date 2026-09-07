import { Box, Typography } from '@mui/material';

export default function LoginHeader() {
    return (
        <Box>
            <Typography className="login-heading-eyebrow">
                Fikir Havuzu
            </Typography>
            <Typography className="login-heading" component="h1">
                Giriş yap
            </Typography>
        </Box>
    );
}