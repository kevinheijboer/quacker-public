<template>
  <div>
    <md-dialog :md-active.sync="showRegistrationDialog">
      <div class="logo-container">
        <img class="logo" src="@/assets/logo.png" alt="logo" />
        <md-button
          @click="showRegistrationDialog = false"
          class="close-button md-dense md-raised md-primary"
          >Close</md-button
        >
      </div>

      <md-dialog-title>Create your account</md-dialog-title>
      <md-dialog-content>
        <div v-if="errorMessage" class="error-container">
          <p>{{ errorMessage }}</p>
        </div>
        <ValidationObserver ref="form">
          <ValidationProvider
            name="username"
            rules="required"
            v-slot="{ errors }"
          >
            <md-field :class="{ 'md-invalid': errors.length > 0 }">
              <label>Username</label>
              <md-input
                id="registration-username-input"
                v-model="registration.username"
              ></md-input>
              <span class="md-error">{{ errors[0] }}</span>
            </md-field>
          </ValidationProvider>
          <ValidationProvider
            name="email"
            rules="email|required"
            v-slot="{ errors }"
          >
            <md-field :class="{ 'md-invalid': errors.length > 0 }">
              <label>Email</label>
              <md-input
                id="registration-email-input"
                v-model="registration.email"
              ></md-input>
              <span class="md-error">{{ errors[0] }}</span>
            </md-field>
          </ValidationProvider>
          <ValidationProvider
            name="password"
            rules="required|min:3|containLetters|containNumbers"
            v-slot="{ errors }"
          >
            <md-field :class="{ 'md-invalid': errors.length > 0 }">
              <label>Password</label>
              <md-input
                id="registration-password-input"
                type="password"
                v-model="registration.password"
              ></md-input>
              <span class="md-error">{{ errors[0] }}</span>
            </md-field>
          </ValidationProvider>
          <div class="md-layout md-gutter">
            <div class="md-layout-item">
              <ValidationProvider
                name="month"
                rules="required"
                v-slot="{ errors }"
              >
                <md-field :class="{ 'md-invalid': errors.length > 0 }">
                  <label for="month">Month</label>
                  <md-select
                    v-model="registration.birthdate.month"
                    name="month"
                    id="month"
                  >
                    <md-option value="1">january</md-option>
                    <md-option value="2">february</md-option>
                    <md-option value="3">march</md-option>
                    <md-option value="4">april</md-option>
                    <md-option value="5">may</md-option>
                    <md-option value="6">june</md-option>
                    <md-option value="7">july</md-option>
                    <md-option value="8">august</md-option>
                    <md-option value="9">september</md-option>
                    <md-option value="10">october</md-option>
                    <md-option value="11">november</md-option>
                    <md-option value="12">december</md-option>
                  </md-select>
                  <span class="md-error">{{ errors[0] }}</span>
                </md-field>
              </ValidationProvider>
            </div>

            <div class="md-layout-item">
              <ValidationProvider
                name="day"
                rules="required"
                v-slot="{ errors }"
              >
                <md-field :class="{ 'md-invalid': errors.length > 0 }">
                  <label for="day">Day</label>
                  <md-select
                    v-model="registration.birthdate.day"
                    name="day"
                    id="day"
                  >
                    <md-option v-for="day in days" :value="day" :key="day">{{
                      day
                    }}</md-option>
                  </md-select>
                  <span class="md-error">{{ errors[0] }}</span>
                </md-field>
              </ValidationProvider>
            </div>

            <div class="md-layout-item">
              <ValidationProvider
                name="year"
                rules="required"
                v-slot="{ errors }"
              >
                <md-field :class="{ 'md-invalid': errors.length > 0 }">
                  <label for="year">Year</label>
                  <md-select
                    v-model="registration.birthdate.year"
                    name="year"
                    id="year"
                  >
                    <md-option
                      v-for="year in years"
                      :value="year"
                      :key="year"
                      >{{ year }}</md-option
                    >
                  </md-select>
                  <span class="md-error">{{ errors[0] }}</span>
                </md-field>
              </ValidationProvider>
            </div>
          </div>
          <md-button
            :disabled="!validRegistration"
            @click="handleRegistration"
            class="signup-button md-raised md-primary"
            >Sign up</md-button
          >
        </ValidationObserver>
      </md-dialog-content>

      <md-dialog-actions></md-dialog-actions>
    </md-dialog>

    <a id="signup" @click="showRegistrationDialog = true"
      >Sign up for Quacker</a
    >
  </div>
</template>

<script>
import Validation from '../services/validation.js';

export default {
  data() {
    return {
      errorMessage: null,
      showRegistrationDialog: false,
      registration: {
        username: null,
        email: null,
        password: null,
        birthdate: {
          month: null,
          day: null,
          year: null,
        },
      },
    };
  },
  methods: {
    handleRegistration() {
      this.$store
        .dispatch('auth/register', this.registration)
        .then(() => {
          this.showRegistrationDialog = false;

          this.registration.username = null;
          this.registration.email = null;
          this.registration.password = null;
          this.registration.birthdate.month = null;
          this.registration.birthdate.day = null;
          this.registration.birthdate.year = null;
          this.registration.username = null;
          this.registration.username = null;
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
            this.errorMessage = error.response.data.Errors[0];
          } else {
            this.errorMessage = 'Something went wrong';
          }
        });
    },
    daysInMonth(month, year) {
      if (month == null) {
        return 31;
      } else if (year == null) {
        return new Date(2021, month, 0).getDate();
      } else {
        return new Date(year, month, 0).getDate();
      }
    },
  },
  computed: {
    validRegistration() {
      if (
        this.registration.username &&
        this.registration.email &&
        this.registration.password &&
        this.registration.birthdate.month &&
        this.registration.birthdate.year &&
        this.registration.birthdate.day
      ) {
        if (this.$refs.form) {
          if (this.$refs.form.flags.invalid) {
            return false;
          }
        }
        return true;
      }
      return false;
    },
    days() {
      return Array.from(
        {
          length: this.daysInMonth(
            this.registration.birthdate.month,
            this.registration.birthdate.year
          ),
        },
        (value, index) => 1 + index
      );
    },
    years() {
      const year = new Date().getFullYear();
      return Array.from(
        { length: year - 1900 },
        (value, index) => 1901 + index
      ).sort(function(a, b) {
        return b - a;
      });
    },
  },
};
</script>

<style scoped src="@/styles/registration.css"></style>
