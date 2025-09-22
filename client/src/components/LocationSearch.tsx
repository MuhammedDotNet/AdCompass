import React, { useState } from 'react';
import { AutoComplete, Button, Card, Typography, List, Space, Tag, App } from 'antd';
import { SearchOutlined, ClearOutlined } from '@ant-design/icons';
import { ApiService } from '../services/api';

const { Title, Text } = Typography;

interface LocationSearchProps {
  onSearchComplete: (results: string[], location: string) => void;
}

const LocationSearch: React.FC<LocationSearchProps> = ({ onSearchComplete }) => {
  const [location, setLocation] = useState('');
  const [loading, setLoading] = useState(false);
  const [results, setResults] = useState<string[]>([]);
  const [lastSearchedLocation, setLastSearchedLocation] = useState('');
  const { message } = App.useApp();
  
  const locationOptions = [
    { value: '/ru', label: '/ru - Вся Россия' },
    { value: '/ru/msk', label: '/ru/msk - Москва' },
    { value: '/ru/spb', label: '/ru/spb - Санкт-Петербург' },
    { value: '/ru/svrd', label: '/ru/svrd - Свердловская область' },
    { value: '/ru/svrd/ekb', label: '/ru/svrd/ekb - Екатеринбург' },
    { value: '/ru/svrd/revda', label: '/ru/svrd/revda - Ревда' },
    { value: '/ru/svrd/pervik', label: '/ru/svrd/pervik - Первоуральск' },
    { value: '/ru/msk/center', label: '/ru/msk/center - Центр Москвы' },
    { value: '/ru/spb/center', label: '/ru/spb/center - Центр СПб' },
    { value: '/ru/permobl', label: '/ru/permobl - Пермский край' },
    { value: '/ru/chelobl', label: '/ru/chelobl - Челябинская область' },
  ];

  const handleSearch = async () => {
    if (!location.trim()) {
      message.warning('Введите локацию для поиска');
      return;
    }

    setLoading(true);
    
    try {
      const result = await ApiService.searchPlatforms(location.trim());
      
      if (result.success && result.data) {
        setResults(result.data.platforms);
        setLastSearchedLocation(result.data.location);
        onSearchComplete(result.data.platforms, result.data.location);
        message.success(`Найдено ${result.data.platforms.length} рекламных площадок`);
      } else {
        message.error(result.errorMessage || 'Ошибка при поиске');
        setResults([]);
      }
    } catch (error) {
      message.error('Ошибка при выполнении поиска');
      setResults([]);
    } finally {
      setLoading(false);
    }
  };

  const handleClear = () => {
    setLocation('');
    setResults([]);
    setLastSearchedLocation('');
    onSearchComplete([], '');
  };

  const handleKeyPress = (e: React.KeyboardEvent) => {
    if (e.key === 'Enter') {
      handleSearch();
    }
  };

  return (
    <Card>
      <Title level={4}>Поиск рекламных площадок</Title>
      <Text type="secondary">
        Введите локацию для поиска подходящих рекламных площадок
      </Text>
      
      <div style={{ marginTop: 16 }}>
        <Space.Compact style={{ width: '100%', display: 'flex' }}>
          <AutoComplete
            placeholder="Начните вводить локацию..."
            value={location}
            onChange={setLocation}
            onSelect={setLocation}
            onKeyDown={handleKeyPress}
            options={locationOptions}
            style={{ flex: 1, minWidth: 0 }}
            filterOption={(inputValue, option) =>
              !!(option?.value?.toLowerCase().includes(inputValue.toLowerCase()) ||
              option?.label?.toLowerCase().includes(inputValue.toLowerCase()))
            }
            size="middle"
          />
          <Button 
            type="primary" 
            icon={<SearchOutlined />}
            onClick={handleSearch}
            loading={loading}
            size="middle"
          >
            <span className="search-button-text">Поиск</span>
          </Button>
          <Button 
            icon={<ClearOutlined />}
            onClick={handleClear}
            size="middle"
          >
            <span className="clear-button-text">Очистить</span>
          </Button>
        </Space.Compact>
      </div>

      {results.length > 0 && (
        <div style={{ marginTop: 24 }}>
          <Title level={5}>
            Результаты поиска для локации: <Tag color="blue">{lastSearchedLocation}</Tag>
          </Title>
          <Text type="secondary">Найдено площадок: {results.length}</Text>
          
          <List
            size="small"
            dataSource={results}
            renderItem={(platform) => (
              <List.Item>
                <Tag color="green">{platform}</Tag>
              </List.Item>
            )}
            style={{ marginTop: 16 }}
          />
        </div>
      )}

      {results.length === 0 && lastSearchedLocation && (
        <div style={{ marginTop: 24 }}>
          <Text type="secondary">
            Для локации <Tag color="blue">{lastSearchedLocation}</Tag> рекламные площадки не найдены
          </Text>
        </div>
      )}
    </Card>
  );
};

export default LocationSearch;
