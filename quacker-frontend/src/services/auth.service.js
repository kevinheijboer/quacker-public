import axios from '@/axios';
import moment from 'moment';
import Cookies from 'js-cookie';

const ENDPOINT = '/auth/';

class AuthService {
  login(user) {
    return axios
      .post(ENDPOINT + 'signin', {
        username: user.username,
        password: user.password,
      })
      .then((response) => {
        if (response.data.token) {
          Cookies.set('user', JSON.stringify(response.data), {
            expires: new Date(response.data.tokenExpiry),
          });
        }
        return response.data;
      });
  }
  logout() {
    Cookies.remove('user');
  }
  async register(user) {
    return axios.post(ENDPOINT + 'register', {
      username: user.username,
      password: user.password,
      email: user.email,
      birthdate: moment(
        `${user.birthdate.day}-${user.birthdate.month}-${user.birthdate.year}`,
        'DD-MM-YYYY'
      ).toDate(),
    });
  }
}

export default new AuthService();
