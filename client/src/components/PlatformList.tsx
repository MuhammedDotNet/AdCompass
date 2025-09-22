import React, { useState, useEffect } from 'react';
import { Card, Typography, List, Button, Space, Tag, Spin, App } from 'antd';
import { ReloadOutlined, DeleteOutlined } from '@ant-design/icons';
import { ApiService } from '../services/api';
import type { AdvertisingPlatform } from '../types/platform.js';

const { Title, Text } = Typography;

interface PlatformListProps {
  refreshTrigger: number;
}

const PlatformList: React.FC<PlatformListProps> = ({ refreshTrigger }) => {
  const [platforms, setPlatforms] = useState<AdvertisingPlatform[]>([]);
  const [loading, setLoading] = useState(false);
  const { message } = App.useApp();

  const loadPlatforms = async () => {
    setLoading(true);
    
    try {
      const result = await ApiService.getAllPlatforms();
      
      if (result.success && result.data) {
        setPlatforms(result.data);
      } else {
        message.error(result.errorMessage || 'Ошибка при загрузке площадок');
      }
    } catch (error) {
      message.error('Ошибка при получении списка площадок');
    } finally {
      setLoading(false);
    }
  };

  const handleClearAll = async () => {
    try {
      const result = await ApiService.clearAll();
      
      if (result.success) {
        message.success('Все данные очищены');
        setPlatforms([]);
      } else {
        message.error(result.errorMessage || 'Ошибка при очистке данных');
      }
    } catch (error) {
      message.error('Ошибка при очистке данных');
    }
  };

  useEffect(() => {
    loadPlatforms();
  }, [refreshTrigger]);

  return (
    <Card>
      <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: 16 }}>
        <Title level={4}>Загруженные рекламные площадки</Title>
        <Space>
          <Button 
            icon={<ReloadOutlined />}
            onClick={loadPlatforms}
            loading={loading}
          >
            Обновить
          </Button>
          <Button 
            danger
            icon={<DeleteOutlined />}
            onClick={handleClearAll}
          >
            Очистить все
          </Button>
        </Space>
      </div>

      {loading ? (
        <div style={{ textAlign: 'center', padding: '50px 0' }}>
          <Spin size="large" />
        </div>
      ) : platforms.length > 0 ? (
        <List
          dataSource={platforms}
          renderItem={(platform) => (
            <List.Item>
              <div style={{ width: '100%' }}>
                <div style={{ marginBottom: 8 }}>
                  <Text strong>{platform.name}</Text>
                </div>
                <div>
                  <Text type="secondary">Локации: </Text>
                  {platform.locations.map((location, index) => (
                    <Tag key={index} color="blue" style={{ margin: '2px' }}>
                      {location}
                    </Tag>
                  ))}
                </div>
              </div>
            </List.Item>
          )}
        />
      ) : (
        <div style={{ textAlign: 'center', padding: '50px 0' }}>
          <Text type="secondary">Нет загруженных рекламных площадок</Text>
        </div>
      )}
    </Card>
  );
};

export default PlatformList;
