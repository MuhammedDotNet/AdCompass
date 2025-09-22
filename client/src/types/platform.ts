export interface AdvertisingPlatform {
  name: string;
  locations: string[];
}

export interface LocationSearchRequest {
  location: string;
}

export interface LocationSearchResponse {
  location: string;
  platforms: string[];
  count: number;
}

export interface FileUploadRequest {
  content: string;
}

export interface LoadingState {
  isLoading: boolean;
  error?: string;
}
