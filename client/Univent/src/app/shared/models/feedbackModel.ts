export interface FeedbackRecipientDto {
  userId: string;
  rating: number;
}

export interface CreateMultipleFeedbacksDto {
  recipients: FeedbackRecipientDto[];
}
