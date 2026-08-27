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
import { login } from '../api/auth';
import { type ChangeEvent, type FormEvent, useState } from 'react';

type LoginForm = {
  phoneNumber: string;
  password: string;
  remember: boolean;
};



export default function LoginPage() {
  const [form, setForm] = useState<LoginForm>({
    phoneNumber: '',
    password: '',
    remember: true,
  });
  const [showPassword, setShowPassword] = useState(false);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [success, setSuccess] = useState(false);

  const handleChange = (event: ChangeEvent<HTMLInputElement>) => {
    const { name, value, checked, type } = event.target;
    setForm((current) => ({
      ...current,
      [name]: type === 'checkbox' ? checked : value,
    }));
  };

  const handleSubmit = async (event: FormEvent<HTMLFormElement>) => {
    event.preventDefault();
  
    setLoading(true);
    setError(null);
    setSuccess(false);
  
    try {
      const user = await login(form.phoneNumber, form.password);
  
      console.log('Logged-in user:', user);
  
      if (form.remember) {
        localStorage.setItem('user', JSON.stringify(user));
      }
  
      setSuccess(true);
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Login failed');
    } finally {
      setLoading(false);
    }
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

          <Box className="login-options">
            <FormControlLabel
              control={<Checkbox name="remember" checked={form.remember} onChange={handleChange} />}
              label="Beni hatırla"
            />
            <Link href="#forgot-password" underline="hover">Şifremi unuttum</Link>
          </Box>

          <Button
            type="submit"
            fullWidth
            variant="contained"
            size="large"
            disabled={loading}
          >
            {loading ? 'Giriş yapılıyor...' : 'Giriş yap'}
          </Button>

          {error && (
            <Alert severity="error" className="login-alert">
              {error}
            </Alert>
          )}
          
          {success && (
            <Alert severity="success" className="login-alert">
              Giriş başarılı.
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
