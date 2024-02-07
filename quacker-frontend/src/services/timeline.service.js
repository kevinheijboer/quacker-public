import axios from '@/axios';
import authHeader from './auth-header';

const ENDPOINT = '/timeline';

class TimelineService {
  async getTimeline() {
    return await axios.get(ENDPOINT, { headers: authHeader() });
  }
}

export default new TimelineService();
