import { Visibility, VisibilityOff } from '@mui/icons-material';
import ArrowForwardIcon from '@mui/icons-material/ArrowForward';
import {
  Alert,
  Box,
  Button,
  Checkbox,
  Container,
  FormControlLabel,
  IconButton,
  InputAdornment,
  Paper,
  TextField,
  Typography,
} from '@mui/material';
import { login } from '../api';
import { type ChangeEvent, type SubmitEvent, useState } from 'react';
import { useNavigate } from 'react-router-dom';

type LoginForm = {
  phoneNumber: string;
  password: string;
  remember: boolean;
};



export default function LoginPage() {
  const navigate = useNavigate();
  const [form, setForm] = useState<LoginForm>({
    phoneNumber: '',
    password: '',
    remember: true,
  });
  const [showPassword, setShowPassword] = useState(false);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const handleChange = (event: ChangeEvent<HTMLInputElement>) => {
    const { name, value, checked, type } = event.target;
    setForm((current) => ({
      ...current,
      [name]: type === 'checkbox' ? checked : value,
    }));
  };

  const handleSubmit = async (event: SubmitEvent<HTMLFormElement>) => {
    event.preventDefault();

    setLoading(true);
    setError(null);

    try {
      const user = await login(form.phoneNumber, form.password);

      console.log('Logged-in user:', user);

      if (form.remember) {
        localStorage.setItem('user', JSON.stringify(user));
      }

      navigate('/landing'); // Redirect to the landing page after successful login
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Login failed');
    } finally {
      setLoading(false);
    }
  };

  return (
    <Box sx={{ minHeight: '100vh', display: 'grid', placeItems: 'center', bgcolor: '#f7f4ee', p: { xs: 2, sm: 4 } }}>
      <Container maxWidth="sm" sx={{ display: 'flex', justifyContent: 'center' }}>
        <Paper elevation={0} component="section" sx={{ width: '100%', maxWidth: 440, p: { xs: 3, sm: 5 }, border: '1px solid #e5e0d8', borderRadius: 2, bgcolor: '#fffdf9', boxShadow: 'rgba(45, 38, 31, 0.12) 0 18px 42px -18px' }}>
          <Box component="form" onSubmit={handleSubmit} noValidate>
            <Typography sx={{ mb: 1, color: '#d85c43', fontWeight: 800, letterSpacing: '0.04em' }}>
              FİKİR HAVUZU
            </Typography>
            <Typography component="h1" sx={{ mb: 1, fontFamily: 'Georgia', fontSize: '2.5rem', color: '#20201d' }}>
              Giriş yap
            </Typography>

            <Box sx={{ display: 'grid', gap: 2.5 }}>
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

            <Box sx={{ display: 'flex', alignItems: 'center', justifyContent: 'space-between', mt: 1, mb: 3 }}>
              <FormControlLabel
                control={<Checkbox name="remember" checked={form.remember} onChange={handleChange} size="small" />}
                label={<Typography variant="body2">Beni hatırla</Typography>}
              />
            </Box>

            <Button
              type="submit"
              fullWidth
              variant="contained"
              size="large"
              disabled={loading}
              endIcon={!loading && <ArrowForwardIcon />}
              sx={{ minHeight: 52, borderRadius: 1, bgcolor: '#d85c43', textTransform: 'none', fontWeight: 800, boxShadow: 'none', '&:hover': { bgcolor: '#bc4934', boxShadow: 'none' } }}
            >
              {loading ? 'Giriş yapılıyor...' : 'Giriş yap'}
            </Button>

            {error && <Alert severity="error" sx={{ mt: 2 }}>{error}</Alert>}
          </Box>
        </Paper>
      </Container>
    </Box>
  );
}
