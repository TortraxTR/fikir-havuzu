import { Visibility, VisibilityOff } from '@mui/icons-material';
import {
  Alert,
  Box,
  Button,
  Checkbox,
  FormControlLabel,
  IconButton,
  InputAdornment,
  Link,
  Paper,
  TextField,
  Typography,
} from '@mui/material';
import { type ChangeEvent, type FormEvent, useState } from 'react';

type LoginForm = {
  email: string;
  password: string;
  remember: boolean;
};

export default function LoginPage() {
  const [form, setForm] = useState<LoginForm>({
    email: '',
    password: '',
    remember: true,
  });
  const [showPassword, setShowPassword] = useState(false);
  const [submitted, setSubmitted] = useState(false);

  const handleChange = (event: ChangeEvent<HTMLInputElement>) => {
    const { name, value, checked, type } = event.target;
    setForm((current) => ({
      ...current,
      [name]: type === 'checkbox' ? checked : value,
    }));
    setSubmitted(false);
  };

  const handleSubmit = (event: FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    setSubmitted(true);
  };

  return (
    <Box className="login-page">
      <Paper className="login-card" elevation={0} component="section">
        <Box component="form" onSubmit={handleSubmit} noValidate>
          <Typography component="h2">Fikir Havuzu</Typography>

          <Box className="login-fields">
            <TextField
              fullWidth
              required
              label="E-posta adresi"
              name="email"
              type="email"
              value={form.email}
              onChange={handleChange}
              autoComplete="email"
              placeholder="ornek@sirket.com"
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

          <Box className="login-options">
            <FormControlLabel
              control={<Checkbox name="remember" checked={form.remember} onChange={handleChange} />}
              label="Beni hatırla"
            />
            <Link href="#forgot-password" underline="hover">Şifremi unuttum</Link>
          </Box>

          <Button type="submit" fullWidth variant="contained" size="large">
            Giriş yap
          </Button>

          {submitted && (
            <Alert severity="info" className="login-alert">
              Giriş servisi bağlantısı hazır olduğunda burada devam edeceğiz.
            </Alert>
          )}

          <Typography className="login-signup">
            Henüz hesabın yok mu? <Link href="#register" underline="hover">Kayıt ol</Link>
          </Typography>
        </Box>
      </Paper>
    </Box>
  );
}
