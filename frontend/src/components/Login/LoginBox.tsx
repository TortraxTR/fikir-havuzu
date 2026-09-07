import { VisibilityOff, Visibility } from '@mui/icons-material';
import { InputAdornment, Box, TextField, IconButton } from '@mui/material';
import type { ChangeEvent, Dispatch, SetStateAction } from 'react';

type LoginBoxProps = {
    form: {
        phoneNumber: string;
        password: string;
    };
    handleChange: (event: ChangeEvent<HTMLInputElement>) => void;
    showPassword: boolean;
    setShowPassword: Dispatch<SetStateAction<boolean>>;
};

export default function LoginBox({ form, handleChange, showPassword, setShowPassword }: LoginBoxProps) {
    return (
        <Box className="login-form-fields">
        <TextField
            fullWidth
            required
            label="Telefon Numarası"
            name="phoneNumber"
            type="text"
            value={form.phoneNumber}
            onChange={handleChange}
            autoComplete="tel"
            placeholder="5XX XXX XX XX"
        />
        <TextField
            fullWidth
            required
            label="Şifre"
            name="password"
            type={showPassword ? 'text' : 'password'}
            value={form.password}
            onChange={handleChange}
            autoComplete="current-password"
            slotProps={{
                input: {
                    endAdornment: (
                        <InputAdornment position="end">
                            <IconButton
                                aria-label={showPassword ? 'Şifreyi gizle' : 'Şifreyi göster'}
                                onClick={() => setShowPassword((visible) => !visible)}
                                edge="end"
                            >
                                {showPassword ? <VisibilityOff /> : <Visibility />}
                            </IconButton>
                        </InputAdornment>
                    ),
                },
            }}
        />
        </Box>
    );
}