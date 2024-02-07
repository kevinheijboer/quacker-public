import Vue from 'vue';
import App from './App.vue';
import router from './router';
import store from './store';
import VueMaterial from 'vue-material';
import 'vue-material/dist/vue-material.min.css';
import VueMq from 'vue-mq';
import {
  setInteractionMode,
  ValidationProvider,
  ValidationObserver,
} from 'vee-validate';

Vue.use(VueMq, {
  breakpoints: {
    // default breakpoints - customize this
    sm: 1004,
    md: 1282,
    lg: Infinity,
  },
  defaultBreakpoint: 'sm', // customize this for SSR
});
Vue.use(VueMaterial);

Vue.component('ValidationProvider', ValidationProvider);
Vue.component('ValidationObserver', ValidationObserver);

Vue.config.productionTip = false;

Vue.config.errorHandler = (err, vm, info) => {
  if (process.env.NODE_ENV !== 'production') {
    // Show any error but this one
    if (err.message !== "Cannot read property 'badInput' of undefined") {
      console.error(err);
    }
  }
};

document.title = process.env.VUE_APP_TITLE;

Vue.prototype.$errorMessage = 'My App';

new Vue({
  router,
  store,
  render: (h) => h(App),
}).$mount('#app');
