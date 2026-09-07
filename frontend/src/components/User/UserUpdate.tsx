import { useEffect, useState, type ChangeEvent, type SubmitEvent } from 'react';
import { Alert, Autocomplete, Box, Button, TextField, Typography } from '@mui/material';
import { fetchAllUsers, updateUser } from '../../api';
import type { User } from '../../types/User';

interface FormData {
    name: string;
    surname: string;
    phone: string;
    registrationNo: string;
    governmentId: string;
}

interface FormErrors {
    name?: string;
    surname?: string;
    phone?: string;
    registrationNo?: string;
    governmentId?: string;
}

const emptyForm = (user: User): FormData => ({
    name: user.name,
    surname: user.surname,
    phone: user.phone,
    registrationNo: user.registrationNo,
    governmentId: user.governmentId,
});

export default function UserUpdate() {
    const [users, setUsers] = useState<User[]>([]);
    const [selectedUserId, setSelectedUserId] = useState('');
    const [formData, setFormData] = useState<FormData | null>(null);
    const [loading, setLoading] = useState(true);
    const [submitting, setSubmitting] = useState(false);
    const [error, setError] = useState<string | null>(null);
    const [success, setSuccess] = useState<string | null>(null);
    const [validationErrors, setValidationErrors] = useState<FormErrors>({});

    useEffect(() => {
        let isMounted = true;

        fetchAllUsers()
            .then((fetchedUsers) => {
                if (isMounted) {
                    setUsers(fetchedUsers);
                }
            })
            .catch((requestError: unknown) => {
                if (isMounted) {
                    setError(requestError instanceof Error ? requestError.message : 'Kullanıcılar alınamadı.');
                }
            })
            .finally(() => {
                if (isMounted) {
                    setLoading(false);
                }
            });

        return () => {
            isMounted = false;
        };
    }, []);

    const selectedUser = users.find((user) => user.id === selectedUserId) ?? null;

    useEffect(() => {
        if (!selectedUser) {
            setFormData(null);
            return;
        }

        setFormData(emptyForm(selectedUser));
        setValidationErrors({});
        setError(null);
        setSuccess(null);
    }, [selectedUser]);

    const handleFieldChange = (event: ChangeEvent<HTMLInputElement>) => {
        const { name, value } = event.target;
        setFormData((current) => {
            if (!current) {
                return current;
            }

            return {
                ...current,
                [name]: value,
            };
        });

        if (validationErrors[name as keyof FormErrors]) {
            setValidationErrors((current) => ({
                ...current,
                [name]: undefined,
            }));
        }
    };

    const validateForm = (): boolean => {
        if (!formData) {
            return false;
        }

        const errors: FormErrors = {};

        if (!formData.name.trim()) {
            errors.name = 'Ad gerekli';
        } else if (formData.name.length > 100) {
            errors.name = 'Ad 100 karakteri geçemez';
        }

        if (!formData.surname.trim()) {
            errors.surname = 'Soyad gerekli';
        } else if (formData.surname.length > 100) {
            errors.surname = 'Soyad 100 karakteri geçemez';
        }

        if (!formData.phone.trim()) {
            errors.phone = 'Telefon gerekli';
        } else if (!/^[0-9+\-\s()]+$/.test(formData.phone)) {
            errors.phone = 'Geçerli bir telefon numarası girin';
        } else if (formData.phone.length > 15) {
            errors.phone = 'Telefon 15 karakteri geçemez';
        }

        if (!formData.registrationNo.trim()) {
            errors.registrationNo = 'Sicil No. gerekli';
        } else if (formData.registrationNo.length > 50) {
            errors.registrationNo = 'Sicil No. 50 karakteri geçemez';
        }

        if (!formData.governmentId.trim()) {
            errors.governmentId = 'T.C. Kimlik No. gerekli';
        } else if (formData.governmentId.length !== 11) {
            errors.governmentId = 'T.C. Kimlik No. tam olarak 11 karakter olmalı';
        }

        setValidationErrors(errors);
        return Object.keys(errors).length === 0;
    };

    const handleSubmit = async (event: SubmitEvent<HTMLFormElement>) => {
        event.preventDefault();

        if (!selectedUser || !formData || !validateForm()) {
            return;
        }

        setError(null);
        setSuccess(null);
        setSubmitting(true);

        try {
            const updatedUser = await updateUser({
                ...selectedUser,
                name: formData.name,
                surname: formData.surname,
                phone: formData.phone,
                registrationNo: formData.registrationNo,
                governmentId: formData.governmentId,
                isActive: true,
            });

            setUsers((currentUsers) => currentUsers.map((user) =>
                user.id === updatedUser.id ? updatedUser : user,
            ));
            setSuccess(`Kullanıcı ${updatedUser.name} ${updatedUser.surname} başarıyla güncellendi.`);
        } catch (requestError: unknown) {
            setError(requestError instanceof Error ? requestError.message : 'Kullanıcı güncellenemedi.');
        } finally {
            setSubmitting(false);
        }
    };

    if (loading) {
        return <Typography color="text.secondary">Kullanıcılar yükleniyor...</Typography>;
    }

    return (
        <Box component="form" onSubmit={handleSubmit} className="form-stack">
            <Autocomplete
                options={users}
                getOptionLabel={(user) => `${user.name} ${user.surname} (${user.phone})`}
                value={selectedUser}
                onChange={(_, user) => {
                    setSelectedUserId(user?.id ?? '');
                }}
                disabled={submitting}
                renderOption={(props, user) => (
                    <li {...props} key={user.id}>
                        {user.name} {user.surname} ({user.phone}) - {user.isActive ? 'Aktif' : 'İnaktif'}
                    </li>
                )}
                renderInput={(params) => (
                    <TextField {...params} required label="Kullanıcı seç" />
                )}
            />

            {selectedUser && formData && (
                <>
                    <Box className="form-grid">
                        <TextField
                            name="name"
                            label="Ad"
                            value={formData.name}
                            onChange={handleFieldChange}
                            disabled={submitting}
                            error={!!validationErrors.name}
                            helperText={validationErrors.name}
                            required
                            fullWidth
                        />
                        <TextField
                            name="surname"
                            label="Soyad"
                            value={formData.surname}
                            onChange={handleFieldChange}
                            disabled={submitting}
                            error={!!validationErrors.surname}
                            helperText={validationErrors.surname}
                            required
                            fullWidth
                        />
                    </Box>

                    <Box className="form-grid">
                        <TextField
                            name="phone"
                            label="Telefon Numarası"
                            value={formData.phone}
                            onChange={handleFieldChange}
                            disabled={submitting}
                            error={!!validationErrors.phone}
                            helperText={validationErrors.phone}
                            required
                            fullWidth
                        />
                        <TextField
                            name="registrationNo"
                            label="Sicil No."
                            value={formData.registrationNo}
                            onChange={handleFieldChange}
                            disabled={submitting}
                            error={!!validationErrors.registrationNo}
                            helperText={validationErrors.registrationNo}
                            required
                            fullWidth
                        />
                    </Box>

                    <TextField
                        name="governmentId"
                        label="T.C. Kimlik No."
                        value={formData.governmentId}
                        onChange={handleFieldChange}
                        disabled={submitting}
                        error={!!validationErrors.governmentId}
                        helperText={validationErrors.governmentId}
                        required
                        fullWidth
                    />

                    <Button type="submit" variant="contained" disabled={submitting}>
                        {submitting ? 'Kullanıcı güncelleniyor...' : 'Kullanıcıyı Güncelle'}
                    </Button>
                </>
            )}

            {error && <Alert severity="error">{error}</Alert>}
            {success && <Alert severity="success">{success}</Alert>}
        </Box>
    );
}