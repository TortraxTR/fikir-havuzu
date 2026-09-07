import { Box, Typography } from '@mui/material';

type LandingStatusProps = {
    message: string;
    error?: boolean;
};

export default function LandingStatus({ message, error = false }: LandingStatusProps) {
    return (
        <Box className="status-box">
            <Typography color={error ? 'error' : 'text.secondary'}>{message}</Typography>
        </Box>
    );
}