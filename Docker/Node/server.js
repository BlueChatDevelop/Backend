const express = require('express');
const cors = require('cors');
const { createProxyMiddleware } = require('http-proxy-middleware');

const app = express();

app.use(cors());

app.use(
    '/api/authorization',
    createProxyMiddleware({
        target: 'http://api:5000/api/authorization', 
        changeOrigin: true,
        pathRewrite: {
            '^/api/authorization': '/api/authorization', 
        },
        logLevel: 'debug',
        onError: (err, req, res) => {
            console.error('Proxy error:', err);
            res.status(500).send('Proxy error occurred.');
        }
    })
);

app.get('/', (req, res) => {
    res.send('Node.js сервер работает и проксирует запросы на .NET API!');
});

app.listen(8000, () => {
    console.log('Node.js сервер запущен на http://localhost:8000');
});
