import { Typography, Alert, TableContainer, Paper, Table, TableHead, TableRow, TableCell, TableBody } from "@mui/material";
import { useState, useEffect } from "react";
import type { Proposal } from "../../types/Proposal";

export default function ProposalList() {

    const [proposals, setProposals] = useState<Proposal[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        let isMounted = true;

        fetchAllProposals()
            .then((fetchedProposals) => {
                if (isMounted) {
                    setProposals(fetchedProposals);
                }
            })
            .catch((requestError: unknown) => {
                if (isMounted) {
                    setError(requestError instanceof Error ? requestError.message : 'Fikirler/Öneriler alınamadı.');
                }
            })
            .finally(() => {
                if (isMounted) {
                    setLoading(false);
                }
            });

        return () => {
            isMounted = false;
        }
    }, []);

    if (loading) {
        return <Typography color="text.secondary">Fikirler/Öneriler yükleniyor...</Typography>;
    }
    
    if (error) {
        return <Alert severity="error">{error}</Alert>;
    }

    if (proposals.length === 0) {
        return <Typography color="text.secondary">Henüz fikir/öneri bulunmuyor.</Typography>;
    }

    return (
        <TableContainer component={Paper}>
            <Table>
                <TableHead>
                    <TableRow>
                        <TableCell>Başlık</TableCell>
                        <TableCell>Açıklama</TableCell>
                        <TableCell>Oluşturan</TableCell>
                        <TableCell>Oluşturulma Tarihi</TableCell>
                    </TableRow>
                </TableHead>
                <TableBody>
                    {proposals.map((proposal) => (
                        <TableRow key={proposal.id}>
                            <TableCell>{proposal.title}</TableCell>
                            <TableCell>{proposal.description}</TableCell>
                            <TableCell>{proposal.creatorName}</TableCell>
                            <TableCell>{new Date(proposal.createdAt).toLocaleString()}</TableCell>
                        </TableRow>
                    ))}
                </TableBody>
            </Table>
        </TableContainer>
    );
}