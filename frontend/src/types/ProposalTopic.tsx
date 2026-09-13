export const PROPOSAL_TOPICS = ['Urun', 'Hizmet', 'Surec'] as const;

export type ProposalTopic = typeof PROPOSAL_TOPICS[number];

export const PROPOSAL_TOPIC_LABELS: Record<ProposalTopic, string> = {
    Urun: 'Ürün',
    Hizmet: 'Hizmet',
    Surec: 'Süreç',
};
