import ArrowBackIcon from '@mui/icons-material/ArrowBack';
import { Alert, Box, Button, Divider, MenuItem, Rating, Select, TextField, Typography } from '@mui/material';
import { useEffect, useState, type ChangeEvent, type SubmitEvent } from 'react';
import { createEvaluation, fetchProposalEvaluations, fetchUserPermissions } from '../../api';
import type { Proposal } from '../../types/Proposal';
import type { Evaluation } from '../../types/Evaluation';

type ProposalDetailProps = {
	proposal: Proposal;
	onBack: () => void;
};

export default function ProposalDetail({ proposal, onBack }: ProposalDetailProps) {
	const [evaluations, setEvaluations] = useState<Evaluation[]>([]);
	const [canEvaluate, setCanEvaluate] = useState(false);
	const [loadingEvaluations, setLoadingEvaluations] = useState(true);
	const [evaluationError, setEvaluationError] = useState<string | null>(null);
	const [submittingEvaluation, setSubmittingEvaluation] = useState(false);
	const [evaluationSuccess, setEvaluationSuccess] = useState<string | null>(null);
	const [evaluationForm, setEvaluationForm] = useState({ comment: '', score: '0', isPositive: 'true' });

	useEffect(() => {
		let isMounted = true;
		const savedUser = localStorage.getItem('user');
		let userId: string | null = null;

		if (savedUser) {
			try {
				const user = JSON.parse(savedUser) as { id?: unknown };
				userId = typeof user.id === 'string' ? user.id : null;
			} catch {
				userId = null;
			}
		}

		Promise.all([
			fetchProposalEvaluations(proposal.id),
			userId ? fetchUserPermissions(userId) : Promise.resolve([]),
		])
			.then(([fetchedEvaluations, permissions]) => {
				if (isMounted) {
					setEvaluations(fetchedEvaluations);
					setCanEvaluate(permissions.some((permission) => permission.code === 'EVALUATION_CREATE'));
				}
			})
			.catch((requestError: unknown) => {
				if (isMounted) {
					setEvaluationError(requestError instanceof Error ? requestError.message : 'Değerlendirmeler alınamadı.');
				}
			})
			.finally(() => {
				if (isMounted) {
					setLoadingEvaluations(false);
				}
			});

		return () => {
			isMounted = false;
		};
	}, [proposal.id]);

	const handleEvaluationChange = (event: ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
		const { name, value } = event.target;
		setEvaluationForm((current) => ({ ...current, [name]: value }));
		setEvaluationError(null);
		setEvaluationSuccess(null);
	};

	const handleEvaluationSubmit = async (event: SubmitEvent<HTMLFormElement>) => {
		event.preventDefault();
		const savedUser = localStorage.getItem('user');
		let userId: string | null = null;

		if (savedUser) {
			try {
				const user = JSON.parse(savedUser) as { id?: unknown };
				userId = typeof user.id === 'string' ? user.id : null;
			} catch {
				userId = null;
			}
		}

		if (!userId) {
			setEvaluationError('Oturum bilgisi bulunamadı.');
			return;
		}

		setSubmittingEvaluation(true);
		setEvaluationError(null);
		setEvaluationSuccess(null);

		try {
			const createdEvaluation = await createEvaluation({
				userId,
				proposalId: proposal.id,
				comment: evaluationForm.comment.trim(),
				score: Number(evaluationForm.score),
				isPositive: evaluationForm.isPositive === 'true',
			});
			setEvaluations((current) => [...current, createdEvaluation]);
			setEvaluationForm({ comment: '', score: '0', isPositive: 'true' });
			setEvaluationSuccess('Değerlendirme gönderildi.');
		} catch (requestError: unknown) {
			setEvaluationError(requestError instanceof Error ? requestError.message : 'Değerlendirme gönderilemedi.');
		} finally {
			setSubmittingEvaluation(false);
		}
	};

	return (
		<Box className="proposal-detail">
			<Box className="detail-topbar">
				<Button
					variant="text"
					startIcon={<ArrowBackIcon />}
					onClick={onBack}
					className="detail-back"
				>
					Listeye dön
				</Button>
			</Box>

			<Box component="header" className="detail-header">
				<Typography component="h2" className="detail-title">
					{proposal.title}
				</Typography>
				<Typography className="detail-topic">
					{proposal.topic}
				</Typography>
			</Box>

			<Box className="detail-meta">
				<Box className="detail-meta-item">
					<Typography className="detail-meta-label">Oluşturan</Typography>
					<Typography className="detail-author">{proposal.userName}</Typography>
				</Box>
				<Box className="detail-meta-item">
					<Typography className="detail-meta-label">Oluşturulma tarihi</Typography>
					<Typography className="detail-date">
						{new Date(proposal.createdAt).toLocaleString('tr-TR')}
					</Typography>
				</Box>
				<Box className="detail-purpose-section">
					<Typography className="detail-meta-label">Amaç</Typography>
					<Typography className="detail-purpose">
						{proposal.purpose || 'Belirtilmemiş'}
					</Typography>
				</Box>
			</Box>

			<Box className="detail-content">
				<Box className="detail-explanation-section">
					<Box className="detail-explanation">
						<Typography className="detail-section-label">
							Açıklama
						</Typography>
						<Typography className="detail-explanation-text">
							{proposal.explanation || 'Belirtilmemiş'}
						</Typography>
					</Box>
				</Box>
			</Box>

			<Divider className="detail-divider" />

			<Box component="section" className="evaluation-section">
				<Typography component="h3" className="detail-section-label">Değerlendirmeler</Typography>
				{loadingEvaluations && <Typography color="text.secondary">Değerlendirmeler yükleniyor...</Typography>}
				{evaluationError && !canEvaluate && <Alert severity="error">{evaluationError}</Alert>}
				{!loadingEvaluations && !evaluationError && evaluations.length === 0 && (
					<Typography color="text.secondary">Henüz değerlendirme bulunmuyor.</Typography>
				)}
				{evaluations.length > 0 && (
					<Box className="evaluation-list">
						{evaluations.map((evaluation) => (
							<Box key={evaluation.id} className="evaluation-item">
								<Typography className="evaluation-result">
									{evaluation.isPositive ? 'Olumlu' : 'Olumsuz'} · {evaluation.score}/10
								</Typography>
								{evaluation.comment && <Typography className="evaluation-comment">{evaluation.comment}</Typography>}
							</Box>
						))}
					</Box>
				)}

				{canEvaluate && (
					<Box component="form" onSubmit={handleEvaluationSubmit} className="form-stack evaluation-form">
						<Typography className="detail-section-label">Değerlendirme yaz</Typography>
						<Box className="evaluation-rating-field">
							<Typography className="detail-meta-label">Puan</Typography>
							<Rating
								name="score"
								value={Number(evaluationForm.score)}
								onChange={(_, value) => setEvaluationForm((current) => ({ ...current, score: String(value ?? 0) }))}
								max={10}
								disabled={submittingEvaluation}
							/>
							<Typography color="text.secondary">{evaluationForm.score} / 10</Typography>
						</Box>
						<Select name="isPositive" value={evaluationForm.isPositive} onChange={(event) => setEvaluationForm((current) => ({ ...current, isPositive: event.target.value }))} disabled={submittingEvaluation} fullWidth>
							<MenuItem value="true">Olumlu</MenuItem>
							<MenuItem value="false">Olumsuz</MenuItem>
						</Select>
						<TextField name="comment" label="Yorum" value={evaluationForm.comment} onChange={handleEvaluationChange} disabled={submittingEvaluation} multiline minRows={4} fullWidth />
						<Button type="submit" variant="contained" className="workspace-action workspace-action-proposals" disabled={submittingEvaluation}>
							{submittingEvaluation ? 'Gönderiliyor...' : 'Değerlendirmeyi gönder'}
						</Button>
						{evaluationError && <Alert severity="error">{evaluationError}</Alert>}
						{evaluationSuccess && <Alert severity="success">{evaluationSuccess}</Alert>}
					</Box>
				)}
			</Box>
		</Box>
	);
}
