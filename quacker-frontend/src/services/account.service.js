import axios from '@/axios';
import authHeader from './auth-header';

const ENDPOINT = '/accounts';

class AccountService {
  async getAccounts() {
    return await axios.get(ENDPOINT, { headers: authHeader() });
  }

  async getAccount(username) {
    return await axios.get(`${ENDPOINT}/${username}`, {
      headers: authHeader(),
    });
  }

  async updateAccount(userId, editProfileRequest, profilePicture) {
    var formData = new FormData();

    formData.append('profilePicture', profilePicture);
    formData.append('profile', JSON.stringify(editProfileRequest));

    return await axios.put(`${ENDPOINT}/${userId}`, formData, {
      headers: {
        Authorization: authHeader(true),
        'Content-Type': 'multipart/form-data',
      },
    });
  }
}

export default new AccountService();
