<template>
  <div>
    <div class="title-container">
      <h2>Home</h2>
    </div>
    <hr />
    <div class="quack-container">
      <div class="picture-col">
        <img
          class="profile-picture"
          :src="S3 + currentUser.username"
          alt="profile picture"
          @error="replaceByDefault"
        />
      </div>
      <div class="quack-col ">
        <md-field class="quack-textarea">
          <label>What's happening?</label>
          <md-textarea
            id="quack-textarea"
            :disabled="quackLoading"
            v-model="quackMessage"
            md-autogrow
          ></md-textarea>
        </md-field>
        <md-button
          :disabled="quackLoading"
          @click="postQuack"
          id="quack-button"
          class="quack-button md-dense md-raised md-primary"
          >Quack</md-button
        >
      </div>
    </div>
    <md-progress-bar
      v-if="quackLoading"
      class="quack-loading-bar"
      md-mode="indeterminate"
    ></md-progress-bar>
    <hr />
    <div class="break"></div>
    <hr />
    <div class="quacks">
      <div class="empty" v-if="quacks.length == 0">
        <template v-if="timelineLoaded">
          <h2>Welcome to Quacker</h2>
          <p>
            This is the best place to see what’s happening in your world. Find
            some people and topics to follow now.
          </p>
        </template>
        <md-progress-spinner
          v-else
          md-mode="indeterminate"
        ></md-progress-spinner>
      </div>
    </div>
    <div class="quack-row" v-for="quack in quacks" :key="quack.id">
      <div class="quack-picture-col">
        <div class="quack-header">
          <img
            class="profile-picture"
            :src="S3 + quack.username"
            alt="profile picture"
            @error="replaceByDefault"
          />
          <h3
            @click="
              $router.push({
                name: 'Profile',
                params: { username: quack.username },
              })
            "
          >
            {{ quack.username }}
          </h3>
          <p>{{ quack.createdOn | formatDate }}</p>
          <span
            v-if="isAdmin"
            @click="deleteQuack(quack.id)"
            class="material-icons bin"
          >
            delete
          </span>
        </div>
      </div>
      <div class="quacked-col">
        <template v-for="(word, index) in quack.message.split(/[ ,]+/)"
          ><span
            :key="index"
            v-if="word[0] == '@' && word.length > 1"
            class="mention"
            @click="
              $router.push({
                name: 'Profile',
                params: { username: word.substring(1) },
              })
            "
            >{{ word }} </span
          ><span
            :key="index"
            v-else-if="word[0] == '#' && word.length > 1"
            class="topic"
            >{{ word }} </span
          ><span :key="index" v-else>{{ word }} </span></template
        >
      </div>
      <hr />
    </div>
  </div>
</template>

<script>
import QuackService from '../services/quack.service';
import TimelineService from '../services/timeline.service';
import moment from 'moment';

export default {
  data() {
    return {
      quacks: [],
      quackMessage: null,
      quackLoading: false,
      timelineLoaded: false,
    };
  },
  methods: {
    postQuack() {
      this.quackLoading = true;
      let quack = {
        message: this.quackMessage,
      };
      QuackService.postQuack(quack)
        .then((response) => {
          this.quacks.push(response.data);
          this.quacks.sort(function(a, b) {
            return new Date(b.createdOn) - new Date(a.createdOn);
          });

          this.quackMessage = null;
          this.quackLoading = false;
        })
        .catch((error) => {
          this.quackMessage = null;
          this.quackLoading = false;
        });
    },
    replaceByDefault(e) {
      e.target.src =
        'https://pv-c.nl/wp-content/uploads/2011/08/person-placeholder.jpg';
    },
    deleteQuack(id) {
      QuackService.deleteQuack(id).then((response) => {
        this.timelineLoaded = false;
        TimelineService.getTimeline()
          .then((response) => {
            this.quacks = response.data;
            this.quacks.sort(function(a, b) {
              return new Date(b.createdOn) - new Date(a.createdOn);
            });
            this.timelineLoaded = true;
          })
          .catch((error) => {
            this.timelineLoaded = false;
          });
      });
    },
  },
  mounted() {
    TimelineService.getTimeline()
      .then((response) => {
        this.quacks = response.data;
        this.quacks.sort(function(a, b) {
          return new Date(b.createdOn) - new Date(a.createdOn);
        });
        this.timelineLoaded = true;
      })
      .catch((error) => {
        this.timelineLoaded = false;
      });
  },
  computed: {
    currentUser() {
      return this.$store.state.auth.user;
    },
    S3() {
      return process.env.VUE_APP_S3;
    },
    isAdmin() {
      return this.$store.state.auth.user.userRoles.includes('Administrator');
    },
  },
  filters: {
    formatDate: function(date) {
      if (date) {
        return moment(String(date)).format('MMMM Do YYYY');
      }
    },
  },
};
</script>

<style lang="css" scoped src="@/styles/quacks.css"></style>
