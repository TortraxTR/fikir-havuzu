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
    <Box className="page-shell login-page-shell">
      <Container className="page-container login-container" maxWidth={false}>
        <Paper className="login-paper" elevation={0} component="section">
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
