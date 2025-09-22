import React, { useState } from 'react';
import { Row, Col, Typography } from 'antd';
import { RocketOutlined } from '@ant-design/icons';
import FileUpload from '../components/FileUpload';
import LocationSearch from '../components/LocationSearch';
import PlatformList from '../components/PlatformList';

const { Title, Paragraph } = Typography;

const HomePage: React.FC = () => {
  const [refreshTrigger, setRefreshTrigger] = useState(0);

  const handleUploadSuccess = (_count: number) => {
    setRefreshTrigger(prev => prev + 1);
  };

  const handleSearchComplete = (_results: string[], _location: string) => {
  };

  return (
    <div className="homepage-container" style={{ 
      width: '100vw', 
      minHeight: '100vh',
      background: 'linear-gradient(135deg, #667eea 0%, #764ba2 100%)',
      margin: 0,
      position: 'relative',
      left: '50%',
      right: '50%',
      marginLeft: '-50vw',
      marginRight: '-50vw'
    }}>
      <div style={{ 
        maxWidth: '1400px', 
        margin: '0 auto',
        width: '100%'
      }}>
        <div style={{ 
          textAlign: 'center', 
          marginBottom: '32px',
          padding: '16px 0'
        }}>
          <div style={{ 
            display: 'flex', 
            alignItems: 'center', 
            justifyContent: 'center',
            gap: '12px',
            marginBottom: '12px',
            flexWrap: 'wrap'
          }}>
            <RocketOutlined style={{ 
              fontSize: 'clamp(32px, 6vw, 48px)', 
              color: 'white',
              filter: 'drop-shadow(2px 2px 4px rgba(0,0,0,0.3))'
            }} />
            <Title level={1} style={{ 
              color: 'white', 
              textShadow: '2px 2px 4px rgba(0,0,0,0.3)',
              margin: 0,
              fontSize: 'clamp(28px, 8vw, 48px)',
              lineHeight: 1.2
            }}>
              AdCompass
            </Title>
          </div>
          <Paragraph style={{ 
            fontSize: 'clamp(14px, 4vw, 20px)', 
            color: 'rgba(255,255,255,0.9)', 
            fontWeight: 500,
            margin: 0,
            lineHeight: 1.4
          }}>
            🚀 Умный сервис для подбора рекламных площадок по локациям
          </Paragraph>
        </div>

        <Row gutter={[16, 16]} style={{ marginBottom: '24px' }}>
          <Col xs={24} sm={24} md={12} lg={12} xl={12}>
            <FileUpload onUploadSuccess={handleUploadSuccess} />
          </Col>
          
          <Col xs={24} sm={24} md={12} lg={12} xl={12}>
            <LocationSearch onSearchComplete={handleSearchComplete} />
          </Col>
        </Row>

        <div style={{ width: '100%' }}>
          <PlatformList refreshTrigger={refreshTrigger} />
        </div>
      </div>
    </div>
  );
};

export default HomePage;
