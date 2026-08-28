import { Box, Typography } from '@mui/material';

type LandingStatusProps = {
    message: string;
    error?: boolean;
};

export default function LandingStatus({ message, error = false }: LandingStatusProps) {
    return (
        <Box sx={{ p: { xs: 3, md: 5 } }}>
            <Typography color={error ? 'error' : 'text.secondary'}>{message}</Typography>
        </Box>
    );
}