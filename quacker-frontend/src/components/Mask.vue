<template>
  <div class="mask"></div>
</template>

<script>
export default {};
</script>

<style scoped>
.mask {
  position: fixed;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
  width: 100%;
  height: 100%;
  z-index: -1;
}
</style>
