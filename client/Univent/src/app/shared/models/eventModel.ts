export interface EventRequest {
  name: string;
  description: string;
  maximumParticipants: number;
  startTime: string;
  endTime: string;
  locationAddress: string;
  locationLat: number;
  locationLong: number;
  pictureUrl: string;
  typeId: string;
};