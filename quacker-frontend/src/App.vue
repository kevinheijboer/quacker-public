<template>
  <div id="app">
    <div v-if="this.$route.name != 'Login'">
      <div class="row" v-if="mobile">
        <div class="col-3 nav-col">
          <NavbarMobile />
        </div>
        <div class="col-13 main-col">
          <router-view :key="$route.path" />
        </div>
      </div>
      <div class="row" v-else-if="tablet">
        <div class="col-2 nav-col">
          <NavbarMobile />
        </div>
        <div class="col-9 main-col">
          <router-view :key="$route.path" />
        </div>
        <div v-if="!mobile" class="col-5 side-col">
          <WhoToFollow />
        </div>
      </div>
      <div class="row" v-else>
        <div class="col-5 nav-col">
          <Navbar />
        </div>
        <div class="col-5 main-col">
          <router-view :key="$route.path" />
        </div>
        <div v-if="!mobile" class="col-6 side-col">
          <WhoToFollow />
        </div>
      </div>
    </div>
    <router-view v-else />
  </div>
</template>

<script>
import Navbar from '@/components/Navbar.vue';
import NavbarMobile from '@/components/NavbarMobile.vue';
import WhoToFollow from '@/components/WhoToFollow.vue';

export default {
  name: 'Home',
  components: {
    Navbar,
    NavbarMobile,
    WhoToFollow,
  },
  methods: {},
  computed: {
    mobile() {
      const stackedBreakpoints = ['sm'];
      return stackedBreakpoints.includes(this.$mq);
    },
    tablet() {
      const stackedBreakpoints = ['md'];
      return stackedBreakpoints.includes(this.$mq);
    },
    desktop() {
      const stackedBreakpoints = ['sm', 'md'];
      return !stackedBreakpoints.includes(this.$mq);
    },
  },
};
</script>

<style lang="scss" src="@/styles/global.scss"></style>
