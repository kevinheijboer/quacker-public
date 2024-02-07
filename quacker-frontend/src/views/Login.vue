<template>
  <div class="login">
    <div class="login-main">
      <div class="login-header">
        <img class="logo" src="@/assets/logo.png" alt="logo" />
        <h1>Log in to Quacker</h1>
      </div>
      <div v-if="errorMessage" class="error-container">
        <p>{{ errorMessage }}</p>
      </div>
      <div class="login-body">
        <md-field>
          <label>Email or username</label>
          <md-input id="username-input" v-model="user.username"></md-input>
        </md-field>
        <md-field>
          <label>Password</label>
          <md-input
            id="password-input"
            v-model="user.password"
            type="password"
          ></md-input>
        </md-field>
        <md-button
          id="login-button"
          :disabled="validLogin"
          @click="handleLogin"
          class="login-button md-raised md-primary"
          >Log in</md-button
        >
      </div>

      <div class="login-footer">
        <RegistrationDialog />
      </div>
    </div>
  </div>
</template>

<script>
import RegistrationDialog from '@/components/RegistrationDialog.vue';
import User from '@/models/user';

export default {
  data() {
    return {
      user: new User(null, null),
      errorMessage: null,
    };
  },
  methods: {
    handleLogin() {
      this.$store
        .dispatch('auth/login', this.user)
        .then(() => {
          this.$router.push('/');
        })
        .catch((error) => {
          if (!error.response) {
            // network error
            this.errorMessage =
              'Network Error: the server could not be reached';
          } else if (error.response.status === 502) {
            this.errorMessage =
              'Gateway error: the service could not be reached';
          } else if (error.response.status === 400) {
            this.errorMessage = error.response.data.title;
          } else if (error.response.status === 401) {
            this.errorMessage =
              'The username and password you entered did not match our records. Please double-check and try again.';
          } else {
            this.errorMessage = 'Something went wrong';
          }
        });
    },
  },
  computed: {
    validLogin() {
      if (this.user.username && this.user.password) {
        return false;
      }
      return true;
    },
  },
  components: {
    RegistrationDialog,
  },
};
</script>

<style scoped src="@/styles/login.css"></style>
