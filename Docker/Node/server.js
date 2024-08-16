const express = require('express');
const cors = require('cors');
const { createProxyMiddleware } = require('http-proxy-middleware');

const app = express();

// Разрешаем CORS для всех источников
app.use(cors());

// Проксирование запросов на /api/authorization/* к http://localhost:5000
app.use('/api/authorization', createProxyMiddleware({
    target: 'http://localhost:5000',
    changeOrigin: true,
    pathRewrite: {
        '^/api/authorization': '/api/authorization', // Переписываем URL для соответствия API
    }
}));

// Пример маршрута для проверки работы сервера
app.get('/', (req, res) => {
    res.send('Node.js сервер работает и проксирует запросы на .NET API!');
});

// Запуск сервера на порту 8000
app.listen(8000, () => {
    console.log('Node.js сервер запущен на http://localhost:8000');
});
