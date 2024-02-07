import axios from '@/axios';
import authHeader from './auth-header';

const ENDPOINT = '/follow';

class FollowService {
  async follow(userId) {
    let request = {
      userId: userId,
    };
    return await axios.post(ENDPOINT, request, { headers: authHeader() });
  }

  async unfollow(userId) {
    let request = {
      userId: userId,
    };
    return await axios.post(ENDPOINT + '/unfollow', request, {
      headers: authHeader(),
    });
  }

  async getUnfollowedUsers() {
    return await axios.get(ENDPOINT, { headers: authHeader() });
  }

  async getFollowCount(username) {
    return await axios.get(ENDPOINT + `/followcount/${username}`, {
      headers: authHeader(),
    });
  }

  async getFollowers(username) {
    return await axios.get(`/followers/${username}`, {
      headers: authHeader(),
    });
  }

  async getFollowing(username) {
    return await axios.get(`/following/${username}`, {
      headers: authHeader(),
    });
  }

  async checkIfFollowing(username) {
    return await axios.get(ENDPOINT + `/isfollowing/${username}`, {
      headers: authHeader(),
    });
  }
}

export default new FollowService();
