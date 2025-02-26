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

export interface EventCardResponse {
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
  authorPictureUrl?: string | null;
  authorFirstName: string;
  authorLastName: string;
  authorRating: number;
}