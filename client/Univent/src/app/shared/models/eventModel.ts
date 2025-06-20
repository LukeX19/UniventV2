export interface CreateEventRequest {
  name: string;
  description: string;
  maximumParticipants: number;
  startTime: string;
  locationAddress: string;
  locationLat: number;
  locationLong: number;
  pictureUrl: string;
  typeId: string;
};

export interface UpdateEventRequest {
  name: string;
  description: string;
  maximumParticipants: number;
  startTime: string;
  locationAddress: string;
  locationLat: number;
  locationLong: number;
  pictureUrl: string;
};

export interface EventAuthorResponse {
  id: string;
  firstName: string;
  lastName: string;
  pictureUrl?: string | null;
  rating: number;
};

export interface EventSummaryResponse {
  id: string;
  name: string;
  enrolledParticipants: number;
  maximumParticipants: number;
  startTime: string;
  locationAddress: string;
  pictureUrl: string;
  createdAt: string;
  updatedAt: string;
  isCancelled: boolean;
  typeName: string;
  author: EventAuthorResponse;
};

export interface EventSummaryWithFeedbackStatusResponse {
  event: EventSummaryResponse;
  hasLoggedUserProvidedFeedback: boolean;
};

export interface EventFullResponse {
  id: string;
  name: string;
  maximumParticipants: number;
  startTime: string;
  description: string;
  locationAddress: string;
  locationLat: number;
  locationLong: number;
  pictureUrl: string;
  createdAt: string;
  updatedAt: string;
  isCancelled: boolean;
  typeName: string;
  author: EventAuthorResponse;
};
