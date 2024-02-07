<template>
  <div>
    <md-dialog :md-active.sync="showEditDialog">
      <div class="logo-container">
        <img class="logo" src="@/assets/logo.png" alt="logo" />
        <md-button
          @click="showEditDialog = false"
          class="close-button md-dense md-raised md-primary"
          >Close</md-button
        >
      </div>

      <md-dialog-title>Edit profile</md-dialog-title>

      <md-dialog-content>
        <ValidationObserver ref="form">
          <md-field>
            <label>Profile picture</label>
            <md-file
              @md-change="handleFileUpload($event)"
              ref="profilePicture"
              id="profilePicture"
              accept="image/*"
            />
          </md-field>
          <md-field>
            <label>Name</label>
            <md-input maxlength="50" v-model="editProfile.name"></md-input>
          </md-field>
          <md-field>
            <label>Bio</label>
            <md-textarea
              maxlength="160"
              v-model="editProfile.bio"
            ></md-textarea>
          </md-field>
          <md-field>
            <label>Location</label>
            <md-input maxlength="30" v-model="editProfile.location"></md-input>
          </md-field>
          <md-field>
            <label>Website</label>
            <md-input maxlength="100" v-model="editProfile.website"></md-input>
          </md-field>
          <md-button
            @click="saveProfile"
            class="signup-button md-raised md-primary"
            >Save</md-button
          >
        </ValidationObserver>
      </md-dialog-content>

      <md-dialog-actions></md-dialog-actions>
    </md-dialog>

    <md-button
      @click="showEditDialog = true"
      class="edit-profile-button md-dense md-raised md-primary"
      >Edit profile</md-button
    >
  </div>
</template>

<script>
import AccountService from '../services/account.service';

export default {
  data() {
    return {
      showEditDialog: false,
      editProfile: {
        name: null,
        bio: null,
        location: null,
        website: null,
      },
      profilePicture: null,
    };
  },
  methods: {
    saveProfile() {
      AccountService.updateAccount(
        this.$store.state.auth.user.userId,
        this.editProfile,
        this.profilePicture
      ).then(() => {
        this.showEditDialog = false;
        this.editProfile.name = null;
        this.editProfile.bio = null;
        this.editProfile.location = null;
        this.editProfile.website = null;
        this.profilePicture = null;

        this.$router.go();
      });
    },
    handleFileUpload(evt) {
      this.profilePicture = evt[0];
    },
  },
  computed: {
    currentUser() {
      return this.$store.state.auth.user;
    },
  },
  mounted() {
    AccountService.getAccount(this.$route.params.username).then((response) => {
      this.editProfile.name = response.data.name;
      this.editProfile.bio = response.data.bio;
      this.editProfile.location = response.data.location;
      this.editProfile.website = response.data.website;
    });
  },
};
</script>

<style scoped src="@/styles/editprofile.css"></style>
