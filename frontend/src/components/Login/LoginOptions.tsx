import { Box, Checkbox, FormControlLabel, Typography } from '@mui/material';
import type { ChangeEvent } from 'react';

type LoginOptionsProps = {
    remember: boolean;
    handleChange: (event: ChangeEvent<HTMLInputElement>) => void;
};

export default function LoginOptions({ remember, handleChange }: LoginOptionsProps) {
    return (
        <Box sx={{ display: 'flex', alignItems: 'center', justifyContent: 'space-between', mt: 1, mb: 3 }}>
            <FormControlLabel
                control={<Checkbox name="remember" checked={remember} onChange={handleChange} size="small" />}
                label={<Typography variant="body2">Beni hatırla</Typography>}
            />
        </Box>
    );
}