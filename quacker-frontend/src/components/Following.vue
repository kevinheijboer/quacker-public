<template>
  <div v-if="user != null">
    <div class="title-container">
      <span @click="goTo('Home')" class="material-icons back-arrow">west</span>
      <h2>{{ user.name }}</h2>
    </div>
    <div class="following-options">
      <div class="followers">
        <p
          @click="
            $router.push({
              name: 'Followers',
              params: { username: user.username },
            })
          "
        >
          Followers
        </p>
      </div>
      <div class="following"><p class="active">Following</p></div>
    </div>
    <hr />
    <div class="quack-row" v-for="user in following" :key="user.userId">
      <div class="quack-picture-col">
        <img
          class="profile-picture"
          :src="S3 + user.username"
          alt="profile picture"
          @error="replaceByDefault"
        />
        <h3
          @click="
            $router.push({
              name: 'Profile',
              params: { username: user.username },
            })
          "
        >
          {{ user.username }}
        </h3>
      </div>
      <hr />
    </div>
  </div>
</template>

<script>
import FollowService from '../services/follow.service';
import AccountService from '../services/account.service';

export default {
  data() {
    return {
      user: null,
      following: [],
    };
  },
  methods: {
    goTo(pathName) {
      this.$router.push({ name: pathName }).catch(() => {});
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
  mounted() {
    FollowService.getFollowing(this.$route.params.username).then((response) => {
      this.following = response.data;
    });
    AccountService.getAccount(this.$route.params.username).then((response) => {
      this.user = response.data;
    });
  },
};
</script>

<style lang="css" scoped src="@/styles/following.css"></style>
