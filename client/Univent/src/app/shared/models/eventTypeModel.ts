export interface EventTypeResponse {
  id: string;
  name: string;
  isDeleted: boolean;
};

export interface EventTypeRequest {
  name: string;
};