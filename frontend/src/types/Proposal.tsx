import type { ProposalTopic } from './ProposalTopic';

export type Proposal = {
    id: string;
    title: string;
    topic: ProposalTopic;
    purpose: string;
    explanation: string;
    userId: string;
    userName: string;
    createdAt: string;
}
