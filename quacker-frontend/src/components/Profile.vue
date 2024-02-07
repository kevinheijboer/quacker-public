<template>
  <div>
    <div v-if="user != null && profileLoaded">
      <div class="title-container">
        <span @click="goTo('Home')" class="material-icons back-arrow"
          >west</span
        >
        <h2>{{ user.name }}</h2>
      </div>
      <hr />
      <div class="profile-container">
        <img
          v-if="user.profilePictureURL"
          class="profile-picture"
          :src="user.profilePictureURL"
          alt="profile picture"
        />
        <img
          v-else
          class="profile-picture"
          src="https://pv-c.nl/wp-content/uploads/2011/08/person-placeholder.jpg"
          alt="profile picture"
        />
        <div class="edit-container">
          <h2 class="name">{{ user.name }}</h2>
          <EditProfileDialog
            style="margin-left: auto;"
            v-if="currentUser.userId == user.userId"
          />
          <template v-else>
            <md-button
              @click="unfollow(user.userId)"
              v-if="followed"
              class="following-button md-dense md-raised md-primary"
              >Following</md-button
            >
            <md-button
              v-else
              @click="follow(user.userId)"
              class="follow-button md-dense md-raised md-primary"
              >Follow</md-button
            ></template
          >
        </div>
        <p class="light">@{{ user.username }}</p>
        <p>
          {{ user.bio }}
        </p>
        <div class="attributes">
          <span v-if="user.website" class="material-icons light">link</span>
          <p v-if="user.website">
            <a :href="'https://' + user.website">{{ user.website }}</a>
          </p>
          <span v-if="user.location" class="material-icons light">place</span>
          <p v-if="user.location" class="light">{{ user.location }}</p>
          <span v-if="user.createdOn" class="material-icons light"
            >calendar_today</span
          >
          <p v-if="user.createdOn" class="light">
            Joined {{ user.createdOn | formatAccountDate }}
          </p>
        </div>
        <div class="followcount">
          <p
            class="light"
            @click="
              $router.push({
                name: 'Following',
                params: { username: user.username },
              })
            "
          >
            <b>{{ followingCount }}</b> Following
          </p>
          <p
            class="light"
            @click="
              $router.push({
                name: 'Followers',
                params: { username: user.username },
              })
            "
          >
            <b>{{ followersCount }}</b> Followers
          </p>
        </div>
      </div>
      <hr />
      <div class="quacks">
        <div class="empty" v-if="quacks.length == 0">
          <h2 v-if="currentUser.userId == user.userId">
            You haven’t Quacked anything yet
          </h2>
          <h2 v-else>
            This person hasn’t Quacked anything yet
          </h2>
          <p v-if="currentUser.userId == user.userId">
            When you post quacks, they will show up here.
          </p>
          <p v-else>
            When they post quacks, they will show up here.
          </p>
        </div>
      </div>
      <div class="quack-row" v-for="quack in quacks" :key="quack.id">
        <div class="quack-picture-col">
          <img
            v-if="user.profilePictureURL"
            class="profile-picture"
            :src="user.profilePictureURL"
            alt="profile picture"
          />
          <img
            v-else
            class="profile-picture"
            src="https://pv-c.nl/wp-content/uploads/2011/08/person-placeholder.jpg"
            alt="profile picture"
          />
          <div class="quack-header">
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
    <div class="notfound" v-else-if="notFound">
      <h1>404</h1>
      <h2>This profile doesn't exist</h2>
    </div>
    <div class="loading-container" v-else>
      <md-progress-spinner md-mode="indeterminate"></md-progress-spinner>
    </div>
  </div>
</template>

<script>
import moment from 'moment';
import FollowService from '../services/follow.service';
import QuackService from '../services/quack.service';
import AccountService from '../services/account.service';
import EditProfileDialog from '@/components/EditProfileDialog.vue';

export default {
  data() {
    return {
      followersCount: 0,
      followingCount: 0,
      quacks: [],
      user: null,
      followed: false,
      notFound: false,
      profileLoaded: false,
    };
  },
  components: {
    EditProfileDialog,
  },
  methods: {
    goTo(pathName) {
      this.$router.push({ name: pathName }).catch(() => {});
    },
    follow(userId) {
      FollowService.follow(userId).then((x) => {
        this.followed = true;
      });
    },
    unfollow(userId) {
      FollowService.unfollow(userId).then((x) => {
        this.followed = false;
      });
    },
    deleteQuack(id) {
      QuackService.deleteQuack(id).then((response) => {
        QuackService.getQuacksFromUser(this.$route.params.username).then(
          (response) => {
            this.quacks = response.data;
            this.quacks.sort(function(a, b) {
              return new Date(b.createdOn) - new Date(a.createdOn);
            });
          }
        );
      });
    },
  },
  computed: {
    currentUser() {
      return this.$store.state.auth.user;
    },
    isAdmin() {
      return this.$store.state.auth.user.userRoles.includes('Administrator');
    },
  },
  mounted() {
    this.profileLoaded = false;
    FollowService.getFollowCount(this.$route.params.username).then(
      (response) => {
        this.followersCount = response.data.followersCount;
        this.followingCount = response.data.followingCount;
      }
    );
    FollowService.checkIfFollowing(this.$route.params.username).then(
      (response) => {
        this.followed = response.data;
      }
    );
    QuackService.getQuacksFromUser(this.$route.params.username).then(
      (response) => {
        this.quacks = response.data;
        this.quacks.sort(function(a, b) {
          return new Date(b.createdOn) - new Date(a.createdOn);
        });
      }
    );

    AccountService.getAccount(this.$route.params.username)
      .then((response) => {
        this.user = response.data;
        this.profileLoaded = true;
      })
      .catch((error) => {
        this.profileLoaded = false;
        if (error.response.status == '404') {
          this.notFound = true;
        }
      });
  },
  filters: {
    formatDate: function(date) {
      if (date) {
        return moment(String(date)).format('MMMM Do YYYY');
      }
    },
    formatAccountDate: function(date) {
      if (date) {
        return moment(String(date)).format('MMMM YYYY');
      }
    },
  },
};
</script>

<style lang="css" scoped src="@/styles/profile.css"></style>
