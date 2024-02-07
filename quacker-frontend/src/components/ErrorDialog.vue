<template>
  <div class="alert">
    <span class="closebtn">&times;</span>
    {{ errorMessage }}
  </div>
</template>

<script>
export default {
  props: ['errorMessage'],
  methods: {},
};
</script>

<style scoped>
/* The alert message box */
.alert {
  padding: 20px;
  background-color: #f8d7da;
  border-color: #f5c6cb;
  border: 1px solid transparent;
  border-radius: 0.25rem;
  color: #721c24;
  margin-bottom: 15px;
  width: 100vw;
  position: absolute;
  left: 0;
  top: 0;
  z-index: 12;
}

/* The close button */
.closebtn {
  margin-left: 15px;
  color: inherit;
  font-weight: bold;
  float: right;
  font-size: 22px;
  line-height: 20px;
  cursor: pointer;
  transition: 0.3s;
}

/* When moving the mouse over the close button */
.closebtn:hover {
  color: black;
}
</style>
