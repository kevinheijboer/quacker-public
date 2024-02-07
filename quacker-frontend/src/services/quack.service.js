import axios from '@/axios';
import authHeader from './auth-header';

const ENDPOINT = '/quacks';

class QuackService {
  async getQuacks() {
    return await axios.get(ENDPOINT, { headers: authHeader() });
  }

  async getQuack(quackId) {
    return await axios.get(`${ENDPOINT}/${quackId}`, { headers: authHeader() });
  }

  async deleteQuack(quackId) {
    return await axios.delete(`${ENDPOINT}/${quackId}`, {
      headers: authHeader(),
    });
  }

  async getQuacksFromUser(username) {
    return await axios.get(`${ENDPOINT}/user/${username}`, {
      headers: authHeader(),
    });
  }

  async postQuack(quack) {
    return await axios.post(ENDPOINT, quack, { headers: authHeader() });
  }

  async getTrends() {
    return await axios.get('/trends', { headers: authHeader() });
  }
}

export default new QuackService();
