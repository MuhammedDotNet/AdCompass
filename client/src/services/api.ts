import axios, { type AxiosResponse } from 'axios';
import type { ApiResponse } from '../types/api.js';
import type { 
  LocationSearchResponse, 
  AdvertisingPlatform 
} from '../types/platform.js';

const API_BASE_URL = 'https://localhost:7130/api';
const apiClient = axios.create({
  baseURL: API_BASE_URL,
  timeout: 10000,
  headers: {
    'Content-Type': 'application/json',
  },
});

apiClient.interceptors.response.use(
  (response) => response,
  (error) => {
    console.error('API Error:', error);
    return Promise.reject(error);
  }
);

export class ApiService {

  static async uploadPlatforms(content: string): Promise<ApiResponse<any>> {
    try {
      const response: AxiosResponse<ApiResponse<any>> = await apiClient.post(
        '/advertisingplatform/upload',
        { content }
      );
      return response.data;
    } catch (error: any) {
      return {
        success: false,
        errorMessage: error.response?.data?.errorMessage || 'Ошибка при загрузке файла'
      };
    }
  }

  static async searchPlatforms(location: string): Promise<ApiResponse<LocationSearchResponse>> {
    try {
      const response: AxiosResponse<ApiResponse<LocationSearchResponse>> = await apiClient.post(
        '/advertisingplatform/search',
        { location }
      );
      return response.data;
    } catch (error: any) {
      return {
        success: false,
        errorMessage: error.response?.data?.errorMessage || 'Ошибка при поиске площадок'
      };
    }
  }

  static async getAllPlatforms(): Promise<ApiResponse<AdvertisingPlatform[]>> {
    try {
      const response: AxiosResponse<ApiResponse<AdvertisingPlatform[]>> = await apiClient.get(
        '/advertisingplatform/all'
      );
      return response.data;
    } catch (error: any) {
      return {
        success: false,
        errorMessage: error.response?.data?.errorMessage || 'Ошибка при получении площадок'
      };
    }
  }

  static async clearAll(): Promise<ApiResponse<any>> {
    try {
      const response: AxiosResponse<ApiResponse<any>> = await apiClient.delete(
        '/advertisingplatform/clear'
      );
      return response.data;
    } catch (error: any) {
      return {
        success: false,
        errorMessage: error.response?.data?.errorMessage || 'Ошибка при очистке данных'
      };
    }
  }
}
