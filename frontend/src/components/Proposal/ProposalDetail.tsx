import ArrowBackIcon from '@mui/icons-material/ArrowBack';
import { Alert, Box, Button, Divider, Typography } from '@mui/material';
import { useEffect, useState } from 'react';
import { fetchProposalEvaluations } from '../../api';
import type { Proposal } from '../../types/Proposal';
import type { Evaluation } from '../../types/Evaluation';
import ProposalEvaluate from './ProposalEvaluate';

type ProposalDetailProps = {
	proposal: Proposal;
	onBack: () => void;
};

export default function ProposalDetail({ proposal, onBack }: ProposalDetailProps) {
	const [evaluations, setEvaluations] = useState<Evaluation[]>([]);
	const [loadingEvaluations, setLoadingEvaluations] = useState(true);
	const [evaluationError, setEvaluationError] = useState<string | null>(null);

	useEffect(() => {
		let isMounted = true;

		fetchProposalEvaluations(proposal.id)
			.then((fetchedEvaluations) => {
				if (isMounted) {
					setEvaluations(fetchedEvaluations);
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
				<Box className="detail-meta-item">
					<Typography className="detail-meta-label">Konu</Typography>
					<Typography className="detail-topic-value">
						{proposal.topic || 'Belirtilmemiş'}
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
				{evaluationError && <Alert severity="error">{evaluationError}</Alert>}
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

				<ProposalEvaluate
					proposalId={proposal.id}
					onEvaluationCreated={(evaluation) => setEvaluations((current) => [...current, evaluation])}
				/>
			</Box>
		</Box>
	);
}
