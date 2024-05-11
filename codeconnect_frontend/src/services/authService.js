import axios from 'axios';

const API_URL = 'http://localhost:80/api';

class AuthService {
    async login(user) {
        const response = await axios.post(
            `${API_URL}/auth/login`,
            { username: user.username, password: user.password},
            {
                headers: { 'Content-Type': 'application/json'},
            });
        if (response.data.accessToken) {
            localStorage.setItem('user', JSON.stringify(response.data));
        }
        return response.data;
    }

    logout() {
        localStorage.removeItem('user');
    }

    async register(registerData) {
        const response =  await axios.post(`${API_URL}/auth/register`,
            { username: registerData.username, email: registerData.email, password: registerData.password},
            {
                headers: { 'Content-Type': 'application/json'},
            });
        return response.data;
    }

}
export default new AuthService();