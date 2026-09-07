import { Alert, Box, Button, TextField } from '@mui/material';
import { useState, type ChangeEvent, type SubmitEvent } from 'react';
import { createProposal } from '../../api';

type ProposalForm = {
	title: string;
	topic: string;
	purpose: string;
	explanation: string;
};

type ProposalFormErrors = Partial<Record<keyof ProposalForm, string>>;

const initialForm: ProposalForm = {
	title: '',
	topic: '',
	purpose: '',
	explanation: '',
};

function getCurrentUserId(): string | null {
	const savedUser = localStorage.getItem('user');

	if (!savedUser) {
		return null;
	}

	try {
		const user = JSON.parse(savedUser) as { id?: unknown };
		return typeof user.id === 'string' && user.id ? user.id : null;
	} catch {
		return null;
	}
}

function validateForm(form: ProposalForm): ProposalFormErrors {
	const errors: ProposalFormErrors = {};

	if (!form.title.trim()) errors.title = 'Başlık zorunludur.';
	if (!form.topic.trim()) errors.topic = 'Konu zorunludur.';
	if (!form.purpose.trim()) errors.purpose = 'Amaç zorunludur.';
	if (!form.explanation.trim()) errors.explanation = 'Açıklama zorunludur.';
	if (form.title.length > 128) errors.title = 'Başlık en fazla 128 karakter olabilir.';
	if (form.topic.length > 128) errors.topic = 'Konu en fazla 128 karakter olabilir.';
	if (form.purpose.length > 128) errors.purpose = 'Amaç en fazla 128 karakter olabilir.';
	if (form.explanation.length > 8192) errors.explanation = 'Açıklama en fazla 8192 karakter olabilir.';

	return errors;
}

export default function ProposalCreate() {
	const [form, setForm] = useState<ProposalForm>(initialForm);
	const [errors, setErrors] = useState<ProposalFormErrors>({});
	const [submitting, setSubmitting] = useState(false);
	const [error, setError] = useState<string | null>(null);
	const [success, setSuccess] = useState<string | null>(null);

	const handleChange = (event: ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
		const { name, value } = event.target;
		setForm((current) => ({ ...current, [name]: value }));
		setErrors((current) => ({ ...current, [name]: undefined }));
		setError(null);
		setSuccess(null);
	};

	const handleSubmit = async (event: SubmitEvent<HTMLFormElement>) => {
		event.preventDefault();
		const validationErrors = validateForm(form);
		const userId = getCurrentUserId();

		if (!userId) {
			setError('Oturum bilgisi bulunamadı. Lütfen tekrar giriş yapın.');
			return;
		}

		if (Object.keys(validationErrors).length > 0) {
			setErrors(validationErrors);
			return;
		}

		setErrors({});
		setError(null);
		setSuccess(null);
		setSubmitting(true);

		try {
			await createProposal({ userId, ...form });
			setForm(initialForm);
			setSuccess('Fikir/öneri başarıyla oluşturuldu.');
		} catch (requestError: unknown) {
			setError(requestError instanceof Error ? requestError.message : 'Fikir/öneri oluşturulamadı.');
		} finally {
			setSubmitting(false);
		}
	};

	return (
		<Box component="form" onSubmit={handleSubmit} className="form-stack">
			<Box className="form-grid">
				<TextField
					name="title"
					label="Başlık"
					value={form.title}
					onChange={handleChange}
					disabled={submitting}
					error={!!errors.title}
					helperText={errors.title}
					slotProps={{ htmlInput: { maxLength: 128 } }}
					required
					fullWidth
				/>
				<TextField
					name="topic"
					label="Konu"
					value={form.topic}
					onChange={handleChange}
					disabled={submitting}
					error={!!errors.topic}
					helperText={errors.topic}
					slotProps={{ htmlInput: { maxLength: 128 } }}
					required
					fullWidth
				/>
			</Box>
			<TextField
				name="purpose"
				label="Amaç"
				value={form.purpose}
				onChange={handleChange}
				disabled={submitting}
				error={!!errors.purpose}
				helperText={errors.purpose}
				slotProps={{ htmlInput: { maxLength: 128 } }}
				required
				fullWidth
			/>
			<TextField
				name="explanation"
				label="Açıklama"
				value={form.explanation}
				onChange={handleChange}
				disabled={submitting}
				error={!!errors.explanation}
				helperText={errors.explanation}
				slotProps={{ htmlInput: { maxLength: 8192 } }}
				multiline
				minRows={6}
				required
				fullWidth
			/>
			<Button
				type="submit"
				variant="contained"
				className="workspace-action workspace-action-proposals"
				disabled={submitting}
			>
				{submitting ? 'Fikir/öneri oluşturuluyor...' : 'Fikir/öneri oluştur'}
			</Button>
			{error && <Alert severity="error">{error}</Alert>}
			{success && <Alert severity="success">{success}</Alert>}
		</Box>
	);
}
