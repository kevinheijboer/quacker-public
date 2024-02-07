import Cookies from 'js-cookie';

export default function authHeader(multipart) {
  let user = JSON.parse(Cookies.get('user'));

  if (user && user.token) {
    if (multipart) {
      return 'Bearer ' + user.token;
    }
    return { Authorization: 'Bearer ' + user.token };
  } else {
    return {};
  }
}
