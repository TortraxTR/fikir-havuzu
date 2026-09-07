export type Evaluation = {
  id: string;
  userId: string;
  proposalId: string;
  score: number;
  comment: string | null;
  isPositive: boolean;
};
