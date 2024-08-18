const express = require('express');
const cors = require('cors');
const { createProxyMiddleware } = require('http-proxy-middleware');

const app = express();

app.use(cors({
    origin: 'http://localhost:8080',
    credentials: true
}));


app.use(
    '/api/authorization',
    createProxyMiddleware({
        target: 'http://api:5000/api/authorization', 
        changeOrigin: true,
        secure: false,
        pathRewrite: {
            '^/api/authorization': '/api/authorization', 
        },
        logLevel: 'debug',
        onError: (err, req, res) => {
            console.error('Proxy error:', err);
            res.status(500).send('Proxy error occurred.');
        },
        onProxyRes: (proxyRes, req, res) => {
            proxyRes.headers['Access-Control-Allow-Origin'] = 'http://localhost:8080';
            proxyRes.headers['Access-Control-Allow-Credentials'] = 'true';
        }
    })
);
app.options('*', cors({
    origin: 'http://localhost:8080',
    credentials: true
}));

app.get('/', (req, res) => {
    res.send('Node.js server is running and proxying requests to .NET API!');
});

app.listen(8000, () => {
    console.log('Node.js server running on http://localhost:8000');
});
