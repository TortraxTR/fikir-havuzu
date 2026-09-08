import { useState, type ChangeEvent, type SubmitEvent } from 'react';
import { Alert, Box, Button, TextField } from '@mui/material';
import { createUser } from '../../api';

interface FormData {
  name: string;
  surname: string;
  email: string;
  phone: string;
  registrationNo: string;
  governmentId: string;
  password: string;
  confirmPassword: string;
  isActive: boolean;
}

interface FormErrors {
  name?: string;
  surname?: string;
  email?: string;
  phone?: string;
  registrationNo?: string;
  governmentId?: string;
  password?: string;
  confirmPassword?: string;
}

export default function UserAdd() {
  const [formData, setFormData] = useState<FormData>({
    name: '',
    surname: '',
    email: '',
    phone: '',
    registrationNo: '',
    governmentId: '',
    password: '',
    confirmPassword: '',
    isActive: true,
  });
  const [errors, setErrors] = useState<FormErrors>({});
  const [submitting, setSubmitting] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [success, setSuccess] = useState<string | null>(null);

  const validateForm = (): boolean => {
    const newErrors: FormErrors = {};

    if (!formData.name.trim()) newErrors.name = 'Ad gerekli';
    if (formData.name.length > 100) newErrors.name = 'Ad 100 karakteri geçemez';

    if (!formData.surname.trim()) newErrors.surname = 'Soyad gerekli';
    if (formData.surname.length > 100) newErrors.surname = 'Soyad 100 karakteri geçemez';

    if (!formData.email.trim()) newErrors.email = 'E-posta gerekli';
    if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(formData.email)) newErrors.email = 'Geçerli bir e-posta adresi girin';
    if (formData.email.length > 254) newErrors.email = 'E-posta 254 karakteri geçemez';

    if (!formData.phone.trim()) newErrors.phone = 'Telefon gerekli';
    if (!/^[0-9+\-\s()]+$/.test(formData.phone)) newErrors.phone = 'Geçerli bir telefon numarası girin';
    if (formData.phone.length > 15) newErrors.phone = 'Telefon 15 karakteri geçemez';

    if (!formData.registrationNo.trim()) newErrors.registrationNo = 'Sicil No. gerekli';
    if (formData.registrationNo.length > 50) newErrors.registrationNo = 'Sicil No. 50 karakteri geçemez';

    if (!formData.governmentId.trim()) newErrors.governmentId = 'T.C. Kimlik No. gerekli';
    if (formData.governmentId.length !== 11) newErrors.governmentId = 'T.C. Kimlik No. tam olarak 11 karakter olmalı';

    if (!formData.password) newErrors.password = 'Şifre gerekli';
    if (formData.password.length < 6) newErrors.password = 'Şifre en az 6 karakter olmalı';
    if (formData.password.length > 100) newErrors.password = 'Şifre 100 karakteri geçemez';

    if (formData.password !== formData.confirmPassword) {
      newErrors.confirmPassword = 'Şifreler eşleşmiyor';
    }

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleChange = (e: ChangeEvent<HTMLInputElement>) => {
    const { name, value, type, checked } = e.target;
    setFormData((prev) => ({
      ...prev,
      [name]: type === 'checkbox' ? checked : value,
    }));
    // Clear error for this field when user starts typing
    if (errors[name as keyof FormErrors]) {
      setErrors((prev) => ({
        ...prev,
        [name]: undefined,
      }));
    }
  };

  const handleSubmit = async (event: SubmitEvent<HTMLFormElement>) => {
    event.preventDefault();
    setError(null);
    setSuccess(null);

    if (!validateForm()) {
      return;
    }

    setSubmitting(true);

    try {
      const newUser = await createUser({
        name: formData.name,
        surname: formData.surname,
        email: formData.email,
        phone: formData.phone,
        registrationNo: formData.registrationNo,
        governmentId: formData.governmentId,
        password: formData.password,
        isActive: formData.isActive,
      });

      setSuccess(`Kullanıcı ${newUser.name} ${newUser.surname} başarıyla oluşturuldu.`);
      setFormData({
        name: '',
        surname: '',
        email: '',
        phone: '',
        registrationNo: '',
        governmentId: '',
        password: '',
        confirmPassword: '',
        isActive: true,
      });
    } catch (requestError: unknown) {
      setError(
        requestError instanceof Error ? requestError.message : 'Kullanıcı oluşturulamadı.'
      );
    } finally {
      setSubmitting(false);
    }
  };

  return (
    <Box component="form" onSubmit={handleSubmit} className="form-stack">
      <Box className="form-grid">
        <TextField
          name="name"
          label="Ad"
          value={formData.name}
          onChange={handleChange}
          disabled={submitting}
          error={!!errors.name}
          helperText={errors.name}
          required
          fullWidth
        />
        <TextField
          name="surname"
          label="Soyad"
          value={formData.surname}
          onChange={handleChange}
          disabled={submitting}
          error={!!errors.surname}
          helperText={errors.surname}
          required
          fullWidth
        />
      </Box>

      <TextField
        name="email"
        label="E-posta"
        type="email"
        value={formData.email}
        onChange={handleChange}
        disabled={submitting}
        error={!!errors.email}
        helperText={errors.email}
        required
        fullWidth
      />

      <Box className="form-grid">
        <TextField
          name="phone"
          label="Telefon Numarası"
          value={formData.phone}
          onChange={handleChange}
          disabled={submitting}
          error={!!errors.phone}
          helperText={errors.phone}
          placeholder="5XX XXX XX XX"
          required
          fullWidth
        />
        <TextField
          name="registrationNo"
          label="Sicil No."
          value={formData.registrationNo}
          onChange={handleChange}
          disabled={submitting}
          error={!!errors.registrationNo}
          helperText={errors.registrationNo}
          required
          fullWidth
        />
      </Box>

      <TextField
        name="governmentId"
        label="T.C. Kimlik No."
        value={formData.governmentId}
        onChange={handleChange}
        disabled={submitting}
        error={!!errors.governmentId}
        helperText={errors.governmentId}
        placeholder="11 rakam"
        required
        fullWidth
      />

      <Box className="form-grid">
        <TextField
          name="password"
          label="Şifre"
          type="password"
          value={formData.password}
          onChange={handleChange}
          disabled={submitting}
          error={!!errors.password}
          helperText={errors.password}
          required
          fullWidth
        />
        <TextField
          name="confirmPassword"
          label="Şifreyi Onayla"
          type="password"
          value={formData.confirmPassword}
          onChange={handleChange}
          disabled={submitting}
          error={!!errors.confirmPassword}
          helperText={errors.confirmPassword}
          required
          fullWidth
        />
      </Box>

      <Button type="submit" variant="contained" disabled={submitting}>
        {submitting ? 'Kullanıcı oluşturuluyor...' : 'Kullanıcı Ekle'}
      </Button>

      {error && <Alert severity="error">{error}</Alert>}
      {success && <Alert severity="success">{success}</Alert>}
    </Box>
  );
}