export interface EventRequest {
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

export interface EventAuthorResponse {
  firstName: string;
  lastName: string;
  pictureUrl?: string | null;
  rating: number;
}

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
  typeName: string;
  author: EventAuthorResponse;
}
