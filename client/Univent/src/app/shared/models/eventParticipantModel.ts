export interface EventParticipantFullResponse {
  eventId: string;
  userId: string;
  firstName: string;
  lastName: string;
  pictureUrl?: string | null;
  rating: number;
  hasCompletedFeedback: boolean;
};

export interface EventParticipantFeedbackStatus {
  hasCompletedFeedback: boolean;
}