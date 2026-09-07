import ArrowBackIcon from '@mui/icons-material/ArrowBack';
import { Box, Button, Typography } from '@mui/material';
import type { Proposal } from '../../types/Proposal';

type ProposalDetailProps = {
	proposal: Proposal;
	onBack: () => void;
};

export default function ProposalDetail({ proposal, onBack }: ProposalDetailProps) {
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
		</Box>
	);
}
