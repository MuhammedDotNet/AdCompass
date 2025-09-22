import React, { useState } from 'react';
import { Upload, Button, Card, Typography, App } from 'antd';
import { InboxOutlined } from '@ant-design/icons';
import { ApiService } from '../services/api';

const { Dragger } = Upload;
const { Title, Text } = Typography;

interface FileUploadProps {
  onUploadSuccess: (count: number) => void;
}

const FileUpload: React.FC<FileUploadProps> = ({ onUploadSuccess }) => {
  const [loading, setLoading] = useState(false);
  const { message } = App.useApp();

  const handleFileUpload = async (file: File) => {
    setLoading(true);
    
    try {
      const content = await file.text();
      const result = await ApiService.uploadPlatforms(content);
      
      if (result.success) {
        message.success(`Успешно загружено ${result.data?.loadedCount || 0} рекламных площадок`);
        onUploadSuccess(result.data?.loadedCount || 0);
      } else {
        message.error(result.errorMessage || 'Ошибка при загрузке файла');
      }
    } catch (error) {
      message.error('Ошибка при обработке файла');
    } finally {
      setLoading(false);
    }
  };

  const uploadProps = {
    name: 'file',
    multiple: false,
    accept: '.txt',
    beforeUpload: (file: File) => {
      handleFileUpload(file);
      return false; // Предотвращаем автоматическую загрузку
    },
    showUploadList: false,
  };

  return (
    <Card>
      <Title level={4}>Загрузка рекламных площадок</Title>
      <Text type="secondary">
        Загрузите текстовый файл с рекламными площадками в формате:
        <br />
        <code>Название площадки:/локация1,/локация2</code>
      </Text>
      
      <Dragger {...uploadProps} style={{ marginTop: 16 }}>
        <p className="ant-upload-drag-icon">
          <InboxOutlined />
        </p>
        <p className="ant-upload-text">
          Нажмите или перетащите файл в эту область для загрузки
        </p>
        <p className="ant-upload-hint">
          Поддерживаются только .txt файлы
        </p>
      </Dragger>
      
      <div style={{ marginTop: 16 }}>
        <Button 
          type="primary" 
          loading={loading}
          onClick={() => {
            const exampleData = `Яндекс.Директ:/ru
Ревдинский рабочий:/ru/svrd/revda,/ru/svrd/pervik
Газета уральских москвичей:/ru/msk,/ru/permobl,/ru/chelobl
Крутая реклама:/ru/svrd`;
            handleFileUpload(new File([exampleData], 'example.txt', { type: 'text/plain' }));
          }}
        >
          Загрузить пример данных
        </Button>
      </div>
    </Card>
  );
};

export default FileUpload;
