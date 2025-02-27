export interface PaginationRequest {
  pageIndex: number;
  pageSize: number;
};

export interface PaginationResponse<T> {
  elements: T[];
  pageIndex: number;
  totalPages: number;
  hasPreviousPage: boolean;
  hasNextPage: boolean;
  resultsCount: number;
};