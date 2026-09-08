import { Alert, Autocomplete, Box, Button, Menu, Paper, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, TextField, Typography } from "@mui/material";
import FilterListIcon from "@mui/icons-material/FilterList";
import { useState, useEffect } from "react";
import type { Proposal } from "../../types/Proposal";
import { fetchAllProposals } from "../../api";
import ProposalDetail from './ProposalDetail';

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

function getProposalDate(proposal: Proposal): string {
    const date = new Date(proposal.createdAt);

    if (Number.isNaN(date.getTime())) {
        return '';
    }

    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');

    return `${year}-${month}-${day}`;
}

export default function ProposalList() {
    const [proposals, setProposals] = useState<Proposal[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [titleFilter, setTitleFilter] = useState('');
    const [topicFilter, setTopicFilter] = useState('');
    const [creatorFilter, setCreatorFilter] = useState('');
    const [dateFilter, setDateFilter] = useState('');
    const [filterMenuAnchor, setFilterMenuAnchor] = useState<null | HTMLElement>(null);
    const [selectedProposal, setSelectedProposal] = useState<Proposal | null>(null);

    useEffect(() => {
        let isMounted = true;

        const userId = getCurrentUserId();
        if (!userId) {
            setError('Oturum bilgisi bulunamadı.');
            setLoading(false);
            return;
        }

        fetchAllProposals(userId)
            .then((fetchedProposals) => {
                if (isMounted) {
                    setProposals(fetchedProposals);
                }
            })
            .catch((requestError: unknown) => {
                if (isMounted) {
                    setError(
                        requestError instanceof Error
                            ? requestError.message
                            : 'Fikirler/Öneriler alınamadı.',
                    );
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

    if (loading) {
        return <Typography color="text.secondary">Fikirler/Öneriler yükleniyor...</Typography>;
    }

    if (error) {
        return <Alert severity="error">{error}</Alert>;
    }

    if (proposals.length === 0) {
        return <Typography color="text.secondary">Henüz fikir/öneri bulunmuyor.</Typography>;
    }

    if (selectedProposal) {
        return (
            <ProposalDetail
                proposal={selectedProposal}
                onBack={() => setSelectedProposal(null)}
            />
        );
    }

    const titleOptions = [...new Set(proposals.map((proposal) => proposal.title))].sort();
    const topicOptions = [...new Set(proposals.map((proposal) => proposal.topic))].sort();
    const creatorOptions = [...new Set(proposals.map((proposal) => proposal.userName))].sort();
    const dateOptions = [...new Set(proposals.map(getProposalDate))]
        .filter(Boolean)
        .sort()
        .reverse();

    const filteredProposals = proposals.filter((proposal) => {
        const proposalDate = getProposalDate(proposal);
        const normalizedTitle = proposal.title.toLocaleLowerCase('tr-TR');
        const normalizedTopic = proposal.topic.toLocaleLowerCase('tr-TR');
        const normalizedCreator = proposal.userName.toLocaleLowerCase('tr-TR');

        return (
            normalizedTitle.includes(titleFilter.toLocaleLowerCase('tr-TR')) &&
            normalizedTopic.includes(topicFilter.toLocaleLowerCase('tr-TR')) &&
            normalizedCreator.includes(creatorFilter.toLocaleLowerCase('tr-TR')) &&
            (!dateFilter || proposalDate === dateFilter)
        );
    });

    return (
        <Box>
            <Button
                variant="outlined"
                startIcon={<FilterListIcon />}
                onClick={(event) => setFilterMenuAnchor(event.currentTarget)}
                className="proposal-list-filter"
            >
                Filtrele
            </Button>

            <Menu
                anchorEl={filterMenuAnchor}
                open={Boolean(filterMenuAnchor)}
                onClose={() => setFilterMenuAnchor(null)}
                className="proposal-filter-menu"
            >
                <Box className="proposal-filter-fields">
                    <Autocomplete
                        freeSolo
                        open={false}
                        options={titleOptions}
                        value={titleFilter}
                        onInputChange={(_, value) => setTitleFilter(value)}
                        renderInput={(params) => (
                            <TextField {...params} label="Başlığa göre ara" />
                        )}
                    />

                    <Autocomplete
                        freeSolo
                        open={false}
                        options={topicOptions}
                        value={topicFilter}
                        onInputChange={(_, value) => setTopicFilter(value)}
                        renderInput={(params) => (
                            <TextField {...params} label="Konuya göre ara" />
                        )}
                    />

                    <Autocomplete
                        freeSolo
                        open={false}
                        options={creatorOptions}
                        value={creatorFilter}
                        onInputChange={(_, value) => setCreatorFilter(value)}
                        renderInput={(params) => (
                            <TextField {...params} label="Oluşturan kişiye göre ara" />
                        )}
                    />

                    <Autocomplete
                        freeSolo
                        open={false}
                        options={dateOptions}
                        value={dateFilter}
                        onInputChange={(_, value) => setDateFilter(value)}
                        getOptionLabel={(date) =>
                            date
                                ? new Date(`${date}T00:00:00`).toLocaleDateString('tr-TR')
                                : ''
                        }
                        renderInput={(params) => (
                            <TextField {...params} label="Tarihe göre ara" />
                        )}
                    />
                </Box>
            </Menu>

            {filteredProposals.length === 0 ? (
                <Typography color="text.secondary">
                    Arama kriterlerine uyan fikir/öneri bulunamadı.
                </Typography>
            ) : (
                <TableContainer component={Paper}>
                    <Table>
                        <TableHead>
                            <TableRow>
                                <TableCell>Başlık</TableCell>
                                <TableCell>Konu</TableCell>
                                <TableCell>Oluşturan</TableCell>
                                <TableCell>Oluşturulma Tarihi</TableCell>
                            </TableRow>
                        </TableHead>
                        <TableBody>
                            {filteredProposals.map((proposal) => (
                                <TableRow
                                    key={proposal.id}
                                    hover
                                    tabIndex={0}
                                    role="button"
                                    onClick={() => setSelectedProposal(proposal)}
                                    onKeyDown={(event) => {
                                        if (event.key === 'Enter' || event.key === ' ') {
                                            event.preventDefault();
                                            setSelectedProposal(proposal);
                                        }
                                    }}
                                    className="proposal-row"
                                >
                                    <TableCell>{proposal.title}</TableCell>
                                    <TableCell>{proposal.topic}</TableCell>
                                    <TableCell>{proposal.userName}</TableCell>
                                    <TableCell>
                                        {new Date(proposal.createdAt).toLocaleString()}
                                    </TableCell>
                                </TableRow>
                            ))}
                        </TableBody>
                    </Table>
                </TableContainer>
            )}
        </Box>
    );
}