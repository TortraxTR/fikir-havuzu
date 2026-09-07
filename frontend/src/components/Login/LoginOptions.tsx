import { Box, Checkbox, FormControlLabel, Typography } from '@mui/material';
import type { ChangeEvent } from 'react';

type LoginOptionsProps = {
    remember: boolean;
    handleChange: (event: ChangeEvent<HTMLInputElement>) => void;
};

export default function LoginOptions({ remember, handleChange }: LoginOptionsProps) {
    return (
        <Box className="login-options">
            <FormControlLabel
                control={<Checkbox name="remember" checked={remember} onChange={handleChange} size="small" />}
                label={<Typography variant="body2">Beni hatırla</Typography>}
            />
        </Box>
    );
}