<template>
  <div class="whotofollow">
    <div class="follow-container">
      <div class="follow-header">
        <h2>Who to follow</h2>
      </div>

      <div class="follow-body">
        <div
          v-if="errorMessage"
          class="error-container"
          style="margin: 15px; padding: 15px"
        >
          <p>{{ errorMessage }}</p>
        </div>
        <div class="loading-container" v-if="!usersLoaded">
          <md-progress-spinner md-mode="indeterminate"></md-progress-spinner>
        </div>
        <div v-else v-for="user in users" :key="user.userId" class="user-row">
          <img
            class="profile-picture"
            :src="S3 + user.username"
            alt="profile picture"
            @error="replaceByDefault"
          />
          <div class="username-container">
            <h3
              @click="
                $router.push({
                  name: 'Profile',
                  params: { username: user.username },
                })
              "
              class="display-name"
            >
              {{ user.username }}
            </h3>
            <p class="username">@{{ user.username }}</p>
          </div>
          <md-button
            @click="follow(user.userId)"
            class="follow-button md-dense md-raised md-primary"
            >Follow</md-button
          >
        </div>
      </div>

      <div class="follow-footer"></div>
    </div>

    <div class="trends-container">
      <div class="follow-header">
        <h2>Trends</h2>
      </div>

      <div class="follow-body">
        <div class="loading-container" v-if="!trendsLoaded">
          <md-progress-spinner md-mode="indeterminate"></md-progress-spinner>
        </div>
        <div v-else v-for="trend in trends" :key="trend.id" class="user-row">
          <div class="username-container">
            <h3 class="display-name">
              {{ trend }}
            </h3>
          </div>
        </div>
      </div>
      <div class="follow-footer"></div>
    </div>
  </div>
</template>

<script>
import FollowService from '../services/follow.service';
import QuackService from '../services/quack.service';

export default {
  name: 'Navbar',
  data() {
    return {
      users: [],
      trends: [],
      usersLoaded: false,
      trendsLoaded: false,
      errorMessage: null,
    };
  },
  methods: {
    follow(userId) {
      FollowService.follow(userId).then((x) => {
        FollowService.getUnfollowedUsers().then((response) => {
          this.users = response.data;
        });
      });
    },
    replaceByDefault(e) {
      e.target.src =
        'https://pv-c.nl/wp-content/uploads/2011/08/person-placeholder.jpg';
    },
  },
  computed: {
    S3() {
      return process.env.VUE_APP_S3;
    },
  },
  mounted() {
    FollowService.getUnfollowedUsers()
      .then((response) => {
        this.users = response.data;
        this.usersLoaded = true;
      })
      .catch((error) => {
        this.usersLoaded = true;
        if (!error.response) {
          // network error
          this.errorMessage = 'Network Error: the server could not be reached';
        } else if (error.response.status === 502) {
          this.errorMessage = 'Gateway error: the service could not be reached';
        } else if (error.response.status === 400) {
          this.errorMessage = error.response.data.Errors[0];
        } else {
          this.errorMessage = 'Something went wrong';
        }
      });
    QuackService.getTrends().then((response) => {
      this.trends = response.data;
      this.trendsLoaded = true;
    });
  },
};
</script>

<style lang="css" scoped src="@/styles/whotofollow.css"></style>
