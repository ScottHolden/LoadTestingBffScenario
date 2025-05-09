const express = require('express');
const axios = require('axios');

const app = express();
const PORT = 3000;
const BACKEND_URL = 'http://localhost:5137/api/invoke'; // Replace with the actual backend URL

app.get('/invoke', async (req, res) => {
    try {
        const response = await axios.get(BACKEND_URL);
        res.json(response.data);
    } catch (error) {
        res.status(500).json({ error: 'Failed to fetch data from backend' });
    }
});

app.listen(PORT, () => {
    console.log(`Server is running on http://localhost:${PORT}`);
});