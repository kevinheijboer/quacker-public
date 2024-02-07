<template>
  <div class="navbar">
    <div>
      <div class="logo-container" @click="goTo('Home')">
        <img class="logo" src="@/assets/logo.png" alt="logo" />
        <h1>Quacker</h1>
      </div>
      <ul>
        <li>
          <md-button
            @click="goTo('Home')"
            class="nav-button md-dense md-raised md-primar"
          >
            <h2
              :class="{
                active: this.$route.name == 'Home',
              }"
            >
              <span class="material-icons">home</span>Home
            </h2></md-button
          >
        </li>
        <li>
          <md-button
            @click="goToProfile(currentUser.username)"
            class="nav-button md-dense md-raised md-primary"
            ><h2
              :class="{
                active: this.$route.name == 'Profile',
              }"
            >
              <span class="material-icons">person</span>Profile
            </h2></md-button
          >
        </li>
      </ul>
    </div>
    <div class="profile" v-if="currentUser != null">
      <transition name="appear">
        <div class="profile-card" v-if="toggleProfileCard">
          <div class="profile-card-row">
            <img
              class="profile-picture"
              :src="S3 + currentUser.username"
              alt="profile picture"
              @error="replaceByDefault"
            />
            <h3 class="username">{{ currentUser.username }}</h3>
          </div>
          <hr />
          <p class="logout" @click="logOut">
            Log out @{{ currentUser.username }}
          </p>
        </div>
      </transition>
      <maskBg
        v-if="toggleProfileCard"
        @click.native="toggleProfileCard = false"
      />
      <button
        @click="toggleProfileCard = !toggleProfileCard"
        class="nav-button account-button md-primary"
      >
        <div class="profile-container">
          <img
            class="profile-picture"
            :src="S3 + currentUser.username"
            alt="profile picture"
            @error="replaceByDefault"
          />
          <div class="username-container">
            <h3 class="display-name">{{ currentUser.username }}</h3>
            <p class="username">@{{ currentUser.username }}</p>
          </div>
          <span style="margin-left: 40px" class="material-icons">
            more_horiz
          </span>
        </div>
      </button>
    </div>
  </div>
</template>

<script>
import maskBg from './Mask';

export default {
  name: 'Navbar',
  data() {
    return {
      toggleProfileCard: false,
    };
  },
  components: {
    maskBg,
  },
  methods: {
    goTo(pathName) {
      this.$router.push({ name: pathName }).catch(() => {});
    },
    goToProfile(username) {
      this.$router
        .push({ name: 'Profile', params: { username: username } })
        .catch(() => {});
    },
    logOut() {
      this.$store.dispatch('auth/logout').then(() => {
        this.$router.push('/login');
      });
    },
    replaceByDefault(e) {
      e.target.src =
        'https://pv-c.nl/wp-content/uploads/2011/08/person-placeholder.jpg';
    },
  },
  computed: {
    currentUser() {
      return this.$store.state.auth.user;
    },
    S3() {
      return process.env.VUE_APP_S3;
    },
  },
};
</script>

<style lang="css" scoped src="@/styles/navbar.css"></style>
