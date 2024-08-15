<script setup lang="ts">
import { RouterView } from 'vue-router'
import FooterComponent from './components/nav/FooterComponent.vue';
import HeaderComponent from './components/nav/HeaderComponent.vue';
</script>

<template>
<header-component></header-component>
<main class="container">
  <router-view></router-view>
</main>
<footer-component></footer-component>
</template>

<style scoped>
</style>
