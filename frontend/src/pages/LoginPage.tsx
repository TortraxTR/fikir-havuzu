import LoginBox from '../components/Login/LoginBox';
import LoginActions from '../components/Login/LoginActions';
import LoginHeader from '../components/Login/LoginHeader';
import LoginOptions from '../components/Login/LoginOptions';
import {
  Box,
  Container,
  Paper,
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
    <Box sx={{ minHeight: '100svh', display: 'grid', placeItems: 'center', bgcolor: '#f7f4ee'}}>
      <Container maxWidth={false} sx={{ width: '100%', display: 'flex', justifyContent: 'center' }}>
        <Paper elevation={0} component="section" sx={{ width: '100%', p: { xs: 3, sm: 5 }, border: '1px solid #e5e0d8', borderRadius: 2, bgcolor: '#fffdf9', boxShadow: 'rgba(45, 38, 31, 0.12) 0 18px 42px -18px' }}>
          <Box component="form" onSubmit={handleSubmit} noValidate>
            <LoginHeader />

            <LoginBox form={form} handleChange={handleChange} showPassword={showPassword} setShowPassword={setShowPassword} />

            <LoginOptions remember={form.remember} handleChange={handleChange} />

            <LoginActions loading={loading} error={error} />
          </Box>
        </Paper>
      </Container>
    </Box>
  );
}
