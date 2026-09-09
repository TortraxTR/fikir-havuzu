import { Alert, Box, Button, MenuItem, Rating, Select, TextField, Typography } from '@mui/material';
import { useEffect, useState, type ChangeEvent, type SubmitEvent } from 'react';
import { createEvaluation, fetchUserPermissions } from '../../api';
import type { Evaluation } from '../../types/Evaluation';

type ProposalEvaluateProps = {
	proposalId: string;
	onEvaluationCreated: (evaluation: Evaluation) => void;
};

const initialForm = { comment: '', score: '0', isPositive: 'true' };

function getCurrentUserId(): string | null {
	const savedUser = localStorage.getItem('user');

	if (!savedUser) {
		return null;
	}

	try {
		const user = JSON.parse(savedUser) as { id?: unknown };
		return typeof user.id === 'string' ? user.id : null;
	} catch {
		return null;
	}
}

export default function ProposalEvaluate({ proposalId, onEvaluationCreated }: ProposalEvaluateProps) {
	const [canEvaluate, setCanEvaluate] = useState(false);
	const [form, setForm] = useState(initialForm);
	const [submitting, setSubmitting] = useState(false);
	const [error, setError] = useState<string | null>(null);
	const [success, setSuccess] = useState<string | null>(null);

	useEffect(() => {
		let isMounted = true;
		const userId = getCurrentUserId();

		if (!userId) {
			return;
		}

		fetchUserPermissions(userId)
			.then((permissions) => {
				if (isMounted) {
					setCanEvaluate(permissions.some((permission) => permission.code === 'EVALUATION_CREATE'));
				}
			})
			.catch(() => {
				if (isMounted) {
					setCanEvaluate(false);
				}
			});

		return () => {
			isMounted = false;
		};
	}, []);

	const handleChange = (event: ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
		const { name, value } = event.target;
		setForm((current) => ({ ...current, [name]: value }));
		setError(null);
		setSuccess(null);
	};

	const handleSubmit = async (event: SubmitEvent<HTMLFormElement>) => {
		event.preventDefault();
		const userId = getCurrentUserId();

		if (!userId) {
			setError('Oturum bilgisi bulunamadı.');
			return;
		}

		setSubmitting(true);
		setError(null);
		setSuccess(null);

		try {
			const createdEvaluation = await createEvaluation({
				userId,
				proposalId,
				comment: form.comment.trim(),
				score: Number(form.score),
				isPositive: form.isPositive === 'true',
			});
			onEvaluationCreated(createdEvaluation);
			setForm(initialForm);
			setSuccess('Değerlendirme gönderildi.');
		} catch (requestError: unknown) {
			setError(requestError instanceof Error ? requestError.message : 'Değerlendirme gönderilemedi.');
		} finally {
			setSubmitting(false);
		}
	};

	if (!canEvaluate) {
		return null;
	}

	return (
		<Box component="form" onSubmit={handleSubmit} className="form-stack evaluation-form">
			<Typography className="detail-section-label">Değerlendirme yaz</Typography>
			<Box className="evaluation-rating-field">
				<Typography className="detail-meta-label">Puan</Typography>
				<Rating
					name="score"
					value={Number(form.score)}
					onChange={(_, value) => setForm((current) => ({ ...current, score: String(value ?? 0) }))}
					max={10}
					disabled={submitting}
				/>
				<Typography color="text.secondary">{form.score} / 10</Typography>
			</Box>
			<Select name="isPositive" value={form.isPositive} onChange={(event) => setForm((current) => ({ ...current, isPositive: event.target.value }))} disabled={submitting} fullWidth>
				<MenuItem value="true">Olumlu</MenuItem>
				<MenuItem value="false">Olumsuz</MenuItem>
			</Select>
			<TextField name="comment" label="Yorum" value={form.comment} onChange={handleChange} disabled={submitting} multiline minRows={4} fullWidth />
			<Button type="submit" variant="contained" className="workspace-action workspace-action-proposals" disabled={submitting}>
				{submitting ? 'Gönderiliyor...' : 'Değerlendirmeyi gönder'}
			</Button>
			{error && <Alert severity="error">{error}</Alert>}
			{success && <Alert severity="success">{success}</Alert>}
		</Box>
	);
}
